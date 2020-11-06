using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEndProject.Models
{
    public class SingleWord
    {
        public int ID;
        public string Category;
        public string Word;
        public int DayTime;
        public List<Link> links=new List<Link>();
        public void SetID(int id)
        {
            ID = id;
        }
        public int GetID()
        {
            return ID;
        }
        public void SetCategory(string category)
        {
            Category = category;
        }
        public string GetCategory()
        {
            return Category;
        }
        public void SetWord(string word)
        {
            Word = word;
        } 
        public string GetWord()
        {
            return Word;
        }
        public void SetDayTime(int time)
        {
            DayTime = time;
        }
        public int GetDayTime()
        {
            return DayTime;
        }
    }
}