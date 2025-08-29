namespace LangNerd.Server.Api.Models;

public class WordModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Word { get; set; }
    public List<WordDefinition> Definitions { get; set; }
}

public class WordDefinition
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WordId { get; set; }
    public string Definition { get; set; }
}

