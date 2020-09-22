using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PLGDPBookingApp.Models;

namespace PLGDPBookingApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            var randomString = Guid.NewGuid().ToString("n").Substring(0, 12);

            if (Request.IsAuthenticated){
                return View(new EventViewModel());
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
        }

        public JsonResult GetEvents()
        {
            var bookings = db.Bookings.ToList();

            var viewModel = new EventViewModel();
            var events = new List<EventViewModel>();
            DateTime start = DateTime.Today.AddDays(-14);
            DateTime end = DateTime.Today.AddDays(-11);

            foreach (Booking item in bookings)
            {
                events.Add(new EventViewModel()
                {
                    id = item.Id,
                    title = item.purpose + "\n" + item.locationname + " - " + item.name,
                    start = item.startdate.ToString("yyyy-MM-dd"),
                    end = item.enddate.AddDays(1).ToString("yyyy-MM-dd"),
                    backgroundColor = "#00a65a",
                    borderColor = "#00a65a",
                    allDay = false
                });

                start = start.AddDays(7);
                end = end.AddDays(7);
            }

            return Json(events.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class EventViewModel
    {
        public Int64 id { get; set; }
        public String title { get; set; }
        public String start { get; set; }
        public String end { get; set; }
        public String backgroundColor { get; set; }
        public String borderColor { get; set; }
        public bool allDay { get; set; }
    }
}