namespace KeyBlazor;

public class KeyMap
{
    public static string Normalize(string key)
    {
        // Convert to lowercase and map to the standardized form
        return NormalizationMap.GetValueOrDefault(key, key);
    }

    public static readonly Dictionary<string, string> NormalizationMap = new()
    {
        // Modifier Keys
        { "ControlLeft", "Ctrl" }, { "ControlRight", "Ctrl" },
        { "ShiftLeft", "Shift" }, { "ShiftRight", "Shift" },
        { "AltLeft", "Alt" }, { "AltRight", "Alt" },
        { "MetaLeft", "Meta" }, { "MetaRight", "Meta" },

        // Alphabet Keys
        { "KeyA", "A" }, { "KeyB", "B" }, { "KeyC", "C" }, { "KeyD", "D" },
        { "KeyE", "E" }, { "KeyF", "F" }, { "KeyG", "G" }, { "KeyH", "H" },
        { "KeyI", "I" }, { "KeyJ", "J" }, { "KeyK", "K" }, { "KeyL", "L" },
        { "KeyM", "M" }, { "KeyN", "N" }, { "KeyO", "O" }, { "KeyP", "P" },
        { "KeyQ", "Q" }, { "KeyR", "R" }, { "KeyS", "S" }, { "KeyT", "T" },
        { "KeyU", "U" }, { "KeyV", "V" }, { "KeyW", "W" }, { "KeyX", "X" },
        { "KeyY", "Y" }, { "KeyZ", "Z" },

        // Number Keys
        { "Digit0", "0" }, { "Digit1", "1" }, { "Digit2", "2" },
        { "Digit3", "3" }, { "Digit4", "4" }, { "Digit5", "5" },
        { "Digit6", "6" }, { "Digit7", "7" }, { "Digit8", "8" },
        { "Digit9", "9" },

        // Function Keys
        { "F1", "F1" }, { "F2", "F2" }, { "F3", "F3" }, { "F4", "F4" },
        { "F5", "F5" }, { "F6", "F6" }, { "F7", "F7" }, { "F8", "F8" },
        { "F9", "F9" }, { "F10", "F10" }, { "F11", "F11" }, { "F12", "F12" },

        // Arrow Keys
        { "ArrowUp", "Up" }, { "ArrowDown", "Down" },
        { "ArrowLeft", "Left" }, { "ArrowRight", "Right" },

        // Navigation Keys
        { "Home", "Home" }, { "End", "End" },
        { "PageUp", "PageUp" }, { "PageDown", "PageDown" },

        // Editing Keys
        { "Backspace", "Backspace" }, { "Delete", "Delete" },
        { "Insert", "Insert" }, { "Enter", "Enter" },
        { "Escape", "Escape" }, { "Space", "Space" },
        { "Tab", "Tab" },

        // Numpad Keys
        { "Numpad0", "Num0" }, { "Numpad1", "Num1" }, { "Numpad2", "Num2" },
        { "Numpad3", "Num3" }, { "Numpad4", "Num4" }, { "Numpad5", "Num5" },
        { "Numpad6", "Num6" }, { "Numpad7", "Num7" }, { "Numpad8", "Num8" },
        { "Numpad9", "Num9" }, { "NumpadAdd", "NumAdd" },
        { "NumpadSubtract", "NumSubtract" },
        { "NumpadMultiply", "NumMultiply" },
        { "NumpadDivide", "NumDivide" }, { "NumpadDecimal", "NumDecimal" },
        { "NumpadEnter", "NumEnter" },

        // Symbols and Other Keys
        { "Semicolon", ";" }, { "Equal", "=" }, { "Comma", "," },
        { "Minus", "-" }, { "Period", "." }, { "Slash", "/" },
        { "Backquote", "`" }, { "BracketLeft", "[" }, { "BracketRight", "]" },
        { "Backslash", "\\" }, { "Quote", "'" },

        // Additional Keys
        { "CapsLock", "CapsLock" }, { "NumLock", "NumLock" },
        { "ScrollLock", "ScrollLock" }, { "Pause", "Pause" },
        { "PrintScreen", "PrintScreen" }, { "ContextMenu", "ContextMenu" },
        { "BrowserBack", "BrowserBack" }, { "BrowserForward", "BrowserForward" }
    };
}