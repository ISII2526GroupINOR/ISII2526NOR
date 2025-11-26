window.themeInterop = {

    setTheme: function (themeName) {
        // Applies: data-theme="light", "dark", "system", ...
        document.body.setAttribute("data-theme", themeName);
    },

    setAccent: function (primary, secondary) {
        document.documentElement.style.setProperty("--accent-primary", primary);
        document.documentElement.style.setProperty("--accent-primary-2", secondary);
    }
};