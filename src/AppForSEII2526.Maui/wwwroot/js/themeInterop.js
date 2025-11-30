window.themeInterop = {

    setTheme: function (themeName) {
        // Applies: data-theme="light", "dark", "system", ...
        document.body.setAttribute("data-theme", themeName);
    },

    setAccent: function (accentName) {
        document.body.setAttribute("data-accent", accentName);
    }
};