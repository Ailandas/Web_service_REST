using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Lifetime;
using System.Web;
using Newtonsoft.Json;
using RestSharp.Serialization.Json;
using Microsoft.Ajax.Utilities;
using System.Threading.Tasks;

namespace WebEndProject.ExternalAPI
{
    public class _150000quotes
    {
        public string jsonAsString="Empty quote";
        public Models.Quote quote= new Models.Quote();//quote deserialized objektas

        public _150000quotes(string keyword)
        {
            keyword = char.ToUpper(keyword[0]) + keyword.Substring(1);//pakeicia pirma raide i didziaja (quotesAPI reikia paduodi su pirma didziaja)

            var client = new RestClient($"https://150000-quotes.p.rapidapi.com/keyword/{keyword}");//nurodomas URI
            var request = new RestRequest(Method.GET);//nurodomas metodas
            request.AddHeader("x-rapidapi-host", "150000-quotes.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "d54c1da138msh4358497f877f566p11c3f3jsn33823fb42aea");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            IRestResponse response = client.Execute(request);

            jsonAsString = response.Content;
            if (jsonAsString == "No Quote Found Under Your Query. Please try again.")//jei nerastas quote pagal keyword
                jsonAsString = new _150000quotes("Life").jsonAsString;//pakeicia tuscia quote i quote su keyword "Life"
            quote = JsonConvert.DeserializeObject<Models.Quote>(jsonAsString);
        }
    }
}