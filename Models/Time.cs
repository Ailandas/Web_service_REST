using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEndProject.Models
{
    public class Time
    {
        int ID;
        string name;
        string after;
        string upTo;
        public void SetID(int id)
        {
            ID = id;
        }
        public int GetID()
        {
            return ID;
        }
        public void SetName(string demoName)
        {
            name = demoName;
        }
        public string GetName()
        {
            return name;
        }
        public void SetAfter(string demoAfter)
        {
            after = demoAfter;
        }
        public string GetAfter()
        {
            return after;
        }
        public void SetUpTo(string demoBefore)
        {
            upTo = demoBefore;
        }
        public string GetUpTo()
        {
            return upTo;
        }
    }
}