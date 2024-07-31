namespace KeyBlazor;

public class KeyboardShortcut
{
    public string[]? Keys { get; private init; }

    public static KeyboardShortcut Parse(string? shortcut)
    {
        if (string.IsNullOrWhiteSpace(shortcut))
            throw new ArgumentException("Shortcut cannot be null or empty",
                nameof(shortcut));

        var parts = shortcut.Split('+')
            .Select(k => KeyMap.Normalize(k.Trim()))
            .ToArray();
        return new KeyboardShortcut { Keys = parts };
    }
}