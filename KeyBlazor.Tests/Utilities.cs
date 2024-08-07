using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Components.Web;

namespace KeyBlazor.Tests;

public class Utilities
{
    public static KeyboardEventArgs KeyBoardEventArgs(string key, bool alt,
        string code, bool ctrl, float location, bool meta, bool repeat,
        bool shift, string type)
    {
        return new KeyboardEventArgs()
        {
            Key = key,
            AltKey = alt,
            Code = "",
            CtrlKey = ctrl,
            Location = 0,
            MetaKey = false,
            Repeat = false,
            ShiftKey = shift,
            Type = null!
        };
    }
}