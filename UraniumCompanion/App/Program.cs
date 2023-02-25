using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pokepanion.Library.ConsoleGui;
using Pokepanion.Library.GameProcessing;
using Pokepanion.Library.Helpers;
using Pokepanion.Library.Uranium;

namespace Pokepanion.UraniumCompanion.App;

class Program {

    static readonly string WINDOW_NAME = "Pokemon Uranium";
    static readonly string POKEDEX_DATA_LOCATION = "./App/UraniumPokedex.json";
    static readonly int AUTO_UPDATE_INTERVAL = 500;

    static SimpleConsoleGui? gui;
    static UraniumPokedex? pokedex;
    static GameWindow? gameWindow;
    static CancellationTokenSource? autoUpdateCancelToken;

    static int Main() {

        pokedex = LoadPokedex(POKEDEX_DATA_LOCATION);

        if (pokedex == null) {
            string[] helpOptions = {
                $"Is the Pokedex data located at {Path.GetFullPath(POKEDEX_DATA_LOCATION)}?",
                "Is the data formatted correctly? You can re-scrape it if you're not sure."
            };

            Console.WriteLine(ExitMessage.Build("Failed to load Uranium Pokedex.", helpOptions));
            return (int)ExitCode.Failure;
        }

        gui = new SimpleConsoleGui()
            .WithColoredHeader("Pokemon Uranium Companion", ConsoleColor.DarkGreen)
            .WithColoredSubheader($"Not watching any window", ConsoleColor.DarkGray)
            .WithColoredOptions(ConsoleColor.DarkGray)
            .WithColoredResponses(ConsoleColor.DarkGray);

        gui.AddKeyAction(
            ConsoleKey.G, null,
            "Relocate Game Window",
            () => TryRelocateGameWindow(WINDOW_NAME)
        );

        gui.AddKeyAction(
            ConsoleKey.A, null,
            "Analyze Current Frame",
            () => AnalyzeCurrentFrame()
        );

        gui.AddKeyAction(
            ConsoleKey.A, ConsoleModifiers.Control,
            "Debug Current Frame",
            () => AnalyzeCurrentFrame(true)
        );

        gui.AddKeyAction(
            ConsoleKey.S, null,
            "Update automatically",
            () => StartAutoUpdating(gui)
        );

        gui.AddKeyAction(
            ConsoleKey.S, ConsoleModifiers.Control,
            "Stop updating automatically",
            () => StopAutoUpdating()
        );

        gui.AddKeyAction(ConsoleKey.Q, null, "Exit", () => {
            Environment.Exit((int)ExitCode.Success);
            return null; // I mean... this is never going to matter...
        });

        // Try to auto-attach to game window
        TryRelocateGameWindow(WINDOW_NAME);

        while (true) {
            gui.ProcessNext();
        }
    }

    static string? TryRelocateGameWindow(string windowName) {
        gameWindow = TryGetGameWindow(windowName);

        if (gameWindow == null) {
            return $"Unable to locate exactly one window named '{windowName}'";
        }

        // When the game is closed, remove the reference and reset the title
        gameWindow.OnGameClosed += () => {
            gameWindow = null;
            gui?.WithColoredSubheader($"Not watching any window", ConsoleColor.DarkGray);
        };

        gui?.WithColoredSubheader($"Watching '{gameWindow.Title}'", ConsoleColor.DarkGray);
        return $"Now watching '{gameWindow.Title}'";
    }

    static string? StartAutoUpdating(BaseConsoleGui gui) {
        // Cancel running task if it exists, then create new
        autoUpdateCancelToken?.Cancel();
        autoUpdateCancelToken = new CancellationTokenSource();

        Task.Run(() => {
            while (!autoUpdateCancelToken.IsCancellationRequested) {
                gui.SendInteraction(ConsoleKey.A, null, true);
                Thread.Sleep(AUTO_UPDATE_INTERVAL);
            }

            return Task.FromCanceled(autoUpdateCancelToken.Token);
        });

        return null;
    }

