using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyBlazor
{
    public class Service : IAsyncDisposable
    {
        private static Service? _instance;
        private readonly ILogger<Service>? _logger;
        private readonly IJSRuntime _jsRuntime;
        private IJSObjectReference? _jsModule;

        private readonly DotNetObjectReference<Service>
            _jsReference;

        private readonly Options _options;
        public  List<Shortcut> RegisteredShortcuts = new();
        private readonly KeySequence _currentKeySequence = new();

        public event Action<KeyboardEventArgs>? KeyDown;
        public event Action<KeyboardEventArgs>? KeyUp;
        public event Action<KeyboardEventArgs>? KeyHeld;

        public Service(
            IOptions<Options> options, IJSRuntime jsRuntime,
            ILogger<Service>? logger = null)
        {
            _logger = logger;
            _options = options.Value;
            _jsRuntime = jsRuntime;
            _jsReference = DotNetObjectReference.Create(this);
            RegisterShortcuts(_options.Shortcuts);
            _ = InitializeJsAsync();
            _instance = this;
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

        private void RegisterShortcuts(List<string?> shortcutStrings)
        {
            foreach (var shortcutString in shortcutStrings)
            {
                try
                {
                    var shortcut = new Shortcut(shortcutString);
                    RegisteredShortcuts.Add(shortcut);
                    _logger?.LogInformation("Added shortcut {ShortcutString}",
                        shortcutString);
                }
                catch (ArgumentException ex)
                {
                    _logger?.LogError(
                        "Error parsing shortcut '{ShortcutString}': {ExMessage}",
                        shortcutString, ex.Message);
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

            _currentKeySequence.Add(evt.Key);

            foreach (var shortcut in RegisteredShortcuts.Where(shortcut =>
                         _currentKeySequence.IsMatching(shortcut)))
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
}