using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppForSEII2526.Maui.Services
{
    public class AssistantServices
    {   //dest parameter for future implemantation of links to the desired page if required
        public event Action? OnAssistantCalled;
        public string AssistantInit()
        {
            var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            switch (language)
            {
                case "en":
                    return "Hi, are you comfortable with the interface? Maybe some help with navigation?";
                    break;
                case "es":
                    return "¿Hola, necesitas ayuda con la interfaz? ¿O quizá para navegar?";
                    break;
                default:
                    return "Hi, are you comfortable with the interface? Maybe some help with navigation?";
                    break;
            }
        }
        public string AssistantMainResponse(string message, string context, out string? dest, string role)
        {
            var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            dest = null;
            string response;

            switch (language)
            {
                case "en":
                    response = AssistEnglish(message, context, out dest, role);
                    break;
                case "es":
                    response = AssistSpanish(message, context, out dest, role);
                    break;
                default:
                    response = "I can´t understand you bro.";
                    break;
            }
            OnAssistantCalled?.Invoke();
            return response;
        }


        private string AssistEnglish(string input, string context, out string? dest, string role)
        {
            string message = "I don´t know what you want bro.";
            dest = "";
            if (message == null || message == "") return message;
            input = input.ToLower();

            if (Regex.IsMatch(input, @"(language|idiom|entiendo|idioma)"))
            {
                if (context == "/adminconfiguration")
                {
                    message = "You are just there, just click the first box, below your user image.";
                }
                else
                {
                    message = "Don´t understand anything, huh? Just go to configuration, in your profile" +
                        ", there you can change the language.";
                    if(role == "admin") dest = "/adminconfiguration";
                    else if (role == "user") dest = "/userprofile";
                }
            }
            else if (Regex.IsMatch(input, @"(font|letter|letters)"))
            {
                if (context == "/adminconfiguration")
                {
                    message = "You are just there, just click the second box, below your user image.";
                }
                else
                {
                    message = "Want to change those letters, huh? Just go to configuration, in your profile" +
                        ", there you can change the font to a better one.";
                    if (role == "admin") dest = "/adminconfiguration";
                    else if (role == "user") dest = "/userprofile";
                }
            }
            else if (Regex.IsMatch(input, @"(widget|widgets)"))
            {
                if (context == "/adminconfiguration")
                {
                    message = "You are just there, just click the tick boxes below your user image enable and disable them.";
                }
                else
                {
                    message = "Don´t like those widgets, huh? Just go to configuration, in your profile" +
                        ", there you can hide and show them.";
                    if (role == "admin") dest = "/adminconfiguration";
                    else if (role == "user") dest = "/userprofile";
                }
            }
            else if (Regex.IsMatch(input, @"(restock|items|item)"))
            {
                if (context == "/adminhome" && Preferences.Get("wRestock", true))
                {
                    message = "You can press the first widget, the one saying restock. If you want to restock a concrete item," +
                        "you can press it, and it will be automatically selected.";
                }
                else if (role=="admin")
                {
                    message = "You can press the second item from the left of the nav menu below. That will take you there.";
                    if (role == "admin") dest = "/adminhome";
                }
            }

            return message;
        }


        private string AssistSpanish(string input, string context, out string? dest, string role)
        {
            string message = "No se que quieres bro.";
            dest = "";
            if (message == null || message == "") return message;
            input = input.ToLower();

            if (Regex.IsMatch(input, @"(language|idiom|entiendo|idioma)"))
            {
                if (context == "/adminconfiguration")
                {
                    message = "Casi lo tienes, solo pulsa la primera caja de texto, debajo de tu imagen de perfil.";
                }
                else
                {
                    message = "No entiendes nada, eh? Solo ve a configuración, en tu perfil" +
                        ", ahí puedes cambiar la imagen de perfil.";
                    if (role == "admin") dest = "/adminconfiguration";
                    else if (role == "user") dest = "/userprofile";
                }
            }
            else if (Regex.IsMatch(input, @"(fuente|letra|letras)"))
            {
                if (context == "/adminconfiguration")
                {
                    message = "Casi lo tienes, solo pulsa la segunda caja de texto, debajo de tu imagen de perfil.";
                }
                else
                {
                    message = "¿Quieres cambiar esas letras, eh? Solo ve a la configuración, en tu perfil" +
                        ", ahí puedes cambiar el tipo de letra.";
                    if (role == "admin") dest = "/adminconfiguration";
                    else if (role == "user") dest = "/userprofile";
                }
            }
            else if (Regex.IsMatch(input, @"(widget|widgets)"))
            {
                if (context == "/adminconfiguration")
                {
                    message = "Ya estas ahí, solo pulsa las cajas con ticks para deshabilitar y habilitar los widgets.";
                }
                else
                {
                    message = "No te gustan esos widgets? Ve a configuración, en tu perfil" +
                        ", ahí puedes ocultarlos y mostrarlos.";
                    if (role == "admin") dest = "/adminconfiguration";
                    else if (role == "user") dest = "/userprofile";
                }
            }
            else if (Regex.IsMatch(input, @"(restock|stock|item|abastecer|artículo|articulo)"))
            {
                if (context == "/adminhome" && Preferences.Get("wRestock", true))
                {
                    message = "Puedes pulsar el primer widget y te llevará a reabastecer" +
                        ". También puedes pulsar el artículo que quieres reabastecer y te lo seleccionará automáticamente.";
                }
                else if (role == "admin")
                {
                    message = "Pulsa el segundo icono del menú inferior. Te llevará a la ventana de reabastecimiento ahí.";
                    if (role == "admin") dest = "/adminhome";
                }
            }

            return message;
        }

    }
}
