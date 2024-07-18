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

let heldKeys = {};

export function addKeyboardEventListener() {
    console.log("Added keyboard event listener");

    document.addEventListener("keydown", function (event) {
        if (!heldKeys[event.code]) {
            const keyboardEventObject = createKeyboardEventObject(event);
            DotNet.invokeMethodAsync('KeyBlazor', 'InvokeKeyDownEvent', keyboardEventObject);

            heldKeys[event.code] = setInterval(() => {
                DotNet.invokeMethodAsync('KeyBlazor', 'InvokeKeyHeldEvent', keyboardEventObject);
            }, 100); // Adjust the interval as needed
        }
    });

    document.addEventListener("keyup", function (event) {
        const keyboardEventObject = createKeyboardEventObject(event);
        DotNet.invokeMethodAsync('KeyBlazor', 'InvokeKeyReleasedEvent', keyboardEventObject);

        if (heldKeys[event.code]) {
            clearInterval(heldKeys[event.code]);
            delete heldKeys[event.code];
        }
    });
}
