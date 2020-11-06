using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEndProject.Models
{
    public class Dictionary
    {
        private string word;
        private List<string> hasCategories;

        public List<string> HasCategories
        {
            get { return hasCategories; }
            set { hasCategories = value; }
        }
        public string Word
        {
            get { return word; }
            set { word = value; }
        }

    }
}