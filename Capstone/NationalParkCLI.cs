using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class NationalParkCLI
    {
        private NationalPark _park;
        public NationalParkCLI(NationalPark park)
        {
            _park = park;
        }

        public void RunCLI()
        {
            ViewParks();
        }

        public void ViewParks()
        {

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Dictionary<int, Park> park = _park.GetParkDictionary();
                Console.WriteLine("Welcome to the National Parks! Please select your park of interest:");
                Console.WriteLine();

                foreach (KeyValuePair<int, Park> item in park)
                {
                    Console.WriteLine($"{item.Key}) {item.Value.Name}");
                }
                Console.WriteLine();
                Console.WriteLine("A) to view all available park information");
                Console.WriteLine("Q) to quit \n");
                
                Console.WriteLine("Please select the number that corresponds to the Park of interest.");
                
                var selection = Console.ReadKey().KeyChar.ToString();

                if (selection == "q" || selection == "Q")
                {
                    exit = true;
                }
                else if (selection == "a" || selection == "A")
                {
                    DisplayAllParksInfo();
                }
                else
                {
                    try
                    {
                        int sel = int.Parse(selection);
                        if (park.ContainsKey(sel))
                        {
                            ParkInformationScreen(park[sel]);
                        }
                        else
                        {
                            Console.WriteLine(") Please pick a valid park selection");
                            Console.ReadKey();
                        }
                    }
                    catch
                    {

                        Console.WriteLine(") Please pick a valid park selection");
                        Console.ReadKey();
                    }
                }                
            }
        }

        public void ParkInformationScreen(Park park)
        {
            bool exit = false;
            while(!exit)
            {
                Console.Clear();
                Console.WriteLine($"{"Park:".PadRight(20)}{park.Name.PadRight(20)}\n\n" +
                 $"{"Location:".PadRight(20)}{park.Location}\n" +
                 $"{"Established".PadRight(20)}{park.EstablishDate}\n" +
                 $"{"Area".PadRight(20)}{InsertCommas(park.Area)} sq km\n" +
                 $"{"Annual Visitors".PadRight(20)}{InsertCommas(park.Visitors)}\n");
                Console.WriteLine($"{park.Description}\n");

                Console.WriteLine("Select a command:");
                Console.WriteLine($"1) View all campgrounds at {park.Name} Park");
                Console.WriteLine("2) Search for an open reservation within an individual campground");
                Console.WriteLine($"3) Search for open reservations over all of {park.Name} Park campgrounds");
                Console.WriteLine("4) Return to previous screen");
                var selection = Console.ReadKey().KeyChar.ToString();

                if (selection == "1")
                {
                    ViewCampgrounds(park);
                }
                else if (selection == "2")
                {
                    SearchAvailableReservation(park);
                }
                else if (selection == "3")
                {
                    SearchAllCamgrounds(park);
                }
                else if (selection == "4")
                {
                    exit = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please enter a valid entry!");
                    Console.ReadKey();
                }
            }
        }

        public void ViewCampgrounds(Park park)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                List<Campground> list = _park.GetCampgroundList(park);
                Console.WriteLine($"{park.Name} National Park\n");
                Console.WriteLine($"| {"Campground ID".PadRight(15)} | " +
                                 $"| {"Campground Name".PadRight(35)} | " +
                                 $"| {"Opening Month".PadRight(15)} | " +
                                 $"| {"Closing Month".PadRight(15)} | " +
                                 $"| {"Daily Fee".PadRight(10)} | ");
                Console.WriteLine(new string('-', 115));

                foreach (Campground item in list)
                {
                    Console.WriteLine($"| {item.CampgroundID.ToString().PadRight(15)} | " +
                                     $"| {item.CampgroundName.PadRight(35)} | " +
                                     $"| {item.DisplayMonthOpen.ToString().PadRight(15)} | " +
                                     $"| {item.DisplayMonthClose.ToString().PadRight(15)} | " +
                                     $"| {item.DailyFee.ToString("C").PadRight(10)} |");
                }
                Console.WriteLine("\nSelect a command");
                Console.WriteLine("1) Search a campground for available reservation by date range");
                Console.WriteLine("2) View all sites that are in a campground");
                Console.WriteLine("3) Return to previous menu");
                var selection = Console.ReadKey().KeyChar.ToString();

                if (selection == "1")
                {
                    SearchAvailableReservation(park);
                }
                else if (selection == "2")
                {
                    ViewAllIndividualSites(park);
                }
                else if (selection == "3")
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Please enter a valid entry!");
                    Console.ReadKey();
                }
            }
        }

        public void SearchAvailableReservation(Park park)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($"{park.Name} National Park");
                Console.WriteLine($"| {"Campground ID".PadRight(15)} | " +
                                     $"| {"Campground Name".PadRight(35)} | " +
                                     $"| {"Month Open".PadRight(10)} | " +
                                     $"| {"Closing Month".PadRight(15)} | " +
                                     $"| {"Daily Fee".PadRight(10)} | ");
                Console.WriteLine(new string('-', 100));

                Dictionary<int, Campground> CampgroundDictionary = _park.GetCampgroundDictionary(park);
                foreach (KeyValuePair<int, Campground> item in CampgroundDictionary)
                {
                    Console.WriteLine($"| {item.Value.CampgroundID.ToString().PadRight(15)} | " +
                                     $"| {item.Value.CampgroundName.PadRight(35)} | " +
                                     $"| {item.Value.DisplayMonthOpen.ToString().PadRight(10)} | " +
                                     $"| {item.Value.DisplayMonthClose.ToString().PadRight(15)} | " +
                                     $"| {item.Value.DailyFee.ToString("C").PadRight(10)} |");
                }
                int sel = NationalPark.GetInteger("Which campground would you like to search for an available reservation? (enter 0 to cancel)?");
                if (sel == 0)
                {
                    exit = true;
                }
                try
                {
                    if (CampgroundDictionary.ContainsKey(sel))
                    {
                        Console.WriteLine("What is the arrival date? Enter date as: yyyy-mm-dd:");
                        string arrival = Console.ReadLine().ToString();
                        DateTime arr = DateTime.Parse(arrival);

                        Console.WriteLine("What is the departure date? Enter date as: yyyy-mm-dd:");
                        string departure = Console.ReadLine().ToString();
                        DateTime dep = DateTime.Parse(departure);

                        _park.ValidateReservationDates(dep, arr);
                        ViewAllSelectedSites(CampgroundDictionary[sel], arrival, departure);
                    }
                    else
                    {
                        Console.WriteLine("Please only enter a valid campground number");
                        Console.ReadKey();
                    }
                }
                catch (InvalidArrivalDateException)
                {
                    Console.WriteLine("Please only enter future dates.");
                    Console.ReadKey();
                }
                catch (InvalidDepartureDateException)
                {
                    Console.WriteLine("Please only enter and end date that is after your arrival date.");
                    Console.ReadKey();
                }
                catch (Exception)
                {
                    Console.WriteLine(") Please only enter a valid campground number");
                    Console.ReadKey();
                }
            }
        }
        
        public void ViewAllSelectedSites(Campground campground, string start, string end)
        {
            bool exit = false;
            while(!exit)
            { 
                Console.Clear();
                Dictionary<int, Site> Sites = _park.GetSelectedSiteDictionary(campground, start, end);
                if(Sites.Count > 0)
                { 

                    Console.WriteLine($"| {"Site ID".PadRight(15)} | " +
                                        $"| {"Site Number".PadRight(15)} | " +
                                        $"| {"Max Occup.".PadRight(15)}" +
                                     $"| {"Accessible?".PadRight(15)} | " +
                                        $"| {"Max RVLength".PadRight(15)} | " +
                                        $"| {"Utility".PadRight(15)} |" +
                                        $"| {"Cost".PadRight(15)} ");
                    Console.WriteLine(new string('-', 120));
                
                    foreach (KeyValuePair<int, Site> item in Sites)
                    {
                        Console.WriteLine($"| {item.Key.ToString().PadRight(15)} | " +
                                        $"| {item.Value.SiteNumber.ToString().PadRight(15)} | " +
                                        $"| {item.Value.MaxOccupancy.ToString().PadRight(15)}" +
                                         $"| {item.Value.Accessible.ToString().PadRight(15)} | " +
                                         $"| {item.Value.MaxRVLength.ToString().PadRight(15)} | " +
                                         $"| {item.Value.Utilities.ToString().PadRight(15)} |" +
                                         $"| {item.Value.TotalFee.ToString("C")}".PadRight(15));
                    }
                    var selection = NationalPark.GetInteger("Which site would you like to reserve? Enter the site ID to select, or enter 0 to return");
                    if (selection == 0)
                    {
                        Console.Clear();
                        exit = true;
                    }
                    else if (Sites.ContainsKey(selection))
                    {
                        Console.WriteLine("What name should the reservation be made under?");
                        string reservationName = Console.ReadLine();
                        int reservationID = _park.AddReservation(Sites[selection], reservationName, start, end);
                        Console.WriteLine($"you're reservation number is {reservationID}. Thanks!");
                        Console.ReadKey();
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine("Please only enter a site number that exists");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("There are no available campsites for this date range. Please select another campground or date range.");
                    Console.ReadKey();
                    exit = true;
                }
            }
        }

        public void ViewAllIndividualSites(Park park)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                
                Console.WriteLine($"{park.Name} National Park\n");
                Console.WriteLine($"| {"Campground ID".PadRight(15)} | " +
                                     $"| {"Campground Name".PadRight(35)} | " +
                                     $"| {"Month Open".PadRight(10)} | " +
                                     $"| {"Closing Month".PadRight(15)} | " +
                                     $"| {"Daily Fee".PadRight(10)} | ");
                Console.WriteLine(new string('-', 100));

                Dictionary<int, Campground> CampgroundDictionary = _park.GetCampgroundDictionary(park);
                foreach (KeyValuePair<int, Campground> item in CampgroundDictionary)
                {
                    Console.WriteLine($"| {item.Value.CampgroundID.ToString().PadRight(15)} | " +
                                     $"| {item.Value.CampgroundName.PadRight(35)} | " +
                                     $"| {item.Value.DisplayMonthOpen.ToString().PadRight(10)} | " +
                                     $"| {item.Value.DisplayMonthClose.ToString().PadRight(15)} | " +
                                     $"| {item.Value.DailyFee.ToString("C").PadRight(10)} |");
                }
                int selection = NationalPark.GetInteger("Enter a campground by ID to view details for all of its sites, or enter 0 to return.");
                if (selection == 0)
                {
                    exit = true;
                }
                else if (CampgroundDictionary.ContainsKey(selection))
                {
                    Campground camp = CampgroundDictionary[selection];
                    Dictionary<int, Site> SiteDictionary = _park.GetSiteDictionary(CampgroundDictionary[selection]);
                    Console.Clear();
                    Console.WriteLine($"| {"Site Number".PadRight(15)} | " +
                    $"| {"Max Occup.".PadRight(15)}" +
                    $"| {"Accessible?".PadRight(15)} | " +
                    $"| {"Max RVLength".PadRight(15)} | " +
                    $"| {"Utility".PadRight(15)} | " +
                    $"| {"Cost".PadRight(15)} ");
                    Console.WriteLine(new string('-', 100));
                    foreach (KeyValuePair<int, Site> item in SiteDictionary)
                    {
                        Console.WriteLine($"| {item.Value.SiteNumber.ToString().PadRight(15)} | " +
                                        $"| {item.Value.MaxOccupancy.ToString().PadRight(15)}" +
                                         $"| {item.Value.Accessible.ToString().PadRight(15)} | " +
                                         $"| {item.Value.MaxRVLength.ToString().PadRight(15)} | " +
                                         $"| {item.Value.Utilities.ToString().PadRight(15)} | " +
                                         $"| {item.Value.TotalFee.ToString("C").PadRight(15)}");
                    }
                    Console.WriteLine("Press any key to return");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Please only enter a valid campgroundID number");
                    Console.ReadKey();
                }
            }

        }
        
        public void DisplayAllParksInfo()
        {
            List<Park> list = _park.GetParksList();

            foreach (Park item in list)
            {
                Console.WriteLine($"Park: {item.Name.PadRight(20)}\n\n" +
                 $"{"Location:".PadRight(20)}{item.Location}\n" +
                 $"{"Established:".PadRight(20)}{item.EstablishDate}\n" +
                 $"{"Area:".PadRight(20)}{InsertCommas(item.Area)} sq km\n" +
                 $"{"Annual Visitors:".PadRight(20)}{InsertCommas(item.Visitors)}\n");
                Console.WriteLine($"{item.Description}");
                Console.WriteLine();
                Console.WriteLine(new string('-', 100));
                Console.WriteLine();
            }
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        ///<summary>
        ///Inserts a comma every three digits from right to left and returns the result string. If the int is 3 digits
        ///or less, it is returned as a string without commas.
        ///</summary>
        public string InsertCommas(int number)
        {   
            string numString = number.ToString();
            string result = numString;
            
            if (number >= 1000)
            {
                result = $"{numString.Substring(numString.Length - 3, 3)}";

                for (int i = 4; i <= numString.Length; i++)
                {
                    
                    if (i % 3 == 1)
                    {
                        result = $"{numString[numString.Length - i]},{result}";
                    }
                    else
                    {
                        result = $"{numString[numString.Length - i]}{result}";
                    }
                    
                }

            }

            return result;
        }

        public void SearchAllCamgrounds(Park park)
        {
            bool exit = false;
            while (!exit)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine($"Please enter the reservation date information to search for open campsites at {park.Name}.\n ");
                    Console.WriteLine("What is the arrival date? Enter date as: yyyy-mm-dd:");
                    string arrival = Console.ReadLine().ToString();
                    DateTime arr = DateTime.Parse(arrival);

                    Console.WriteLine("What is the departure date? Enter date as: yyyy-mm-dd:");
                    string departure = Console.ReadLine().ToString();
                    DateTime dep = DateTime.Parse(departure);
                    _park.ValidateReservationDates(dep, arr);
                    Console.Clear();

                    Console.WriteLine($"Here are available campsites at {park.Name} Park between {arrival} and {departure}\n");
                    List<Campground> list = _park.GetCampgroundList(park);
                    Dictionary<int, Site> selectedSites = new Dictionary<int, Site>();
                    foreach (Campground camp in list)
                    {
                        Console.WriteLine($"| Campground: {camp.CampgroundName.ToString().PadRight(15)}");
                        Console.WriteLine("| Campsites:".PadRight(15));
                        Dictionary<int, Site> Sites = _park.GetSelectedSiteDictionary(camp, arrival, departure);

                        Console.WriteLine($"| {"Site ID".PadRight(15)} | " +
                                            $"| {"Site Number".PadRight(15)} | " +
                                            $"| {"Max Occup.".PadRight(15)}" +
                                            $"| {"Accessible?".PadRight(15)} | " +
                                            $"| {"Max RVLength".PadRight(15)} | " +
                                            $"| {"Utility".PadRight(15)} |" +
                                            $"| {"Cost".PadRight(15)} ");
                        Console.WriteLine(new string('-', 120));

                        foreach (KeyValuePair<int, Site> item in Sites)
                        {
                            selectedSites.Add(item.Key, item.Value);
                            Console.WriteLine($"| {item.Key.ToString().PadRight(15)} | " +
                                            $"| {item.Value.SiteNumber.ToString().PadRight(15)} | " +
                                            $"| {item.Value.MaxOccupancy.ToString().PadRight(15)}" +
                                                $"| {item.Value.Accessible.ToString().PadRight(15)} | " +
                                                $"| {item.Value.MaxRVLength.ToString().PadRight(15)} | " +
                                                $"| {item.Value.Utilities.ToString().PadRight(15)} |" +
                                                $"| {item.Value.TotalFee.ToString("C")}".PadRight(15));
                        }
                        Console.WriteLine();
                    }
                    bool exit2 = false;
                    while(!exit2)
                    {
                        int selection = NationalPark.GetInteger("Which campsite would you like to make a reservation under? (enter 0 to cancel)?");
                        if (selection == 0)
                        {
                            Console.Clear();
                            exit = true; exit2 = true;
                        }
                        else if (selectedSites.ContainsKey(selection))
                        {
                            Console.WriteLine("What name should the reservation be made under?");
                            string reservationName = Console.ReadLine();
                            int reservationID = _park.AddReservation(selectedSites[selection], reservationName, arrival, departure);
                            Console.WriteLine($"you're reservation number is {reservationID}. Thanks!");
                            Console.ReadKey();
                            exit = true; exit2 = true;
                        }
                        else
                        {
                            Console.WriteLine("Please only enter a site number that exists");
                        }
                    }
                }
                catch (InvalidArrivalDateException)
                {
                    Console.WriteLine("Please only enter future dates.");
                    Console.ReadKey();
                }
                catch (InvalidDepartureDateException)
                {
                    Console.WriteLine("Please only enter and end date that is after your arrival date.");
                    Console.ReadKey();
                }
                catch
                {
                    Console.WriteLine("Please only enter the date in a valid format");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

    }
}
