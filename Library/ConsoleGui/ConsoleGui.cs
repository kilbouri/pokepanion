using System;
using System.Collections.Generic;

namespace Pokepanion.Library.ConsoleGui;

public abstract class BaseConsoleGui {

    protected struct ConsoleKeyModifierPair {
        public ConsoleKey Key { get; init; }
        public ConsoleModifiers? Modifiers { get; init; }

        public bool Control => Modifiers.HasValue && ((int)Modifiers & (int)ConsoleModifiers.Control) != 0;
        public bool Shift => Modifiers.HasValue && ((int)Modifiers & (int)ConsoleModifiers.Shift) != 0;
        public bool Alt => Modifiers.HasValue && ((int)Modifiers & (int)ConsoleModifiers.Alt) != 0;

        public override string ToString() {
            List<string> sequence = new(4);

            if (Control) { sequence.Add("Ctrl"); }
            if (Shift) { sequence.Add("Shift"); }
            if (Alt) { sequence.Add("Alt"); }

            sequence.Add(ConsoleKeyToString(Key));

            return string.Join("+", sequence);
        }

        private static string ConsoleKeyToString(ConsoleKey key) => key switch {
            ConsoleKey.D0 => "0",
            ConsoleKey.D1 => "1",
            ConsoleKey.D2 => "2",
            ConsoleKey.D3 => "3",
            ConsoleKey.D4 => "4",
            ConsoleKey.D5 => "5",
            ConsoleKey.D6 => "6",
            ConsoleKey.D7 => "7",
            ConsoleKey.D8 => "8",
            ConsoleKey.D9 => "9",
            _ => key.ToString()
        };
    }

    protected struct LabeledAction {
        public Func<string?> Action { get; init; }
        public string Label { get; init; }
    }

    private ConsoleKeyModifierPair? lastInput;
    private string? lastOutput;

    protected readonly Dictionary<ConsoleKeyModifierPair, LabeledAction> keyActions = new();

    /// <summary>
    /// Draws the GUI menu.
    /// </summary>
    /// <param name="lastOutput">A string response from the last executed action</param>
    /// <returns>A boolean indicating whether the next input should be intercepted (not shown) or not.</returns>
    protected abstract void DrawMenu(ConsoleKeyModifierPair? lastInput, string? lastOutput);

    /// <summary>
    /// Allows updating the GUI just before an action invokes. It is recommended to show a message
    /// that indicates a command is processing.
    /// </summary>
    /// <param name="input">The input that triggered the action about to be invoked.</param>
    protected abstract void UpdateGuiBeforeActionInvoke(ConsoleKeyModifierPair? input);

    /// <summary>
    /// Allows updating the GUI just after an action completes.
    /// </summary>
    /// <param name="input">The input that triggered the action that just completed.</param>
    /// <param name="output">The output of the action that just completed.</param>
    protected abstract void UpdateGuiAfterActionInvoke(ConsoleKeyModifierPair? input, string? output);


    /// <summary>
    /// Register a menu item and handler.
    /// </summary>
    /// <param name="key">The modifier-less key that triggers this action</param>
    /// <param name="modifiers">Modifiers that should be present on the key in order to trigger this action</param>
    /// <param name="handleKey">An action that handles the input</param>
    /// <param name="actionName">A label for the action in the menu</param>
    /// <returns>This instance, for chaining</returns>
    public BaseConsoleGui AddKeyAction(ConsoleKey key, ConsoleModifiers? modifiers, string actionName, Func<string?> handleKey) {
        ConsoleKeyModifierPair dictKey = new() {
            Key = key,
            Modifiers = modifiers
        };

        LabeledAction action = new() {
            Action = handleKey,
            Label = actionName
        };

        keyActions[dictKey] = action;
        return this;
    }

    /// <summary>
    /// Allows sending programatic input to the GUI.
    /// </summary>
    /// <param name="key">The key to fake being pressed.</param>
    /// <param name="modifiers">Modifiers to fake being pressed.</param>
    public void SendInteraction(ConsoleKey key, ConsoleModifiers? modifiers, bool suppressInvokeHooks = false, bool redraw = true) {
        ConsoleKeyModifierPair actionKey = new() {
            Key = key,
            Modifiers = modifiers
        };

        ProcessInternal(actionKey, suppressInvokeHooks, redraw);
    }

    /// <summary>
    /// Draws the screen and processes the next user key.
    /// </summary>
    public void ProcessNext() {
        DrawMenu(lastInput, lastOutput);

        var keyInfo = Console.ReadKey(true);

        ConsoleKeyModifierPair actionKey = new() {
            Key = keyInfo.Key,
            Modifiers = keyInfo.Modifiers != 0 ? keyInfo.Modifiers : null
        };

        ProcessInternal(actionKey);
    }

    private void ProcessInternal(ConsoleKeyModifierPair input, bool suppressInvokeHooks = false, bool redraw = true) {
        // Always update last input
        lastInput = input;

        // If no action is triggered by this input, don't update last output
        if (!keyActions.TryGetValue(input, out LabeledAction action)) {
            return;
        }

        // Invoke before hook unless suppressed
        if (!suppressInvokeHooks) {
            UpdateGuiBeforeActionInvoke(input);
        }

        // Invoke action and store response
        lastOutput = action.Action.Invoke();

        // Invoke after hook unless suppressed
        if (!suppressInvokeHooks) {
            UpdateGuiAfterActionInvoke(input, lastOutput);
        }

        if (redraw) {
            DrawMenu(lastInput, lastOutput);
        }
    }
}
