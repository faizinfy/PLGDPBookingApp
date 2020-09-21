using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLGDPBookingApp.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string mobileno { get; set; }
        public string locationname { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string purpose { get; set; }

        public DateTime createddate { get; set; }
    }
}