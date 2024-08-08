using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace KeyBlazor;

public class KeyBlazorService : IAsyncDisposable
{
    private readonly ILogger<KeyBlazorService>? _logger;
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? _jsModule;

    private readonly DotNetObjectReference<KeyBlazorService> _jsReference;

    private readonly KeyBlazorOptions _options;
    public List<HotKey> RegisteredHotKeys = new();
    private readonly KeySequence _currentKeySequence = new();

    public event Action<KeyboardEventArgs>? KeyDown;
    public event Action<KeyboardEventArgs>? KeyUp;
    public event Action<KeyboardEventArgs>? KeyHeld;

    public KeyBlazorService(
        IOptions<KeyBlazorOptions> options, IJSRuntime jsRuntime,
        ILogger<KeyBlazorService>? logger = null)
    {
        _logger = logger;
        _options = options.Value;
        _jsRuntime = jsRuntime;
        _jsReference = DotNetObjectReference.Create(this);
        RegisterHotKeys(_options.Shortcuts);
        _ = InitializeJsAsync();
        _logger?.LogInformation("Added keyboard event service");
    }

    private async Task InitializeJsAsync()
    {
        const string path = "./_content/KeyBlazor/js/keyBlazor.js";
        _jsModule = await _jsRuntime
            .InvokeAsync<IJSObjectReference>("import", path);
        await SetKeyHoldIntervalAsync(_options.KeyHoldInterval);
        await AddKeyboardEventListenerAsync();
    }

    private void RegisterHotKeys(List<string?> hotKeyStrings)
    {
        foreach (var hotKeyString in hotKeyStrings)
        {
            try
            {
                var hotKey = new HotKey(hotKeyString);
                RegisteredHotKeys.Add(hotKey);
                _logger?.LogInformation("Added hotkey {HotKeyString}",
                    hotKeyString);
                _ = RegisterHotKeyJsAsync(hotKey);
            }
            catch (ArgumentException ex)
            {
                _logger?.LogError(
                    "Error parsing hotkey '{HotKeyString}': {ExMessage}",
                    hotKeyString, ex.Message);
            }
        }
    }

    private async Task SetKeyHoldIntervalAsync(int keyHoldInterval)
    {
        if (_jsModule == null) return;
        await _jsModule.InvokeVoidAsync("setKeyHoldInterval",
            keyHoldInterval, _jsReference);
        _logger?.LogInformation("Set keyHoldInterval to {KeyHoldInterval}",
            keyHoldInterval);
    }

    private async Task AddKeyboardEventListenerAsync()
    {
        if (_jsModule == null) return;
        await _jsModule.InvokeVoidAsync("addKeyboardEventListener",
            _jsReference);
        _logger?.LogInformation("Added keyboard event listener");
    }

    private async Task RegisterHotKeyJsAsync(HotKey hotKey)
    {
        if (_jsModule == null) return;
        await _jsModule.InvokeVoidAsync("registerHotKey",
            JsonSerializer.Serialize(hotKey));
        _logger?.LogInformation(
            "Registered hotkey {HotKey}", hotKey);
    }

    [JSInvokable("InvokeKeyDownEvent")]
    public void InvokeKeyDownEvent(KeyboardEventArgs evt)
    {
        HandleKeyDown(evt);
    }

    [JSInvokable("InvokeKeyHeldEvent")]
    public void InvokeKeyHeldEvent(KeyboardEventArgs evt)
    {
        HandleKeyHeld(evt);
    }

    [JSInvokable("InvokeKeyReleasedEvent")]
    public void InvokeKeyReleasedEvent(KeyboardEventArgs evt)
    {
        HandleKeyReleased(evt);
    }

    private void HandleKeyDown(KeyboardEventArgs evt)
    {
        _logger?.LogDebug("Key down: {Key}", evt.Key);
        KeyDown?.Invoke(evt);

        _currentKeySequence.Add(evt.Key);

        foreach (var hotKey in RegisteredHotKeys.Where(
                     hotKey => _currentKeySequence.IsMatching(hotKey)))
        {
            _logger?.LogInformation("Hotkey activated: {Join}",
                string.Join("+", hotKey.Keys));
            // Handle the hotkey activation here
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
        KeyUp?.Invoke(evt);
        _currentKeySequence.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        if (_jsModule != null)
        {
            await _jsModule.DisposeAsync();
        }

        _jsReference?.Dispose();
    }
}