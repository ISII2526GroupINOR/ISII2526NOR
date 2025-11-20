using AppForSEII2526.Maui.Services;
using Microsoft.Extensions.Logging;

namespace AppForSEII2526.Maui {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();

            builder.ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Robot-Italic.ttf", "Roboto Italic");
                    fonts.AddFont("Roboto-Light.ttf", "Roboto Light");
                    fonts.AddFont("Roboto-Thin.ttf", "Roboto Thin");
                    fonts.AddFont("RobotoCondensed-VariableFont_wght.ttf", "Roboto Condensed");
                    fonts.AddFont("Comfortaa-VariableFont_wght.ttf", "Comfortaa");
                }
            );
#endif
            builder.Services.AddBlazorBootstrap();
            builder.Services.AddScoped<LanguageServices>();
            builder.Services.AddScoped<FontServices>();
            builder.Services.AddSingleton<FontServices>();
            return builder.Build();
        }
    }
}
