using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppForSEII2526.Maui.Services
{
    public class AssistantServices
    {
        public string AssistEnglish(string input, string context)
        {
            string message = "I don´t know what you want";

            // Non contextual messages
            if (Regex.IsMatch(input, "/language | /idiom"))
            {
                if(Regex.IsMatch(context, "/adminprofile | /userprofile"))
                {
                    message = "Don´t understand anything, huh? Just go to configuration" +
                        ", there you can change the language";   
                }

                if(Regex.IsMatch(context, "/adminconfiguration"))
                {
                    message = "You are just there, just click the first menu below your user image";
                }
            }

            return message;
        }

    }
}
