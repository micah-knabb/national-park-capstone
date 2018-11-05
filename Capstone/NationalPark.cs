using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class NationalPark
    {
        /// <summary>
        /// 
        /// </summary>
        private IParkDAL _db = null;

        /// <summary>
        /// Constructor that initializes connection.
        /// </summary>
        /// <param name="connection">Connection to database</param>
        public NationalPark(string connection)
        {
            _db = new ParkDAL(connection);
            //_db = new MockParkDAL();
        }

        /// <summary>
        /// Gets a list of all the parks in the system.
        /// </summary>
        /// <returns>Returns a list of all parks in alphabetical order</returns>
        public List<Park> GetParksList()
        {
            List<Park> list = _db.GetAllParks();
            return list;
        }

        /// <summary>
        /// Gets a list of all the campgrounds in a particular park
        /// </summary>
        /// <param name="park">Needs a Park class object as an argument</param>
        /// <returns>Returns a list of all campgrounds from a chosen park.</returns>
        public List<Campground> GetCampgroundList(Park park)
        {
            List<Campground> list = _db.GetAllCampgrounds(park.ParkID);

            return list;
        }

        /// <summary>
        /// Returns a dictionary of Parks, with the key being the Park ID and the value being the park class object.
        /// </summary>
        /// <returns>Returns a dictionary of Parks, with the key being the Park ID and the value being the park class object.</returns>
        public Dictionary<int, Park> GetParkDictionary()
        {
            Dictionary<int, Park> ParkDictionary = new Dictionary<int, Park>();
            List<Park> listOfParks = _db.GetAllParks();

            foreach (Park item in listOfParks)
            {
                ParkDictionary.Add(item.ParkID, item);
            }

            return ParkDictionary;
        }

        /// <summary>
        /// Returns a dictionary of Campgrounds from a particular park, with the key being the campground ID and the value being the campground class object.
        /// </summary>
        /// <param name="park">A park object</param>
        /// <returns>Returns a dictionary of Campgrounds from a particular park, with the key being the campground ID and the value being the campground class object.</returns>
        public Dictionary<int, Campground> GetCampgroundDictionary(Park park)
        {
            List<Campground> list = _db.GetAllCampgrounds(park.ParkID);
            Dictionary<int, Campground> CampgroundDictionary = new Dictionary<int, Campground>();

            foreach (Campground item in list)
            {
                CampgroundDictionary.Add(item.CampgroundID, item);
            }
            return CampgroundDictionary;

        }

        /// <summary>
        /// Returns a dictionary of all sites from a particular campground, with the key being the site ID and the value being the site class object.
        /// </summary>
        /// <param name="campground">A campground from which the sites will be returned</param>
        /// <returns>A dictionary of all sites from a particular campground, with the key being the site ID and the value being the site class object</returns>
        public Dictionary<int, Site> GetSiteDictionary(Campground campground)
        {
            List<Site> list = _db.GetAllSites(campground.CampgroundID);
            Dictionary<int, Site> SiteDictionary = new Dictionary<int, Site>();

            foreach (Site item in list)
            {
                SiteDictionary.Add(item.SiteID, item);
            }
            return SiteDictionary;
        }

        /// <summary>
        /// Returns all of the available sites from a selected campground, given a reservation date range. 
        /// </summary>
        /// <param name="campground">A particular campground object specified by the user.</param>
        /// <param name="start">The start of a reservation date in yyyy-mm-dd format.</param>
        /// <param name="end">the end of a reservation date in yyyy-mm-dd format.</param>
        /// <returns>A dictionary of all available sites from a particular campground in a set date range, with the key being the site ID and the value being the site class object</returns>
        public Dictionary<int, Site> GetSelectedSiteDictionary(Campground campground, string start, string end)
        {
            List<Site> list = _db.GetSelectedSites(campground.CampgroundID, start, end);
            Dictionary<int, Site> SiteDictionary = new Dictionary<int, Site>();

            foreach (Site item in list)
            {
                SiteDictionary.Add(item.SiteID, item);
            }
            return SiteDictionary;
        }

        /// <summary>
        /// Books a reservation to the database, given a particular site, reservation name, and a date range.
        /// </summary>
        /// <param name="site">A site object selected by the user.</param>
        /// <param name="reservationName">Name of the person reserving the site.</param>
        /// <param name="start">First day of the reseravation.</param>
        /// <param name="end">Last day of the reservation.</param>
        /// <returns></returns>
        public int AddReservation(Site site, string reservationName, string start, string end)
        {
            int Id = _db.BookReservation(site.SiteID, reservationName, start, end);
      
            return Id;
        }

        /// <summary>
        /// Validates that an arrival date is not before the current date, and that the departure date is not before the arrival date.
        /// </summary>
        /// <param name="departure">Last day of the reservation</param>
        /// <param name="arrival">First day of a reservation</param>
        public void ValidateReservationDates(DateTime departure, DateTime arrival)
        {
            if(arrival < DateTime.Now)
            {
                throw new InvalidDepartureDateException();
            }
            else if(departure < arrival)
            {
                throw new InvalidArrivalDateException();
            }
        }

        /// <summary>
        /// Method for forcing the user to enter a valid int as an input, and returns that value.
        /// </summary>
        /// <param name="message">The message to be displayed to the user for directions on what to select </param>
        /// <returns></returns>
        public static int GetInteger(string message)
        {
            string userInput = String.Empty;
            int intValue = 0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!int.TryParse(userInput, out intValue));

            return intValue;

        }
    }
}




