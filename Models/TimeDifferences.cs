using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEndProject.Models
{
    public static class TimeDifferences
    {
        public static int GetDayOfTimeValue(DateTime now)
        {
            int hourOfDay = DateTime.Now.Hour;
            List<Time> listOfTimes = Models.SqlLite.GetTimes();
            for(int i=0; i<listOfTimes.Count; i++)
            {
                int hourAfter = Convert.ToInt32(listOfTimes[i].GetAfter().Split(':')[0]);
                int hourBefore = Convert.ToInt32(listOfTimes[i].GetUpTo().Split(':')[0]);

                int HourNow= now.Hour;

                
                if(HourNow>=hourAfter && HourNow <= hourBefore)
                {
                    return listOfTimes[i].GetID();
                }
            }
            return 2;
        }
    }
}