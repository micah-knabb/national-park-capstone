using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Capstone
{
    public class ParkDAL : IParkDAL
    {
        private string _connection;
        private const string _getLastIdSQL = "Select CAST(Scope_identity() as int);";

        public ParkDAL(string connection)
        {
            _connection = connection;
        }

        #region Populate from reader methods

        /// <summary>
        /// Populates a Park class object from when called upon by a SQLDataReader
        /// </summary>
        /// <param name="reader">The SqlDataReader object that is created from a database</param>
        /// <returns></returns>
        private Park PopulateParkInfoFromReader(SqlDataReader reader)
        {
            Park item = new Park();
            item.ParkID = (int)reader["park_id"];
            item.Name = (string)reader["name"];
            item.Location = (string)reader["location"];
            item.EstablishDate = (DateTime)reader["establish_date"];
            item.Area = (int)reader["area"];
            item.Visitors = (int)reader["visitors"];
            item.Description = (string)reader["description"];
            
            return item;
        }
        
        /// <summary>
        /// Populates a Campground class object from when called upon by a SQLDataReader
        /// </summary>
        /// <param name="reader">The SqlDataReader object that is created from a database</param>
        /// <returns></returns>
        private Campground PopulateCampgroundInfoFromReader(SqlDataReader reader)
        {
            Campground item = new Campground();
            item.CampgroundID = (int)reader["campground_id"];
            item.CampgroundName = (string)reader["name"];
            item.OpenFrom = (int)reader["open_from_mm"];
            item.OpenTill = (int)reader["open_to_mm"];
            item.DailyFee = (decimal)reader["daily_fee"];

            return item;
        }
        
        /// <summary>
        /// Populates a Site class object from when called upon by a SQLDataReader when needed by GetSelectedSites (When user selects a date range and campground) 
        /// </summary>
        /// <param name="reader">The SqlDataReader object that is created from a database</param>
        /// <returns></returns>
        private Site PopulateSiteInfoFromReader(SqlDataReader reader)
        {
            Site item = new Site();
            item.SiteID = (int)reader["site_id"];
            item.CampgroundID = (int)reader["campground_id"];
            item.SiteNumber = (int)reader["site_number"];
            item.MaxOccupancy = (int)reader["max_occupancy"];
            item.Accessible = (bool)reader["accessible"];//bool, 0 = false, 1 = true
            item.MaxRVLength = (int)reader["max_rv_length"];
            item.Utilities = (bool)reader["utilities"];//bool, 0 = false, 1 = true
            item.LengthOfStay = (int)reader["days"];
            item.TotalFee = (decimal)reader["fee"];


            return item;
        }

        /// <summary>
        /// Populates a Site class object from when called upon by a SQLDataReader when needed by GetAllSites (When user wants to see all sites in a campground)
        /// </summary>
        /// <param name="reader">The SqlDataReader object that is created from a database</param>
        /// <returns></returns>
        private Site PopulateAllSiteInfoFromReader(SqlDataReader reader)
        {
            Site item = new Site();
            item.SiteID = (int)reader["site_id"];
            item.CampgroundID = (int)reader["campground_id"];
            item.SiteNumber = (int)reader["site_number"];
            item.MaxOccupancy = (int)reader["max_occupancy"];
            item.Accessible = (bool)reader["accessible"];//bool, 0 = false, 1 = true
            item.MaxRVLength = (int)reader["max_rv_length"];
            item.Utilities = (bool)reader["utilities"];//bool, 0 = false, 1 = true
            item.TotalFee = (decimal)reader["daily_fee"];


            return item;
        }
        #endregion

        #region Accessing database methods

        ///<summary>
        ///Accesses the database and returns a list of all parks.
        ///</summary>
        ///<returns>
        ///A list of all current parks in alphabetical order
        ///</returns>
        public List<Park> GetAllParks() 
        {
            List<Park> result = new List<Park>();

            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();

                // Run query
                SqlCommand cmd = new SqlCommand();

                const string SQL_Parks = "SELECT * from Park order by park.name ASC";
                cmd.CommandText = SQL_Parks;
                cmd.Connection = connection;

                // Returns a result set of data
                SqlDataReader reader = cmd.ExecuteReader();

                // Parse results        
                while (reader.Read())
                {
                    var item = PopulateParkInfoFromReader(reader);
                    result.Add(item);
                }
            }
            
            return result;
            
        }
        
        ///<summary>
        ///Accesses the database and returns a list of all campgrounds from a user selected park.
        ///</summary>
        /// <param name="id">The campground ID</param>
        /// <returns>A list of campground objects</returns>
        public List<Campground> GetAllCampgrounds(int id) 
        {
            List<Campground> result = new List<Campground>();
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();

                // Run query
                SqlCommand cmd = new SqlCommand();

                const string SQL_Campgrounds = @"SELECT * from campground where park_id = @park_id";
                cmd.CommandText = SQL_Campgrounds;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@park_id", id);

                // Returns a result set of data
                SqlDataReader reader = cmd.ExecuteReader();

                // Parse results        
                while (reader.Read())
                {
                    var item = PopulateCampgroundInfoFromReader(reader);
                    result.Add(item);
                }
            }
            return result;
        }
        
        /// <summary>
        /// Books a reservation for the user selected campsite. Inserts 
        /// </summary>
        /// <param name="siteId">The site ID number of user selected campsite</param>
        /// <param name="nameForReservation">The name that will be used for the reservation</param>
        /// <param name="fromDate">The start date of the reservation</param>
        /// <param name="toDate">The end date of the reservation</param>
        /// <returns>Confirmation feedback and confirmation id(reservationID primary key)</returns>
        public int BookReservation(int siteId, string nameForReservation, string fromDate, string toDate)
        {
            int confirmationId = 0;
            
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();

                const string SQL_InsertReservation = @"Insert Into reservation (site_id, name, from_date, to_date) " +
                                        "Values (@siteId, @nameForReservation, @fromDate, @toDate);";
                // Run query
                SqlCommand cmd = new SqlCommand(SQL_InsertReservation + _getLastIdSQL, connection);

                cmd.Parameters.AddWithValue("@siteId", siteId);
                cmd.Parameters.AddWithValue("@nameForReservation", nameForReservation);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    confirmationId = (int)cmd.ExecuteScalar(); 
                }
            }


            return confirmationId;
        }
        
        /// <summary>
        /// Accesses the database and returns a list of all campground sites from a user selected campground.
        /// </summary>
        /// <param name="id">The campground ID from a selected campground</param>
        /// <returns>A list of all sites located at a campground</returns>
        public List<Site> GetAllSites(int id)
        {
            List<Site> result = new List<Site>();
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();

                // Run query
                SqlCommand cmd = new SqlCommand();

                const string SQL_Sites = @"SELECT * from site join campground on campground.campground_id = site.campground_id where site.campground_id = @campground_id";
                cmd.CommandText = SQL_Sites;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@campground_id", id);

                // Returns a result set of data
                SqlDataReader reader = cmd.ExecuteReader();

                // Parse results        
                while (reader.Read())
                {
                    var item = PopulateAllSiteInfoFromReader(reader);
                    result.Add(item);
                }
            }
            return result;

        }
        
        /// <summary>
        /// Accesses the database and returns a list of all campground sites that are not reserved from a date range in a particular campground.
        /// </summary>
        /// <param name="id">A campground ID</param>
        /// <param name="start">The start date of a selected reservation</param>
        /// <param name="end">The end date of a selected reservation</param>
        /// <returns></returns>
        public List<Site> GetSelectedSites(int id, string start, string end)
        {
            List<Site> result = new List<Site>();
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();

                // Run query
                SqlCommand cmd = new SqlCommand();

                const string SQL_Sites = @"SELECT TOP (5) *, (DATEDIFF(day, @start, @end)) as 'days', (campground.daily_fee * (DATEDIFF(day, @start, @end))) as 'fee' from site join campground on campground.campground_id = site.campground_id where site.campground_id = @campground_id and site_id not in (select site_id from reservation where reservation.from_date <= @end and reservation.to_date >= @start)";
                                                    
                cmd.CommandText = SQL_Sites;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@campground_id", id);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                // Returns a result set of data
                SqlDataReader reader = cmd.ExecuteReader();

                // Parse results        
                while (reader.Read())
                {
                    var item = PopulateSiteInfoFromReader(reader);
                    result.Add(item);
                }
            }
            return result;

        }
        #endregion

        #region Commented out Code
        //Commented out, as we currently do not have a need for this method, but it might be useful in the future.
        ///// <summary>
        ///// Populates a Site class object from when called upon by a SQLDataReader when needed by GetAllSites (When user wants to see all sites in a campground)
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <returns></returns>
        //#region PopulateReservationInfoFromReader
        //public Reservation PopulateReservationInfoFromReader(SqlDataReader reader)
        //{
        //    Reservation item = new Reservation();
        //    item.ReservationID = (int)reader["reservation_id"];
        //    item.SiteID = (int)reader["site_id"];
        //    item.NameOfReservation = (string)reader["name"];
        //    item.FromDate = (DateTime)reader["from_date"];
        //    item.ToDate = (DateTime)reader["to_date"];
        //    item.CreateDate = (DateTime)reader["create_date"];

        //    return item;
        //}
        #endregion
    }
}
