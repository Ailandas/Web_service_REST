using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEndProject.Models
{
    public class Category
    {
        public string name;
        public List<Link> links;
        public void SetName(string demoName)
        {
            name = demoName;
        }
        public string GetName()
        {
            return name;
        }
        public void SetLinks(List<Link> demoLinks)
        {
            links = demoLinks;
        }
        public List<Link> GetLinks()
        {
            return links;
        }
    }
}