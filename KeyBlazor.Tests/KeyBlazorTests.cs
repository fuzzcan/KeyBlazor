using System.Collections.Generic;
using Xunit;
using Bunit;
using Moq;
using Microsoft.JSInterop;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using KeyBlazor.Tests;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace KeyBlazorTests
{
    public class KeyBlazorTests : TestContext
    {
        public KeyBlazorTests()
        {
            Mock<IJSRuntime> jsRuntimeMock = new();
            Mock<IOptions<KeyBlazor.Options>> optionsMock = new();
            Mock<ILogger<KeyBlazor.KeyBlazor>> loggerMock = new();
            var options = new KeyBlazor.Options
            {
                KeyHoldInterval = 100,
                Shortcuts = ["Ctrl+Shift+A"]
            };

            optionsMock.Setup(o => o.Value)
                .Returns(options);

            Services.AddSingleton(jsRuntimeMock.Object);
            Services.AddSingleton(optionsMock.Object);
            Services.AddSingleton(loggerMock.Object);
            Services.AddSingleton<KeyBlazor.KeyBlazor>();
        }

        [Fact]
        public void KeyBlazorComponent_ShouldRender()
        {
            // Arrange
            var cut = RenderComponent<KeyBlazorComponent>();

            // Assert
            Assert.NotNull(cut);
        }

        [Fact]
        public void HandleKeyDown_ShouldTriggerKeyDownEvent()
        {
            // Arrange
            var service = Services.GetRequiredService<KeyBlazor.KeyBlazor>();
            var eventTriggered = false;
            service.KeyDown += _ => eventTriggered = true;
            var cut = RenderComponent<KeyBlazorComponent>();
            var div = cut.Find("div");

            // Act
            div.KeyDown(new KeyboardEventArgs
            {
                Key = "A", Code = "KeyA", CtrlKey = true, ShiftKey = true
            });

            // Assert
            Assert.True(eventTriggered);
        }

        [Fact]
        public void HandleKeyHeld_ShouldTriggerKeyHeldEvent()
        {
            // Arrange
            var service = Services.GetRequiredService<KeyBlazor.KeyBlazor>();
            var eventTriggered = false;
            service.KeyHeld += _ => eventTriggered = true;
            var cut = RenderComponent<KeyBlazorComponent>();
            var div = cut.Find("div");

            // Act
            div.KeyDown(new KeyboardEventArgs
            {
                Key = "A", Code = "KeyA", CtrlKey = true, ShiftKey = true
            });

            // Simulate key held event (normally you would have to wait or use a timer)
            KeyBlazor.KeyBlazor.InvokeKeyHeldEvent(
                new KeyboardEventArgs
                {
                    Key = "A", Code = "KeyA", CtrlKey = true, ShiftKey = true
                });

            // Assert
            Assert.True(eventTriggered);
        }

        [Fact]
        public void HandleKeyReleased_ShouldTriggerKeyUpEvent()
        {
            // Arrange
            var service = Services.GetRequiredService<KeyBlazor.KeyBlazor>();
            var eventTriggered = false;
            service.KeyUp += _ => eventTriggered = true;
            var cut = RenderComponent<KeyBlazorComponent>();
            var div = cut.Find("div");

            // Act
            div.KeyUp(
                new KeyboardEventArgs
                {
                    Key = "A", Code = "KeyA", CtrlKey = true, ShiftKey = true
                });

            // Assert
            Assert.True(eventTriggered);
        }
    }
}