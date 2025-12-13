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
            input = input.ToLower();
            string message = "I don´t know what you want bro.";
            dest = null;
            // Non contextual messages
            if (Regex.IsMatch(input, @"(language|idiom)"))
            {
                if (context == "/adminconfiguration")
                {
                    message = "You are just there, just click the first box, below your user image.";
                }
                else
                {
                    message = "Don´t understand anything, huh? Just go to configuration, in your profile" +
                        ", there you can change the language.";
                    if(role == "admin") dest = "/adminprofile";
                    if (role == "user") dest = "/userprofile";
                }
            }
            return message;
        }


        private string AssistSpanish(string input, string context, out string? dest, string role)
        {
            string message = "No se que quieres bro.";
            dest = null;

            return message;
        }

    }
}
