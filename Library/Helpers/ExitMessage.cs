namespace Pokepanion.Library.Helpers;

public static class ExitMessage {
    public static string Build(string issue, params string[] helpOptions) {
        const string helpOptionPrefix = "\n  - ";
        return $"{issue}\n\nHelp:{helpOptionPrefix}{string.Join(helpOptionPrefix, helpOptions)}";
    }
}
