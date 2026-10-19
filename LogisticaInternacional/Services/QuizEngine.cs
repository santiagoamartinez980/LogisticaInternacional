namespace LogisticaInternacional.Services;

/// <summary>
/// Único motor de selección del banco. Todos los módulos consumen este servicio/API.
/// La selección nunca depende de progreso, prerrequisitos ni notas mínimas.
/// </summary>
public sealed class QuizEngine(ContentRepository data)
{
    public IReadOnlyList<Question> Select(int? topic, string? subtopic, string? difficulty,
        bool errorsOnly, int? count, bool random)
    {
        IEnumerable<Question> query = data.Questions;
        if (topic.HasValue) query = query.Where(q => q.Topic == topic.Value);
        if (!string.IsNullOrWhiteSpace(subtopic)) query = query.Where(q => q.SubtopicId == subtopic);
        if (!string.IsNullOrWhiteSpace(difficulty) && difficulty != "Todas")
            query = query.Where(q => q.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase));
        // Los errores se almacenan localmente en el navegador. El cliente puede filtrar
        // el banco por IDs fallados y seguir usando este mismo motor para las preguntas.
        if (random) query = query.OrderBy(_ => Random.Shared.Next());
        if (count is > 0) query = query.Take(Math.Min(count.Value, data.Questions.Count));
        return query.ToList();
    }
}
