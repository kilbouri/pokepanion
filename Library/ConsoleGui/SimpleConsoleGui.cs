using System;

namespace Pokepanion.Library.ConsoleGui;

public class SimpleConsoleGui : BaseConsoleGui {

    private ConsoleColor? headerColor;
    private ConsoleColor? subheaderColor;
    private ConsoleColor? optionColor;
    private ConsoleColor? responseColor;

    private string? header;
    private string? subheader;

    /// <summary>
    /// Adds a default-colored header to the GUI.
    /// </summary>
    /// <param name="header">The header text</param>
    /// <returns>This object for chaining.</returns>
    public SimpleConsoleGui WithHeader(string header) {
        this.header = header;
        headerColor = null;
        return this;
    }

    /// <summary>
    /// Adds a colored header to the GUI.
    /// </summary>
    /// <param name="header">The header text</param>
    /// <param name="headerColor">The color of the header text</param>
    /// <returns>This object for chaining.</returns>
    public SimpleConsoleGui WithColoredHeader(string header, ConsoleColor headerColor) {
        this.header = header;
        this.headerColor = headerColor;
        return this;
    }

    /// <summary>
    /// Adds a default-colored subheader to the GUI.
    /// </summary>
    /// <param name="subheader">The subheader text</param>
    /// <returns>This object for chaining.</returns>
    public SimpleConsoleGui WithSubheader(string subheader) {
        this.subheader = subheader;
        return this;
    }

    /// <summary>
    /// Adds a colored subheader to the GUI.
    /// </summary>
    /// <param name="subheader">The subheader text</param>
    /// <param name="subheaderColor">The color of the subheader text</param>
    /// <returns>This object for chaining.</returns>
    public SimpleConsoleGui WithColoredSubheader(string subheader, ConsoleColor subheaderColor) {
        this.subheader = subheader;
        this.subheaderColor = subheaderColor;
        return this;
    }

    /// <summary>
    /// Sets the color of the options list
    /// </summary>
    /// <param name="color">The color of the options list, or null to reset.</param>
    /// <returns>This object for chaining.</returns>
    public SimpleConsoleGui WithColoredOptions(ConsoleColor? color) {
        optionColor = color;
        return this;
    }

    /// <summary>
    /// Sets the color of the action response block
    /// </summary>
    /// <param name="color">The color of the action response block, or null to reset.</param>
    /// <returns>This object for chaining.</returns>
    public SimpleConsoleGui WithColoredResponses(ConsoleColor? color) {
        responseColor = color;
        return this;
    }

    protected override void DrawMenu(ConsoleKeyModifierPair? lastInput, string? lastOutput) {
        Console.Clear();

        if (header != null) { WriteColoredString(header, headerColor); }
        if (subheader != null) { WriteColoredString(subheader, subheaderColor); }

        Console.WriteLine();

        int longestInputString = 0;

        foreach (var (input, _) in keyActions) {
            longestInputString = Math.Max(input.ToString().Length, longestInputString);
        }

        foreach (var (input, labeledAction) in keyActions) {
            WriteColoredString($"[{input.ToString().PadLeft(longestInputString)}] {labeledAction.Label}", optionColor);
        }

        if (lastOutput != null) {
            Console.WriteLine();
            WriteColoredString("Last command output:", responseColor);
            Console.WriteLine();
            WriteColoredString(lastOutput, responseColor);
        }

        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write("Input: ");
    }

    protected override void UpdateGuiBeforeActionInvoke(ConsoleKeyModifierPair? input) {
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write("Processing...".PadRight(Console.WindowWidth));
    }

    protected override void UpdateGuiAfterActionInvoke(ConsoleKeyModifierPair? input, string? output) {
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write("Input: ".PadRight(Console.WindowWidth));
    }

    private static void WriteColoredString(string str, ConsoleColor? color) {

        if (color.HasValue) {
            Console.ForegroundColor = color.Value;
        }

        Console.WriteLine(str);
        Console.ResetColor();
    }
}
