using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using Microsoft.AspNetCore.Components;

namespace AppForSEII2526.Maui.Services
{
    public class LanguageServices
    {

        public string CurrentLanguage { get; private set; } = "es";

        public event Action? OnLanguageChanged;

        public void SetLanguage(string code)
        {
            var culture = new CultureInfo(code);

            CurrentLanguage = code;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            OnLanguageChanged?.Invoke();
        }

    }
}
