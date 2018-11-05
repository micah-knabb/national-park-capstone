using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Reservation
    {
        public int ReservationID { get; set; }

        public int SiteID { get; set; }

        public string NameOfReservation { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
