using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Diagnostics;

public class ThemeService
{

    public event Action? OnChange;

    public string CurrentTheme { get; private set; } = "system";
    public string CurrentAccent { get; private set; } = "default";

    // TEMPORAL: forzar tema estático durante pruebas
    private const bool ForceStaticTheme = false;
    private const string StaticTheme = "dark"; // cambiar a "light", "system", etc.

    public ThemeService()
    {
    }

    // -------------------------------------------------------------
    // Public API
    // -------------------------------------------------------------

    public async Task InitializeAsync()
    {
        // Try to load persisted settings
        var settings = await LoadSettingsAsync();
        if (settings != null)
        {
            CurrentTheme = settings.Theme;
            CurrentAccent = settings.Accent;
        }
        else
        {
            // Default: system theme
            CurrentTheme = "system";
            CurrentAccent = "default";
        }
        // Apply the theme and accent
        OnChange?.Invoke();
        await Task.CompletedTask;
    }

    public Task ApplyThemeAsync(string theme)
    {
        CurrentTheme = theme;

        try
        {
            SaveSettingsAsync(new ThemeSettings
            {
                Theme = theme,
                Accent = CurrentAccent,
                AccentPrimary = null,
                AccentSecondary = null
            }).Wait();
        }
        catch (Exception ex)
        {
            // JS runtime no disponible en este momento; registrar para diagnóstico.
            Debug.WriteLine($"ThemeService.ApplyThemeAsync: fallo al guardar: {ex}");
        }


        OnChange?.Invoke();
        return Task.CompletedTask;
    }

    public Task ApplyAccentAsync(string primary, string secondary)
    {
        CurrentAccent = primary;

        try
        {
            SaveSettingsAsync(new ThemeSettings
            {
                Theme = CurrentTheme,
                Accent = primary,
                AccentPrimary = primary,
                AccentSecondary = secondary
            }).Wait();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ThemeService.ApplyAccentAsync: fallo al guardar: {ex}");
        }

        OnChange?.Invoke();
        return Task.CompletedTask;
    }

    // -------------------------------------------------------------
    // Persistence
    // -------------------------------------------------------------

    private Task SaveSettingsAsync(ThemeSettings settings)
    {
        var json = JsonSerializer.Serialize(settings);
        Preferences.Set("theme-settings", json);
        return Task.CompletedTask;
    }

    private Task<ThemeSettings?> LoadSettingsAsync()
    {
        try
        {
            var json = Preferences.Get("theme-settings", (string?)null);
            if (string.IsNullOrWhiteSpace(json)) return Task.FromResult<ThemeSettings?>(null);

            try
            {
                var s = JsonSerializer.Deserialize<ThemeSettings>(json);
                return Task.FromResult<ThemeSettings?>(s);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ThemeService.LoadSettingsAsync: fallo al deserializar: {ex}");
                return Task.FromResult<ThemeSettings?>(null);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ThemeService.LoadSettingsAsync: fallo al leer Preferences: {ex}");
            return Task.FromResult<ThemeSettings?>(null);
        }
    }
}

public class ThemeSettings
{
    public string Theme { get; set; } = "system";
    public string Accent { get; set; } = "default";
    public string? AccentPrimary { get; set; }
    public string? AccentSecondary { get; set; }
}

