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
    [TestClass()]
    public class ParkSqlDALTests
    {
        private TransactionScope _tran;
        private string connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = NationalPark; Integrated Security = True";
        private int _numberOfParks= 0;
        private int _numberOfCampgroundsWithinPark = 0;
        private int _numberOfSitesWithinCampground = 0;
        private int _numberOfSitesAvailable = 0;




        [TestInitialize]
        public void Initialize()
        {
            _tran = new TransactionScope();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                               
                cmd = new SqlCommand("Select Count(*) From park;", conn);
                _numberOfParks = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("Select Count(*) From campground Where park_id = 1;", conn);
                _numberOfCampgroundsWithinPark = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("Select Count(*) From site Where campground_id = 1;", conn);
                _numberOfSitesWithinCampground = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("SELECT Count(*) " + 
                    "From site Join campground on campground.campground_id = site.campground_id " +
                    "Where site.campground_id = 1 and site_id not in (Select site_id " +
                                                                        "From reservation " +
                                                                        "Where reservation.from_date <= '2018-10-20' and reservation.to_date >= '2018-10-15');", conn);
                _numberOfSitesAvailable = (int)cmd.ExecuteScalar();

                if(_numberOfSitesAvailable >= 5)
                {
                    _numberOfSitesAvailable = 5;
                }
                //Insert a Dummy Record for City that belongs to 'ABC Country'
                //If we want to the new id of the record inserted we can use
                // SELECT CAST(SCOPE_IDENTITY() as int) as a work-around
                // This will get the newest identity value generated for the record most recently inserted
                //cmd = new SqlCommand("INSERT INTO City VALUES ('Test City', 'ABC', 'Test District', 1); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                //cityId = (int)cmd.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose();
        }

        [TestMethod()]
        public void ParkDALTests()
        {
            /*TEST GetAllParks method*/
            
            //Arrange
            ParkDAL parkDAL = new ParkDAL(connectionString);

            //Act
            List<Park> parks = parkDAL.GetAllParks();
            
            //Assert
            Assert.AreEqual(_numberOfParks, parks.Count);//retrieved the number of rows beforehand to compare to the count in the returned list


            /*TEST GetAllCampgrounds method*/

            List<Campground> campgrounds = parkDAL.GetAllCampgrounds(parks[0].ParkID); //using the first index of the returned list from GetAllParks

            Assert.AreEqual(_numberOfCampgroundsWithinPark, campgrounds.Count);//retrieved number of campgrounds within park_id = 1 beforehand


            /*TEST GetAllSites method*/

            List<Site> sites = parkDAL.GetAllSites(campgrounds[0].CampgroundID); //using the first index of the returned list from GetAllCampgrounds

            Assert.AreEqual(_numberOfSitesWithinCampground, sites.Count);//retrieved number of campgrounds within campground_id = 1 beforehand

            /*TEST GetAllSites method*/

            //using the first index of the returned list from GetAllCampgrounds, a test from_date, a test to_date
            List<Site> sitesAvailable = parkDAL.GetSelectedSites(campgrounds[0].CampgroundID, "2018-10-15", "2018-10-20");

            Assert.AreEqual(_numberOfSitesAvailable, sitesAvailable.Count);//retrieved number of campgrounds within campground_id = 1 beforehand
            
        }
    }
}
