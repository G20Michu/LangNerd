namespace LangNerd.Server.Api.Models;

public class WordModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public  string Word { get; set; }
    public List<WordDefinition> Definitions { get; set; }
}
