using System;
using System.IO;
namespace LangNerd.Server.Api.Env;
class EnvLoader
{
    public static void Load(string Filepath)
    {
        if (!File.Exists(Filepath))
            throw new FileNotFoundException($"Not Found: {Filepath}");

        foreach (var line in File.ReadAllLines(Filepath))
        {
            var trimmed = line.Trim();

            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                continue;

            var parts = trimmed.Split('=', 2);
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                Console.WriteLine($"key {key} , value {value}");
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}
