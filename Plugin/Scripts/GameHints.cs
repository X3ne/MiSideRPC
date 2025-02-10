using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MiSideRPC.Utils;
using Newtonsoft.Json;

namespace MiSideRPC.Scripts;

public class HintGameData
{
    public string Name { get; set; }
    public string Chapter { get; set; }
    public string Version { get; set; }
    public string CoverImage { get; set; }
}

public class ChapterHints
{
    [JsonProperty("chapter")]
    public string Chapter { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("scene")]
    public string Scene { get; set; }

    [JsonProperty("hints")]
    public List<string> Hints { get; set; }
}

public class GameHints
{
    private static List<string> _chapters;
    private static List<ChapterHints> _hints;

    static GameHints()
    {
        LoadHintsFromJson();
    }
    
    private static void LoadHintsFromJson()
    {
        try
        {
            string dllDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(dllDirectory, "Assets", "game_hints.json");
            
            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine($"Error: JSON file not found at {jsonFilePath}");
                _chapters = new List<string>();
                _hints = new List<ChapterHints>();
                return;
            }

            string json = File.ReadAllText(jsonFilePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine("Error: JSON file is empty.");
                _chapters = new List<string>();
                _hints = new List<ChapterHints>();
                return;
            }

            var data = JsonConvert.DeserializeObject<GameHintsJson>(json);

            if (data == null)
            {
                Console.WriteLine("Error: Deserialized JSON is null.");
                _chapters = new List<string>();
                _hints = new List<ChapterHints>();
                return;
            }

            _chapters = data.Chapters ?? new List<string>();
            _hints = data.Hints ?? new List<ChapterHints>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading JSON file: {ex.Message}");
            _chapters = new List<string>();
            _hints = new List<ChapterHints>();
        }
    }

    public HintGameData? GetHintByText(string text)
    {
        text = text.ToLower();

        foreach (var chapter in _hints)
        {
            foreach (var hint in chapter.Hints)
            {
                // This is to avoid typo mistakes
                if (LevenshteinDistance.Compute(hint.ToLower(), text) < 5)
                {
                    return new HintGameData
                    {
                        Name = hint,
                        Chapter = chapter.Chapter,
                        Version = chapter.Version,
                        CoverImage = chapter.Scene
                    };
                }
            }
        }

        return null;
    }

    private class GameHintsJson
    {
        [JsonProperty("chapters")]
        public List<string> Chapters { get; set; }

        [JsonProperty("hints")]
        public List<ChapterHints> Hints { get; set; }
    }
}
