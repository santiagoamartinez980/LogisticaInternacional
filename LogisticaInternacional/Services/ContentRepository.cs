using System.Text.Json;

namespace LogisticaInternacional.Services;

public sealed class ContentRepository
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };
    public List<TheoryItem> Theory { get; }
    public List<Question> Questions { get; }
    public List<GlossaryItem> Glossary { get; }
    public List<CaseItem> Cases { get; }
    public List<ResourceItem> Resources { get; }

    public ContentRepository(IWebHostEnvironment env)
    {
        Theory = Load<TheoryItem>(env, "theory.json");
        Questions = Load<Question>(env, "questions.json");
        Glossary = Load<GlossaryItem>(env, "glossary.json");
        Cases = Load<CaseItem>(env, "cases.json");
        Resources = Load<ResourceItem>(env, "resources.json");
    }
    private static List<T> Load<T>(IWebHostEnvironment env, string file)
    {
        var path = Path.Combine(env.ContentRootPath, "Data", file);
        return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(path), Options) ?? new();
    }
}
public sealed record TheoryItem(string Id, int Topic, string TopicTitle, string Title, string Definition, string Explanation, string Example, string CommonError, string ReviewQuestion, List<string> KeyConcepts);
public sealed record Question(string Id, int Topic, string SubtopicId, string Difficulty, string Type, string Prompt, List<string> Options, int AnswerIndex, string Explanation);
public sealed record GlossaryItem(string Term, string Definition);
public sealed record CaseItem(string Id, int Topic, string Title, string Scenario, List<string> Tasks, string ModelAnswer);
public sealed record ResourceItem(int Topic, string Title, string Url, string Description);
