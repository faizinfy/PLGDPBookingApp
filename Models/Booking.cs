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
        public string sector { get; set; }
        public int noofparticipant { get; set; }
        public bool isinternet { get; set; }
        public bool ispasystem { get; set; }
        public bool islcdprojector { get; set; }
        public enumStatus status { get; set; }
        public string remarks { get; set; }
        public string bookingNo { get; set; }
        public DateTime createddate { get; set; }
    }

    public enum enumStatus
    {
        Pending = 0,
        Confirmed = 1,
        Cancelled = 2,
        Rejected = 3
    }
}