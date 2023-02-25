using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pokepanion.PokedexScraper.App;

class Program {
    // Output config
    static readonly bool PRETTY_JSON = true;
    static readonly string OUTPUT_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
    static readonly string OUTPUT_FILE_NAME = "UraniumPokedex";

    // Web scraper config
    const string WIKI_BASE_URL = @"https://pokemon-uranium.fandom.com";
    const string POKEDEX_ENDPOINT = "/wiki/Pokedex";

    async static Task Main() {

        var pokemon = new UraniumPokedexScraper().ScrapeValues(WIKI_BASE_URL, POKEDEX_ENDPOINT);

        // Write pokedex data to output json file
        string outputPath = Path.Combine(OUTPUT_DIRECTORY, $"{OUTPUT_FILE_NAME}.json");
        using var outFile = File.CreateText(outputPath);

        outFile.Write(JsonConvert.SerializeObject(await pokemon, PRETTY_JSON ? Formatting.Indented : Formatting.None));
        Console.WriteLine($"Pokedex written to {outputPath}");
    }
}
