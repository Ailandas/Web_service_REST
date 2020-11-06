using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.UI.WebControls;
using WebEndProject.Models;
using WebGrease.Css.Extensions;

namespace WebEndProject.Controllers
{
    public class QuoteController : ApiController
    {
        

        ////////////////custom //////////////////////////////////
        
        /// <summary>
        /// Metodas, grazinantis zodi ir quote is pasirinktos kategorijos
        /// </summary>
        /// <param name="category">Kategorijos pavadinimas</param>
        /// <returns></returns>
        [Route("api/quotedictionary/{category}")]
        [HttpGet]
        public List<object> Get(string category)
        {
            
             bool existance=Models.SqlLite.CheckIfCategoryEgzists(category);
             if (existance == true)
             {

                List<object> cachingList = new List<object>();
                cachingList = (List<object>)MemoryCacher.GetValue(category);
                if (cachingList != null)
                    return cachingList;//jei yra cachinge, grazina is cache
                else
                {
                    DateTime now = DateTime.Now;
                    int id = Models.TimeDifferences.GetDayOfTimeValue(now);

                    Models.SingleWord singleWord = Models.SqlLite.GetSingleWord(id, category);

                    ExternalAPI.Dictionary fetchWord = new ExternalAPI.Dictionary(singleWord.GetWord());//gauna dictionary pagal keyword

                    ExternalAPI._150000quotes fetchQuote = new ExternalAPI._150000quotes(category);//gauna quote pagal keyword

                    ExternalAPI.Translator fetchTranslation = new ExternalAPI.Translator(fetchQuote.quote.Message);
                    ExternalAPI.Translator fetchTranslation1 = new ExternalAPI.Translator(singleWord.GetWord());

                    List<object> TempCachingList = new List<object>();
                    TempCachingList.Add(fetchQuote.quote.Message);
                    TempCachingList.Add(fetchTranslation.translatedText.ToString());
                    TempCachingList.Add(fetchTranslation1);
                    TempCachingList.Add(fetchWord.o);
                    MemoryCacher.Add(category, TempCachingList, DateTimeOffset.UtcNow.AddMinutes(1));
                    return TempCachingList;
                }
             }
             else
             {
                return null;
             }
        }

