using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Campground
    {
        public int CampgroundID { get; set; }

        public int ParkID { get; set; }

        public string CampgroundName { get; set; }

        public int OpenFrom { get; set; }
        
        private string GetMonthString(int month)
        {
            if (month == 1)
            {
                return "January";
            }
            else if (month == 2)
            {
                return "Febuary";
            }
            else if (month == 3)
            {
                return "March";
            }
            else if (month == 4)
            {
                return "April";
            }
            else if (month == 5)
            {
                return "May";
            }
            else if (month == 6)
            {
                return "June";
            }
            else if (month == 7)
            {
                return "July";
            }
            else if (month == 8)
            {
                return "August";
            }
            else if (month == 9)
            {
                return "September";
            }
            else if (month == 10)
            {
                return "October";
            }
            else if (month == 11)
            {
                return "November";
            }
            else if (month == 12)
            {
                return "December";
            }
            else
            {
                return "Invalid";
            }
        }

        public string DisplayMonthOpen
        {
            get
            {
                return GetMonthString(OpenFrom);
            }            
        }
        
        public int OpenTill { get; set; }

        public string DisplayMonthClose
        {
            get
            {
                return GetMonthString(OpenTill);
            }

        }

        public decimal DailyFee { get; set; }
    }
}
