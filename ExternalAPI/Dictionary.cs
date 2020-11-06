using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEndProject.ExternalAPI
{
    public class Dictionary
    {


        public JObject o;
        public string moreWords = "Empty word";
        public Models.Dictionary dictionary = new Models.Dictionary();//dictionary deserialized objektas

        public Dictionary(string keyword)
        {
            var client = new RestClient($"https://wordsapiv1.p.rapidapi.com/words/{keyword}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "wordsapiv1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "d54c1da138msh4358497f877f566p11c3f3jsn33823fb42aea");
            IRestResponse response = client.Execute(request);
            moreWords = response.Content;
            o = JObject.Parse(moreWords);
            dictionary = JsonConvert.DeserializeObject<Models.Dictionary>(moreWords);
        }
    }
}