        /// <summary>
        /// Metodas, iterpiantis nauja irasa i duomenu baze
        /// </summary>
        /// <param name="obj">Iraso objektas(kategorija,zodis,laikas)</param>
        /// <returns></returns>
        [Route("api/quotedictionary")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] PostObject obj)
        {
            /*    { "kategorija": "TestKategorija",
                    "zodis": "TestZodis",
                    "laikas": "Diena"}
             */
            try
            {
                string uppercaseTime = obj.laikas.ToUpper();
                List<Time> timeOptions = SqlLite.GetTimes();
                foreach (var time in timeOptions)
                {
                    if (time.GetName() == uppercaseTime)
                    {
                        Models.SqlLite.InsertToDatabase(obj.kategorija, obj.zodis, time.GetID());
                        return Ok(200);
                    }
                }
                return NotFound();
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Metodas, grazinantis visas kategorijas
        /// </summary>
        /// <returns></returns>
        [Route("api/quotedictionary/categories")]
        [HttpGet]
        public List<Category> GetCategories()
        {
            string baseUrl = (Url.Request.RequestUri.GetComponents(
                    UriComponents.SchemeAndServer, UriFormat.Unescaped).TrimEnd('/')
                 + System.Web.HttpContext.Current.Request.ApplicationPath).TrimEnd('/');

            List<Category> cachingList = new List<Category>();
            cachingList = (List<Category>)MemoryCacher.GetValue("categories");

            if (cachingList != null)
            {
                return cachingList;//Newtonsoft.Json.JsonConvert.SerializeObject(cachingList).ToString();
            }
            else
            {
                List<Category> categories = Models.SqlLite.GetCategories();
                for (int i=0; i<categories.Count; i++)
                {
                    Link link = new Link();
                    link.href= baseUrl + $"/api/quotedictionary/categories/{categories[i].GetName()}";
                    link.method = "DELETE";
                    link.rel = "delete_self";
                    Link link1 = new Link();
                    link1.href = baseUrl + $"/api/quotedictionary/categories/{categories[i].GetName()}";
                    link1.method = "GET";
                    link1.rel = "get_self";
                    Link link2 = new Link();
                    link2.href = baseUrl + $"/api/quotedictionary/categories/";
                    link2.method = "PUT";
                    link2.rel = "put_self";

                    List<Link> tempLinks = new List<Link>();
                    tempLinks.Add(link);
                    tempLinks.Add(link1);
                    tempLinks.Add(link2);

                    categories[i].SetLinks(tempLinks);
                }
                
                MemoryCacher.Add("categories", categories, DateTimeOffset.UtcNow.AddMinutes(1));

                return categories;//Newtonsoft.Json.JsonConvert.SerializeObject(categories).ToString(); 
             }
            

            

        }

        /// <summary>
        /// Metodas, grazinantis zodzius is kategorijos
        /// </summary>
        /// <param name="category">Kategorijos pavadinimas</param>
        /// <returns></returns>
        [Route("api/quotedictionary/categories/{category}")]
        [HttpGet]
        public IHttpActionResult GetCategoryEntries(string category)
        {
            List<SingleWord> cachingList = new List<SingleWord>();
            cachingList = (List<SingleWord>)MemoryCacher.GetValue(category);
            if (cachingList != null)
            {
                return Ok(cachingList);
            }
            else
            {
                string baseUrl = (Url.Request.RequestUri.GetComponents(
                 UriComponents.SchemeAndServer, UriFormat.Unescaped).TrimEnd('/')
              + System.Web.HttpContext.Current.Request.ApplicationPath).TrimEnd('/');

                List<SingleWord> SingleCategory = Models.SqlLite.GetEntriesByCategory(category);
                if (SingleCategory.Count > 0)
                {
                    for (int i = 0; i < SingleCategory.Count; i++)
                    {
                        //linkas deletinimui
                        Link link = new Link();
                        link.href = "DELETE: " + baseUrl + $"/api/quotedictionary/words/{SingleCategory[i].Word}";
                        link.method = "DELETE";
                        link.rel = "delete_self";
                        //linkas getinimui
                        Link link1 = new Link();
                        link1.href = "GET self : " + baseUrl + $"/api/quotedictionary/words/{SingleCategory[i].Word}";
                        link1.rel = "get_self";
                        link1.method = "GET";
                        Link link2 = new Link();
                        link2.href = baseUrl + $"/api/quotedictionary/words/";
                        link2.method = "PUT";
                        link2.rel = "put_self";

                        SingleCategory[i].links.Add(link);
                        SingleCategory[i].links.Add(link1);
                        SingleCategory[i].links.Add(link2);
                    }
                    MemoryCacher.Add(category, SingleCategory, DateTimeOffset.UtcNow.AddMinutes(1));
                    return Ok(SingleCategory);
                }
                else
                {
                    return NotFound();
                }
               
            }
        }

        /// <summary>
        /// Metodas skirtas atnaujinti kategorijos pavadinima
        /// </summary>
        /// <param name="putObject">Objektas laikantis informacija apie nauja ir sena kategorija</param>
        /// <returns></returns>
        [Route("api/quotedictionary/categories/")]
        [HttpPut]
        public IHttpActionResult UpdateCategoryName([FromBody] PutObject putObject)
        {
            string oldData = putObject.oldData;
            string newData = putObject.newData;
            bool Existance = Models.SqlLite.CheckIfCategoryEgzists(oldData);
            if(Existance==true)
            {
                Models.SqlLite.UpdateCategoryName(newData, oldData);
                return Ok();
            }
            else
            {
                return NotFound();
            }
           
            

        }
        /// <summary>
        /// Metodas, istrinantis kategorija
        /// </summary>
        /// <param name="category">Kategorijos pavadinimas</param>
        /// <returns></returns>
        [Route("api/quotedictionary/categories/{category}")]
        [HttpDelete]
        public IHttpActionResult DeleteCategory(string category) 
        {
            bool deleted = Models.SqlLite.DeleteCategory(category);
            if (deleted == true)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Metodas, istrinantis zodi
        /// </summary>
        /// <param name="word">Zodis</param>
        /// <returns></returns>
        [Route("api/quotedictionary/words/{word}")]
        [HttpDelete]
        public IHttpActionResult DeleteWord(string word)
        {
            bool deleted = Models.SqlLite.DeleteWord(word);
            if (deleted == true)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Metodas skirtas atnaujinti zodi duomenu bazeje
        /// </summary>
        /// <param name="putObject">Objektas laikantis informacija apie sena ir nauja zodi</param>
        /// <returns></returns>
        [Route("api/quotedictionary/words/")]
        [HttpPut]
        public IHttpActionResult UpdateSingleWord([FromBody] PutObject putObject)
        {
            string oldData = putObject.oldData;
            string newData = putObject.newData;
           // Models.SqlLite.UpdateSingleWord(newData, oldData);
            
            bool Existance = Models.SqlLite.CheckIfWordExists(oldData);
            if (Existance == true)
            {
                Models.SqlLite.UpdateSingleWord(newData, oldData);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Metodas, grazinantis zodi
        /// </summary>
        /// <param name="word">Zodis</param>
        /// <returns></returns>
        [Route("api/quotedictionary/words/{word}")]
        [HttpGet]
        public ExternalAPI.Dictionary GetSingleWord(string word)
        {

            ExternalAPI.Dictionary dictionary = (ExternalAPI.Dictionary)MemoryCacher.GetValue(word);

            if (dictionary != null)
            {
                return dictionary;//Ok(dictionary.moreWords.ToString());
            }
            else
            {
                ExternalAPI.Dictionary fetchWord = new ExternalAPI.Dictionary(word);
                MemoryCacher.Add(word, fetchWord, DateTimeOffset.UtcNow.AddMinutes(1));
                return fetchWord;//Ok(fetchWord.moreWords.ToString());
            }  
        }

    }
}
