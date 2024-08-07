namespace KeyBlazor;

public class Shortcut
{
    public string[]? Keys { get; private init; }

    public Shortcut(string? shortcut)
    {
        const string message = "Shortcut cannot be null or empty";
        if (string.IsNullOrWhiteSpace(shortcut))
            throw new ArgumentException(message, nameof(shortcut));

        Keys = shortcut.Split('+')
            .Select(k => KeyMap.Normalize(k.Trim()))
            .ToArray();
    }
}