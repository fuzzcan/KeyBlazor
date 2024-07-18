using Microsoft.AspNetCore.Components.Web;

namespace KeyBlazor;
public class CustomKeyboardEventArgs
{
    public string Key { get; set; }
    public string Code { get; set; }
    public bool AltKey { get; set; }
    public bool CtrlKey { get; set; }
    public bool ShiftKey { get; set; }
    public bool MetaKey { get; set; }

    public static implicit operator KeyboardEventArgs(
        CustomKeyboardEventArgs customArgs)
    {
        return new KeyboardEventArgs
        {
            Key = customArgs.Key,
            Code = customArgs.Code,
            AltKey = customArgs.AltKey,
            CtrlKey = customArgs.CtrlKey,
            ShiftKey = customArgs.ShiftKey,
            MetaKey = customArgs.MetaKey
        };
    }
}