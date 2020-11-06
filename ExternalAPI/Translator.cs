using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEndProject.ExternalAPI
{
    public class Translator
    {
        public string translatedText;

        public Translator(string text)
        {
            string readyString = text;
            readyString.Replace(' '.ToString(), "%20");
            readyString.Replace(','.ToString(), "%252C%20");
            readyString.Replace('?'.ToString(), "%253F%20");
            readyString.Replace('.'.ToString(), "%20.");
            var client = new RestClient($"https://microsoft-azure-translation-v1.p.rapidapi.com/translate?from=en&to=lt&text={readyString}");// What%20a%20lovely%20day%20today%252C%20isnt%20it");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "microsoft-azure-translation-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "d54c1da138msh4358497f877f566p11c3f3jsn33823fb42aea");
            request.AddHeader("accept", "application/json");
            IRestResponse response = client.Execute(request);
            translatedText = response.Content;
            string[] splited =translatedText.Split('>');
            string[] splited1 = splited[1].Split('<');
            translatedText = splited1[0];
        }
    }
}