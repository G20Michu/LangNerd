using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using LangNerd.Server.Api.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
namespace LangNerd.Server.Api.Database;

public class DataLoader
{
    private readonly ApplicationDbContext _dbContext;

    public DataLoader(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private string ComputeFileHash(string filePath)
    {
        using var sha = SHA256.Create();
        using var stream = File.OpenRead(filePath);
        var hashBytes = sha.ComputeHash(stream);
        return Convert.ToHexString(hashBytes);
    }

    private List<WordModel> GetJsonWords(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"{filePath} does not exist.");

        var jsonString = File.ReadAllText(filePath);

        var rawWords = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        var words = new List<WordModel>();

        foreach (var rawWord in rawWords)
        {
            if (!rawWord.TryGetValue("word", out var wordText)) continue;
            if (!rawWord.TryGetValue("definition", out var defText)) defText = "";

            var definitions = defText.Split(';')
                                     .Select(d => new WordDefinition { Definition = d.Trim() })
                                     .Where(d => !string.IsNullOrWhiteSpace(d.Definition))
                                     .ToList();

            words.Add(new WordModel
            {
                Word = wordText.Trim(),
                Definitions = definitions
            });
        }

        return words;
    }

    private async Task ReloadDatabase(string filePath)
    {
        Console.WriteLine("Reloading database...");

        var jsonWords = GetJsonWords(filePath);
        var storedWords = await _dbContext.Words
        .Include(w => w.Definitions)
        .OrderBy(w => w.Id)
        .ToListAsync();

        var allDefinitions = new List<WordDefinition>();

        foreach (var word in storedWords)
        {
            foreach (var def in word.Definitions)
            {
                allDefinitions.Add(new WordDefinition
                {
                    Definition = def.Definition,
                });
            }
        }


        //if word was removed from json also remove that from database 
        foreach (var word in storedWords)
        {
            var existingJsonWord = jsonWords.FirstOrDefault(w => string.Equals(w.Word, word.Word, StringComparison.OrdinalIgnoreCase));
            if (existingJsonWord == null)
            {
                _dbContext.Words.RemoveRange(word);
            }
        }
        foreach (var jsonWord in jsonWords)
            {
                var existingWord = storedWords.FirstOrDefault(w =>
                    string.Equals(w.Word, jsonWord.Word, StringComparison.OrdinalIgnoreCase));

                if (existingWord != null)
                {
                    var dbDefs = existingWord.Definitions
                             .Select(d => d.Definition.Trim())
                             .ToList();

                    var jsonDefs = jsonWord.Definitions
                           .Select(d => d.Definition.Trim())
                           .ToList();
                    //                bool allDbDefsInJson = dbDefs.All(def => jsonDefs.Contains(def));
                    bool allDbDefsInJson = jsonDefs.All(def => dbDefs.Contains(def));
                    if (!allDbDefsInJson)
                    {
                        _dbContext.WordDefinitions.RemoveRange(existingWord.Definitions);

                        foreach (var jsonDef in jsonWord.Definitions)
                        {
                            var newWordDefinition = new WordDefinition
                            {
                                Definition = jsonDef.Definition.Trim(),
                                WordId = existingWord.Id  // bardzo ważne, żeby powiązać definicję z właściwym słowem
                            };

                            _dbContext.WordDefinitions.Add(newWordDefinition);
                        }

                    }

                }
                else
                {
                    // add new word
                    var newWord = new WordModel
                    {
                        Word = jsonWord.Word,
                        Definitions = jsonWord.Definitions.Select(d => new WordDefinition
                        {
                            Definition = d.Definition
                        }).ToList()
                    };

                    await _dbContext.Words.AddAsync(newWord);
                }
            }

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving to DB: " + ex);
        }
    }


    public async Task CheckAndLoad(string filePath)
    {
        Console.WriteLine("Checking words.json...");

        if (!File.Exists(filePath))
            throw new FileNotFoundException("words.json not found");

        var wordInfo = await _dbContext.WordInfo
            .OrderBy(w => w.Id)
            .FirstOrDefaultAsync();

        var computedHash = ComputeFileHash(filePath);

        if (wordInfo != null)
        {
            if (wordInfo.FileHash != computedHash) 
            {
                Console.WriteLine("Need to reload words.json to the database");
                wordInfo.FileHash = computedHash;
                await _dbContext.SaveChangesAsync();
                await ReloadDatabase(filePath);
            }
        }
        else
        {
            var newWordInfo = new WordInfo { FileHash = computedHash };
            await _dbContext.WordInfo.AddAsync(newWordInfo);
            await _dbContext.SaveChangesAsync();
            await ReloadDatabase(filePath);
        }
    }
}
