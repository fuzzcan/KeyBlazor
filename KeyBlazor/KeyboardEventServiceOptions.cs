namespace KeyBlazor;

public class KeyboardEventServiceOptions
{
    public int KeyHoldInterval { get; set; } = 100;
    public List<string?> Shortcuts { get; set; } = [];
}