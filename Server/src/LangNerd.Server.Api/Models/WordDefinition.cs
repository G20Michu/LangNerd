namespace LangNerd.Server.Api.Models;

public class WordDefinition
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WordId { get; set; }
    public string Definition { get; set; }
}

