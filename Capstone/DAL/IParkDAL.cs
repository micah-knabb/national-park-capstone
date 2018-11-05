using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Capstone
{
    public interface IParkDAL
    {

        List<Park> GetAllParks();

        List<Campground> GetAllCampgrounds(int id);
        
        List<Site> GetAllSites(int id);

        List<Site> GetSelectedSites(int id, string start, string end);

        int BookReservation(int siteId, string nameForReservation, string fromDate, string toDate);

    }
}
