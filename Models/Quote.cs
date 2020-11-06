using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEndProject.Models
{
    public class Quote
    {
        private string message;
        private string author;
        private string keywords;
        private string profession;
        private string nationality;
        private string authorBirth;
        private string authorDeath;

        public Quote() 
        { }

        public string AuthorDeath
        {
            get { return authorDeath; }
            set { authorDeath = value; }
        }
        public string AuthorBirth
        {
            get { return authorBirth; }
            set { authorBirth = value; }
        }
        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }
        public string Profession
        {
            get { return profession; }
            set { profession = value; }
        }
        public string Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        public string Message
        {
            get { return message; }
            set { message = value; }
        }


    }
}