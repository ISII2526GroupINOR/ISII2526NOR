window.themeInterop = {

    setTheme: function (themeName) {
        // Applies: data-theme="light", "dark", "system", ...
        document.documentElement.setAttribute("data-theme", themeName);
    },

    setAccent: function (accentName) {

        document.documentElement.setAttribute("data-accent", accentName);
    }
};