let html5QrCode;
let dotnetHelper;

window.qrScanner = {
    start: async function (elementId, dotnetRef) {
        try {
            if (typeof Html5Qrcode === "undefined") {
                throw "Html5Qrcode is undefined. Script not loaded.";
            }

            const element = document.getElementById(elementId);
            if (!element) {
                throw "QR reader element not found in DOM.";
            }

            const cameras = await Html5Qrcode.getCameras();
            if (!cameras || cameras.length === 0) {
                throw "No cameras found.";
            }

            window._html5QrCode = new Html5Qrcode(elementId);

            await window._html5QrCode.start(
                cameras[0].id,
                { fps: 10, qrbox: 250 },
                (decodedText) => {
                    dotnetRef.invokeMethodAsync("OnQrScanned", decodedText);
                }
            );
        }
        catch (err) {
            console.error("QR SCANNER ERROR:", err);
            throw err; // <-- THIS is what Blazor sees
        }
    },

    stop: async function () {
        if (window._html5QrCode) {
            await window._html5QrCode.stop();
            window._html5QrCode.clear();
            window._html5QrCode = null;
        }
    }
};
