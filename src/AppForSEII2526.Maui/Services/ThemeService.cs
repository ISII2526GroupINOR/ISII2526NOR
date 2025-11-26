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
    private readonly IJSRuntime _js;

    public event Action? OnChange;

    public string CurrentTheme { get; private set; } = "system";
    public string CurrentAccent { get; private set; } = "default";

    // TEMPORAL: forzar tema estático durante pruebas
    private const bool ForceStaticTheme = false;
    private const string StaticTheme = "dark"; // cambiar a "light", "system", etc.

    public ThemeService(IJSRuntime js)
    {
        _js = js;
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

            await ApplyThemeAsync(CurrentTheme);
            await ApplyAccentAsync(settings.AccentPrimary, settings.AccentSecondary);
        }
        else
        {
            // Default: system theme
            await ApplyThemeAsync("system");
        }
    }

    public async Task ApplyThemeAsync(string theme)
    {
        CurrentTheme = theme;

        try
        {
            await _js.InvokeVoidAsync("themeInterop.setTheme", theme);
        }
        catch (InvalidOperationException ex)
        {
            // JS runtime no disponible en este momento; registrar para diagnóstico.
            Debug.WriteLine($"ThemeService.ApplyThemeAsync: interop no listo: {ex}");
            return;
        }

        await SaveSettingsAsync(new ThemeSettings
        {
            Theme = theme,
            Accent = CurrentAccent,
            AccentPrimary = null,
            AccentSecondary = null
        });

        OnChange?.Invoke();
    }

    public async Task ApplyAccentAsync(string primary, string secondary)
    {
        CurrentAccent = primary;

        try
        {
            await _js.InvokeVoidAsync("themeInterop.setAccent", primary, secondary);
        }
        catch (InvalidOperationException ex)
        {
            Debug.WriteLine($"ThemeService.ApplyAccentAsync: interop no listo: {ex}");
            return;
        }

        await SaveSettingsAsync(new ThemeSettings
        {
            Theme = CurrentTheme,
            Accent = primary,
            AccentPrimary = primary,
            AccentSecondary = secondary
        });

        OnChange?.Invoke();
    }

    // -------------------------------------------------------------
    // Persistence
    // -------------------------------------------------------------

    private async Task SaveSettingsAsync(ThemeSettings settings)
    {
        var json = JsonSerializer.Serialize(settings);
        try
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "theme-settings", json);
        }
        catch (InvalidOperationException ex)
        {
            Debug.WriteLine($"ThemeService.SaveSettingsAsync: no disponible: {ex}");
            // No disponible; ignorar o almacenar en otra parte si es necesario.
        }
    }

    private async Task<ThemeSettings?> LoadSettingsAsync()
    {
        try
        {
            var json = await _js.InvokeAsync<string>("localStorage.getItem", "theme-settings");
            if (string.IsNullOrWhiteSpace(json)) return null;

            try
            {
                return JsonSerializer.Deserialize<ThemeSettings>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ThemeService.LoadSettingsAsync: fallo al deserializar: {ex}");
                return null;
            }
        }
        catch (InvalidOperationException ex)
        {
            Debug.WriteLine($"ThemeService.LoadSettingsAsync: interop no listo: {ex}");
            // JS runtime no listo; devolver null para usar valores por defecto.
            return null;
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