    static string? StopAutoUpdating() {
        // Just cancel the running task
        autoUpdateCancelToken?.Cancel();
        return null;
    }

    static string AnalyzeCurrentFrame(bool showDebug = false) {

        if (gameWindow == null) {
            return "Not watching any window";
        }

        if (gameWindow.IsMinimized) {
            return "Window is minimized";
        }

        static Rectangle NameRectFromOrigin(int x, int y) => new(x, y, 215, 70);

        WindowInspector inspector = new(gameWindow);
        inspector.InspectRegion("Oponent 1", NameRectFromOrigin(20, 5))
                 .InspectRegion("Ally 1", NameRectFromOrigin(650, 385));

        StringBuilder output = new();

        if (showDebug) {
            inspector.ShowInspectedRegions();
        }

        if (!inspector.TryInspect(out Dictionary<string, string?> result)) {
            return "Failed to inspect game window";
        }

        foreach (var (regionName, regionText) in result) {
            if (regionText == null) {
                output.AppendLine($"{regionName}: No Pokemon" + (regionText != null ? $" ({regionText})" : ""));
                output.AppendLine();
                continue;
            }

            if (pokedex == null) {
                output.AppendLine($"{regionName}: Pokedex missing" + (regionText != null ? $" ({regionText})" : ""));
                output.AppendLine();
                continue;
            }

            UraniumPokemonInfo pokemonInfo = pokedex.GetBest(regionText, out float confidence);

            var immune = pokemonInfo.TypeEffectivenesses[UraniumEffectiveness.Immune];
            var doubleResisted = pokemonInfo.TypeEffectivenesses[UraniumEffectiveness.DoubleResisted];
            var resisted = pokemonInfo.TypeEffectivenesses[UraniumEffectiveness.Resisted];
            var normal = pokemonInfo.TypeEffectivenesses[UraniumEffectiveness.Normal];
            var weak = pokemonInfo.TypeEffectivenesses[UraniumEffectiveness.Weak];
            var superWeak = pokemonInfo.TypeEffectivenesses[UraniumEffectiveness.SuperWeak];

            output.AppendLine($"{regionName}:");
            output.AppendLine($"  ========= Basic Information ===========");
            output.AppendLine($"            Name: {pokemonInfo.Name} ({regionText} | {confidence})");
            output.AppendLine($"          Number: {pokemonInfo.Id}");
            output.AppendLine($"    Primary Type: {pokemonInfo.PrimaryType}");
            output.AppendLine($"  Secondary Type: {pokemonInfo.SecondaryType?.ToString() ?? "None"}");
            output.AppendLine();
            output.AppendLine($"  ========== Type Effectiveness =========");
            output.AppendLine($"           Immune: {string.Join(", ", immune.Select(x => x.ToString()))}");
            output.AppendLine($"  Double Resisted: {string.Join(", ", doubleResisted.Select(x => x.ToString()))}");
            output.AppendLine($"         Resisted: {string.Join(", ", resisted.Select(x => x.ToString()))}");
            output.AppendLine($"           Normal: {string.Join(", ", normal.Select(x => x.ToString()))}");
            output.AppendLine($"             Weak: {string.Join(", ", weak.Select(x => x.ToString()))}");
            output.AppendLine($"       Super Weak: {string.Join(", ", superWeak.Select(x => x.ToString()))}");
            output.AppendLine();
            output.AppendLine();
        }

        return output.ToString();
    }

    static GameWindow? TryGetGameWindow(string windowTitle) {
        try {
            return new GameWindow(windowTitle);
        } catch (Exception) {
            return null;
        }
    }

    static UraniumPokedex? LoadPokedex(string filePath) {
        try {
            string fileContents = File.ReadAllText(filePath);
            var uraniumDexInfo = JsonConvert.DeserializeObject<UraniumPokemonInfo[]>(fileContents);

            if (uraniumDexInfo == null) {
                return null;
            }

            return new UraniumPokedex(uraniumDexInfo);
        } catch (Exception) {
            return null;
        }
    }
}
