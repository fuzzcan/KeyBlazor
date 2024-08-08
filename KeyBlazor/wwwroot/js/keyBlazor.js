let keyHoldInterval = 100; // Default interval time in milliseconds
let heldKeys = {};
let hotKeys = [];

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

export function registerHotKey() {
    console.log("OOOOOOOOOOOOOO");
    hotKeys.push(JSON.parse(hotKey));
}

export function addKeyboardEventListener(dotNetHelper) {
    document.addEventListener("keydown", (event) => {
        const keyboardEventObject = createKeyboardEventObject(event);

        for (const hotKey of hotKeys) {
            const match = hotKey.Keys.every((key, index) => {
                return key === keyboardEventObject[key];
            });

            if (match) {
                if (hotKey.PreventDefaultBehaviour) {
                    event.preventDefault();
                }
                dotNetHelper.invokeMethodAsync('InvokeKeyDownEvent', keyboardEventObject);
                heldKeys[event.code] = setInterval(() => {
                    dotNetHelper.invokeMethodAsync('InvokeKeyHeldEvent', keyboardEventObject);
                }, keyHoldInterval);
                return;
            }
        }

        if (!heldKeys[event.code]) {
            dotNetHelper.invokeMethodAsync('InvokeKeyDownEvent', keyboardEventObject);
            heldKeys[event.code] = setInterval(() => {
                dotNetHelper.invokeMethodAsync('InvokeKeyHeldEvent', keyboardEventObject);
            }, keyHoldInterval);
        }
    });

    document.addEventListener("keyup", (event) => {
        const keyboardEventObject = createKeyboardEventObject(event);
        dotNetHelper.invokeMethodAsync('InvokeKeyReleasedEvent', keyboardEventObject);
        if (heldKeys[event.code]) {
            clearInterval(heldKeys[event.code]);
            delete heldKeys[event.code];
        }
    });
}
