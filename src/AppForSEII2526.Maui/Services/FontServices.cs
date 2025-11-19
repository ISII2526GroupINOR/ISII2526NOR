using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.Maui.Services
{
    public class FontServices
    {
        public string CurrentFontClass { get; private set; } = "font-opensans";
        public event Action? OnFontChanged;
        public void SetFont(string fontClass)
        {
            CurrentFontClass = fontClass;
            OnFontChanged.Invoke();
        }
    }
}
