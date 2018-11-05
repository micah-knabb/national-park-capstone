using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using Capstone;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Tests
{
    [TestClass]
    public class NationalParkTests
    {
        private TransactionScope _tran;
        public string connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = NationalPark; Integrated Security = True";
        NationalPark ParkObject = new NationalPark(@"Data Source =.\SQLEXPRESS;Initial Catalog = NationalPark; Integrated Security = True");
        int numberOfCampgroundsParkOne;
        int numberSitesParkOneCampgroundOne;
        int numberOfParks;
       

        [TestInitialize]
        public void Initialize()
        {
            _tran = new TransactionScope();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                //Gets the number of cmpgrounds in park one to test in GetCampgroundList() and GetCampgroundDictionaryTest()
                cmd = new SqlCommand("Select Count(*) From campground join park on campground.park_id = park.park_id where park.park_id = 1", conn);
                numberOfCampgroundsParkOne = (int)cmd.ExecuteScalar();

                //Gets the number of sites in Park 1, campground 1 to be tested in GetSiteDictionaryTest()
                cmd = new SqlCommand("Select Count(*) From site join campground on site.campground_id = campground.campground_id join park on campground.park_id = park.park_id where park.park_id = 1 and campground.campground_id = 1", conn);
                numberSitesParkOneCampgroundOne = (int)cmd.ExecuteScalar();

                //Gets the number of parks
                cmd = new SqlCommand("Select Count(*) From parks", conn);
                numberOfParks = (int)cmd.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose();
        }

        [TestMethod()]
        public void GetParksListTest()
        {
           
            List<Park> AllParks = ParkObject.GetParksList();

            Assert.IsNotNull(AllParks);
            Assert.AreEqual(numberOfParks, AllParks.Count);
        }

        [TestMethod]
        public void GetParkDictionaryTest()
        {
            Dictionary<int, Park> ParkDictionary = ParkObject.GetParkDictionary();

            Assert.IsNotNull(ParkDictionary);
            Assert.AreEqual(numberOfParks, ParkDictionary.Count);
            Assert.AreEqual(2, ParkDictionary[2].ParkID); //tests to see if the key for the dictionary is the same as the park ID it returns.
        }

        [TestMethod]
        public void GetCampgroundListTest()
        {
            Dictionary<int, Park> ParkDictionary = ParkObject.GetParkDictionary();
            Park park = ParkDictionary[1];
            List<Campground> CampgroundList = ParkObject.GetCampgroundList(park);

            Assert.IsNotNull(ParkDictionary);
            Assert.AreEqual(numberOfCampgroundsParkOne, CampgroundList.Count);
        }

        [TestMethod]
        public void GetCampgroundDictionaryTest()
        {
            Dictionary<int, Park> ParkDictionary = ParkObject.GetParkDictionary();
            Park park = ParkDictionary[1];

            Dictionary<int, Campground> CampgroundDictionary = ParkObject.GetCampgroundDictionary(park);

            Assert.IsNotNull(CampgroundDictionary);
            Assert.AreEqual(numberOfCampgroundsParkOne, CampgroundDictionary.Count);
        }

        [TestMethod]
        public void GetSiteDictionaryTest()
        {
            Dictionary<int, Park> ParkDictionary = ParkObject.GetParkDictionary();
            Park park = ParkDictionary[1];
            Dictionary<int, Campground> CampgroundDictionary = ParkObject.GetCampgroundDictionary(park);
            Campground Camp = CampgroundDictionary[1];

            Dictionary<int, Site> SiteDict = ParkObject.GetSiteDictionary(Camp);

            Assert.IsNotNull(SiteDict);
            Assert.AreEqual(numberSitesParkOneCampgroundOne, SiteDict.Count);
        }

        [TestMethod]
        public void GetSelectedSiteDictionaryTest()
        {
            Dictionary<int, Park> ParkDictionary = ParkObject.GetParkDictionary();
            Park park = ParkDictionary[1];
            Dictionary<int, Campground> CampgroundDictionary = ParkObject.GetCampgroundDictionary(park);
            Campground Camp = CampgroundDictionary[1];

            Dictionary<int, Site> selectedSiteDictionary = ParkObject.GetSelectedSiteDictionary(Camp, "2018-11-11", "2018-11-13");

            Assert.IsNotNull(selectedSiteDictionary);
        }

        [TestMethod]
        public void AddReservationTest()
        {
            Dictionary<int, Park> ParkDictionary = ParkObject.GetParkDictionary();//tests GetParkDictionary method
            Park park = ParkDictionary[1];//gets the first park from the dictionary
            Dictionary<int, Campground> CampgroundDictionary = ParkObject.GetCampgroundDictionary(park);//tests the GetCampgroundDictionary method
            Campground Camp = CampgroundDictionary[1];//gets the first Campground for the dictionary
            Dictionary<int, Site> selectedSiteDictionary = ParkObject.GetSelectedSiteDictionary(Camp, "2018-11-11", "2018-11-13");//tests the GetSelectedSiteDictionary method
            List<int> siteList = selectedSiteDictionary.Keys.ToList();
            int siteID = siteList[0];//selects the first site from the list
            int worked = ParkObject.AddReservation(selectedSiteDictionary[siteID], "name", "2018-11-11", "2018-11-13");//tests the AddReservation method

            Assert.IsNotNull(worked);//passes if the reservation was made
        }
    }
}
