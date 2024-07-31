using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyBlazor;

public class KeyboardEventService : IAsyncDisposable
{
    private static KeyboardEventService? _instance;
    private readonly ILogger<KeyboardEventService>? _logger;
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? _module;
    private readonly DotNetObjectReference<KeyboardEventService> _jsReference;

    private readonly KeyboardEventServiceOptions _options;
    private readonly List<KeyboardShortcut> _shortcuts = [];
    private readonly List<string> _currentSequence = [];

    public event Action<KeyboardEventArgs>? KeyDown;
    public event Action<KeyboardEventArgs>? KeyUp;
    public event Action<KeyboardEventArgs>? KeyHeld;

    public KeyboardEventService(
        IOptions<KeyboardEventServiceOptions> options, IJSRuntime jsRuntime,
        ILogger<KeyboardEventService>? logger = null)
    {
        _logger = logger;
        _options = options.Value;
        _jsRuntime = jsRuntime;
        _logger?.LogInformation("Added keyboard event service");

        _jsReference = DotNetObjectReference.Create(this);

        RegisterShortcuts(_options.Shortcuts);
        _ = InitializeJsAsync();

        _instance = this;
    }

    private async Task InitializeJsAsync()
    {
        const string path = "./_content/KeyBlazor/js/keyBlazor.js";
        _module = await _jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", path);
        await SetKeyHoldIntervalAsync(_options.KeyHoldInterval);
        await AddKeyboardEventListenerAsync();
    }

    private void RegisterShortcuts(List<string?> shortcutStrings)
    {
        foreach (var shortcutString in shortcutStrings)
        {
            try
            {
                var shortcut = KeyboardShortcut.Parse(shortcutString);
                _shortcuts.Add(shortcut);
                _logger?.LogInformation(
                    "Added shortcut {ShortcutString}", shortcutString);
            }
            catch (ArgumentException ex)
            {
                const string description = "Error parsing shortcut";
                _logger?.LogError(
                    "{Message} \'{ShortcutString}\': {ExMessage}",
                    description, shortcutString, ex.Message);
            }
        }
    }

    private async Task SetKeyHoldIntervalAsync(int keyHoldInterval)
    {
        if (_module == null) return;
        await _module.InvokeVoidAsync("setKeyHoldInterval",
            keyHoldInterval, _jsReference);
        _logger?.LogInformation(
            "Set keyHoldInterval to {KeyHoldInterval}",
            keyHoldInterval);
    }

    private async Task AddKeyboardEventListenerAsync()
    {
        if (_module == null) return;
        await _module.InvokeVoidAsync("addKeyboardEventListener",
            _jsReference);
        _logger?.LogInformation("Added keyboard event listener");
    }

    [JSInvokable]
    public static void InvokeKeyDownEvent(KeyboardEventArgs evt)
    {
        _instance?.HandleKeyDown(evt);
    }

    [JSInvokable]
    public static void InvokeKeyHeldEvent(KeyboardEventArgs evt)
    {
        _instance?.HandleKeyHeld(evt);
    }

    [JSInvokable]
    public static void InvokeKeyReleasedEvent(KeyboardEventArgs evt)
    {
        _instance?.HandleKeyReleased(evt);
    }

    private void HandleKeyDown(KeyboardEventArgs evt)
    {
        _logger?.LogDebug("Key down: {Key}", evt.Key);
        KeyDown?.Invoke(evt);

        _currentSequence.Add(evt.Key);

        foreach (var shortcut in _shortcuts.Where(IsMatchingShortcut))
        {
            _logger?.LogInformation("Shortcut activated: {Join}",
                string.Join("+", shortcut.Keys));
            // Handle the shortcut activation here
        }
    }

    private void HandleKeyHeld(KeyboardEventArgs evt)
    {
        _logger?.LogInformation("Key held: {Key}", evt.Key);
        KeyHeld?.Invoke(evt);
    }

    private void HandleKeyReleased(KeyboardEventArgs evt)
    {
        _logger?.LogDebug("Key up: {Key}", evt.Key);
        _logger?.LogInformation("YOOHOO");
        KeyUp?.Invoke(evt);
        _currentSequence.Clear();
    }

    private bool IsMatchingShortcut(KeyboardShortcut shortcut)
    {
        if (_currentSequence.Count < shortcut.Keys.Length)
            return false;

        var isMatch = !_currentSequence.Where((t, i) => t != shortcut.Keys[i])
            .Any();
        _logger?.LogDebug(
            "Checking sequence: {CurrentSequence} against shortcut: {ShortcutKeys} -> Match: {IsMatch}",
            string.Join("+", _currentSequence),
            string.Join("+", shortcut.Keys),
            isMatch);

        return isMatch;
    }

    public async ValueTask DisposeAsync()
    {
        if (_module != null)
        {
            await _module.DisposeAsync();
        }

        _jsReference?.Dispose();
    }
}