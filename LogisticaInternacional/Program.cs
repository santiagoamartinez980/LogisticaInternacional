using System.Text.Json;
using LogisticaInternacional.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ContentRepository>();
builder.Services.AddSingleton<QuizEngine>();

var app = builder.Build();
app.UseStaticFiles();
app.MapRazorPages();

app.MapGet("/api/topics", (ContentRepository data) => Results.Ok(data.Theory
    .GroupBy(x => x.Topic).OrderBy(g => g.Key)
    .Select(g => new { id = g.Key, title = g.First().TopicTitle, count = g.Count() })));
app.MapGet("/api/theory", (ContentRepository data, int? topic, string? q) =>
{
    var rows = data.Theory.AsEnumerable();
    if (topic.HasValue) rows = rows.Where(x => x.Topic == topic.Value);
    if (!string.IsNullOrWhiteSpace(q))
        rows = rows.Where(x => $"{x.Title} {x.Definition} {x.Explanation}".Contains(q, StringComparison.OrdinalIgnoreCase));
    return Results.Ok(rows);
});
app.MapGet("/api/questions", (QuizEngine quiz, int? topic, string? subtopic, string? difficulty, bool? errors, int? count, bool? random) =>
    Results.Ok(quiz.Select(topic, subtopic, difficulty, errors == true, count, random != false)));
app.MapGet("/api/glossary", (ContentRepository data, string? q) =>
    Results.Ok(data.Glossary.Where(x => string.IsNullOrWhiteSpace(q) ||
        $"{x.Term} {x.Definition}".Contains(q, StringComparison.OrdinalIgnoreCase))));
app.MapGet("/api/cases", (ContentRepository data) => Results.Ok(data.Cases));
app.MapGet("/api/resources", (ContentRepository data) => Results.Ok(data.Resources));
app.MapGet("/api/stats", (ContentRepository data) => Results.Ok(new { topics = data.Theory.Select(x => x.Topic).Distinct().Count(), subtopics = data.Theory.Count, questions = data.Questions.Count, glossaryTerms = data.Glossary.Count, cases = data.Cases.Count }));
app.Run();
