let keyHoldInterval = 100; // Default interval time in milliseconds
let heldKeys = {};

function createKeyboardEventObject(event) {
    return {
        key: event.key,
        code: event.code,
        altKey: event.altKey,
        ctrlKey: event.ctrlKey,
        shiftKey: event.shiftKey,
        metaKey: event.metaKey
    };
}

export function setKeyHoldInterval(interval) {
    keyHoldInterval = interval;
}

export function addKeyboardEventListener() {
    document.addEventListener("keydown", (event) => {
        if (!heldKeys[event.code]) {
            const keyboardEventObject = createKeyboardEventObject(event);
            DotNet.invokeMethodAsync('KeyBlazor', 'InvokeKeyDownEvent', keyboardEventObject);

            heldKeys[event.code] = setInterval(() => {
                DotNet.invokeMethodAsync('KeyBlazor', 'InvokeKeyHeldEvent', keyboardEventObject);
            }, keyHoldInterval);
        }
    });

    document.addEventListener("keyup", (event) => {
        const keyboardEventObject = createKeyboardEventObject(event);
        DotNet.invokeMethodAsync('KeyBlazor', 'InvokeKeyReleasedEvent', keyboardEventObject);
        if (heldKeys[event.code]) {
            clearInterval(heldKeys[event.code]);
            delete heldKeys[event.code];
        }
    });
}
