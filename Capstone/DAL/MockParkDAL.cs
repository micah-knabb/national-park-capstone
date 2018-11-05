using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class MockParkDAL : IParkDAL
    {
        public MockParkDAL()
        {
            // make dummy data
        }

        public int BookReservation()
        {
            throw new NotImplementedException();
        }

        public int BookReservation(int siteId, string nameForReservation, string fromDate, string toDate)
        {
            throw new NotImplementedException();
        }

        public List<Campground> GetAllCampgrounds()
        {
            throw new NotImplementedException();
        }

        public List<Campground> GetAllCampgrounds(int id)
        {
            throw new NotImplementedException();
        }

        public List<Park> GetAllParks()
        {
            throw new NotImplementedException();
        }

        public List<Site> GetAllSites(int id)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, Park> GetParkDictionary()
        {
            throw new NotImplementedException();
        }

        public List<Site> GetSelectedSites(int id, string start, string end)
        {
            throw new NotImplementedException();
        }

        public Park PopulateParkInfoFromReader(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        public Site PopulateSiteInfoFromReader(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        public List<Site> SearchDateAvailability(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }
    }
}
