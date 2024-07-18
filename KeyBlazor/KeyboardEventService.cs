using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace KeyBlazor
{
    public class KeyboardEventService : IAsyncDisposable
    {
        private static ILogger<KeyboardEventService>? _logger;
        private static bool _enableLogging;

        public static event Action<KeyboardEventArgs>? KeyDown;
        public static event Action<KeyboardEventArgs>? KeyUp;
        public static event Action<KeyboardEventArgs>? KeyHeld;

        public KeyboardEventService(ILogger<KeyboardEventService>? logger,
            IOptions<KeyboardEventServiceOptions> options)
        {
            _logger = logger;
            _enableLogging = options.Value.EnableLogging;

            LogMessage("Added keyboard event service");
        }

        private static void LogMessage(string message)
        {
            _logger.LogInformation("LOGGING ENABLED: " + _enableLogging);
            if (_enableLogging)
            {
                _logger?.LogInformation("{Message}", message);
            }
        }

        [JSInvokable]
        public static void InvokeKeyDownEvent(CustomKeyboardEventArgs evt)
        {
            LogMessage("Key down: " + evt.Key);
            KeyDown?.Invoke(evt); // Implicit cast to KeyboardEventArgs
        }

        [JSInvokable]
        public static void InvokeKeyHeldEvent(CustomKeyboardEventArgs evt)
        {
            LogMessage("Key held: " + evt.Key);
            KeyHeld?.Invoke(evt); // Implicit cast to KeyboardEventArgs
        }

        [JSInvokable]
        public static void InvokeKeyReleasedEvent(CustomKeyboardEventArgs evt)
        {
            LogMessage("Key up: " + evt.Key);
            KeyUp?.Invoke(evt); // Implicit cast to KeyboardEventArgs
        }

        public ValueTask DisposeAsync()
        {
            // Dispose resources if necessary
            return ValueTask.CompletedTask;
        }
    }
}