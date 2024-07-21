namespace KeyBlazor;

public class KeyboardShortcut
{
    public string[] Keys { get; set; }

    public static KeyboardShortcut Parse(string? shortcut)
    {
        var parts = shortcut.Split('+');
        return new KeyboardShortcut
        {
            Keys = parts
        };
    }
}