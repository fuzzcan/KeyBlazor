namespace KeyBlazor;

public class Shortcut
{
    public string[]? Keys { get; private init; }

    public Shortcut(string? shortcut)
    {
        if (string.IsNullOrWhiteSpace(shortcut))
            throw new ArgumentException("Shortcut cannot be null or empty",
                nameof(shortcut));

        Keys = shortcut.Split('+')
            .Select(k => KeyMap.Normalize(k.Trim()))
            .ToArray();
    }
}