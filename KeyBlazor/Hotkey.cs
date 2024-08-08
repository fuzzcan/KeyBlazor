namespace KeyBlazor;

public class Hotkey
{
    public string[]? Keys { get; private init; }
    public bool PreventDefaultBehaviour { get; set; }

    public Hotkey(string? shortcut, bool preventDefaultBehaviour = false)
    {
        const string message = "Shortcut cannot be null or empty";
        if (string.IsNullOrWhiteSpace(shortcut))
            throw new ArgumentException(message, nameof(shortcut));

        Keys = shortcut.Split('+')
            .Select(k => KeyMap.Normalize(k.Trim()))
            .ToArray();

        PreventDefaultBehaviour = preventDefaultBehaviour;
    }
}