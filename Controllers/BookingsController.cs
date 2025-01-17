﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PLGDPBookingApp.Models;
using WebGrease;

namespace PLGDPBookingApp.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        int numberOfBookingExist = 0;

        // GET: Bookings
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View(db.Bookings.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }

        public ActionResult GetData(JqueryDatatableParam param)
        {
            var bookings = db.Bookings.ToList();

            //bookings.ToList().ForEach(x => x.StartDateString = x.StartDate.ToString("dd'/'MM'/'yyyy"));

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                bookings = bookings.Where(x => x.name.ToLower().Contains(param.sSearch.ToLower())
                                              || x.locationname.ToLower().Contains(param.sSearch.ToLower())
                                              || x.startdate.ToString("d MMM yyyy").ToLower().Contains(param.sSearch.ToLower())).ToList();
            }

            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
            var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];

            if (sortColumnIndex == 3)
            {
                bookings = sortDirection == "asc" ? bookings.OrderBy(c => c.startdate).ToList() : bookings.OrderByDescending(c => c.startdate).ToList();
            }
            //else if (sortColumnIndex == 4)
            //{
            //    bookings = sortDirection == "asc" ? bookings.OrderBy(c => c.StartDate) : bookings.OrderByDescending(c => c.StartDate);
            //}
            //else if (sortColumnIndex == 5)
            //{
            //    bookings = sortDirection == "asc" ? bookings.OrderBy(c => c.Salary) : bookings.OrderByDescending(c => c.Salary);
            //}
            //else
            //{
            //    Func<Employee, string> orderingFunction = e => sortColumnIndex == 0 ? e.Name :
            //                                                   sortColumnIndex == 1 ? e.Position :
            //                                                   e.Location;

            //    bookings = sortDirection == "asc" ? bookings.OrderBy(orderingFunction) : bookings.OrderByDescending(orderingFunction);
            //}

            var displayResult = bookings.Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();
            var totalRecords = bookings.Count();

            List<List<string>> aaData = new List<List<string>>();
            foreach (Booking item in displayResult) {
                List<string> oneaaData = new List<string>();
                oneaaData.Add(item.name);
                oneaaData.Add(item.mobileno);
                oneaaData.Add(item.locationname);
                oneaaData.Add(item.startdate.ToString("d MMM yyyy h:mm tt"));
                oneaaData.Add(item.enddate.ToString("d MMM yyyy h:mm tt"));
                oneaaData.Add("<span class='badge bg-" + getProp(item.status) + "'>" + Enum.GetName(typeof(enumStatus), item.status) + "</span>");

                oneaaData.Add("<a href='" + Url.Action("Edit", new { id = item.Id }) +"'> Edit </a> | <a href='" + Url.Action("Details", new { id = item.Id }) + "'> Details </a></li>");

                aaData.Add(oneaaData);
            };

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = aaData
            }, JsonRequestBehavior.AllowGet);

        }

        private string getProp(enumStatus status)
        {
            var prop = "";
            switch (status)
            {
                case enumStatus.Pending:
                    prop = "info";
                    break;
                case enumStatus.Confirmed:
                    prop = "success";
                    break;
                case enumStatus.Cancelled:
                    prop = "warning";
                    break;
                case enumStatus.Rejected:
                    prop = "danger";
                    break;
                default:
                    prop = "primary";
                    break;
            }

            return prop;
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,mobileno,locationname,startdate,enddate,purpose,sector,noofparticipant,isinternet,ispasystem,islcdprojector")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                List<Booking> check = db.Bookings.Where(a => a.startdate <= booking.enddate && a.enddate >= booking.startdate && a.locationname == booking.locationname && (a.status == enumStatus.Confirmed || a.status == enumStatus.Pending)).ToList();
                if (check.Count > 0)
                {
                    ViewBag.Error = "Bertindih dengan " + check.Count + " tempahan lain.";
                    return View(booking);
                }
                else
                {
                    booking.createddate = DateTime.Now;
                    booking.bookingNo = Guid.NewGuid().ToString("n").Substring(0, 8);
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(booking);
        }

        [AllowAnonymous]
        public ActionResult CreatePublic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePublic([Bind(Include = "Id,name,mobileno,locationname,startdate,enddate,purpose,sector,noofparticipant,isinternet,ispasystem,islcdprojector")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                List<Booking> check = db.Bookings.Where(a => a.startdate <= booking.enddate && a.enddate >= booking.startdate && a.locationname == booking.locationname && (a.status == enumStatus.Confirmed || a.status == enumStatus.Pending)).ToList();
                if (check.Count > 0)
                {
                    ViewBag.Error = "Bertindih dengan " + check.Count + " tempahan lain.";
                    return View(booking);
                }
                else
                {
                    booking.createddate = DateTime.Now;
                    booking.bookingNo = Guid.NewGuid().ToString("n").Substring(0, 8);
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    ViewBag.Success = "Tempahan berjaya.";
                }
            }

            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,mobileno,locationname,startdate,enddate,purpose,sector,noofparticipant,isinternet,ispasystem,islcdprojector,createddate,status,remarks")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                List<Booking> check = db.Bookings.Where(a => a.startdate <= booking.enddate && a.enddate >= booking.startdate && a.locationname == booking.locationname && (a.status == enumStatus.Confirmed || a.status == enumStatus.Pending) && a.Id != booking.Id).ToList();
                if (check.Count > 0)
                {
                    ViewBag.Error = "Bertindih dengan " + check.Count + " tempahan lain.";
                    return View(booking);
                }
                else
                {
                    db.Entry(booking).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult checkBooking()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult checkBooking(DateTime startdate, DateTime enddate, string locationname)
        {
            if (isBookingExist(startdate,enddate,locationname) == true)
            {
                ViewBag.Error = "Bertindih dengan " + numberOfBookingExist + " tempahan lain.";
            }
            else
            {
                ViewBag.Success = "Tiada tempahan.";
            }

            return View();
        }

        public bool isBookingExist(DateTime startdate, DateTime enddate, string locationname)
        {
            bool isExist = false;

            List<Booking> check = db.Bookings.Where(a => a.startdate <= enddate && a.enddate >= startdate && a.locationname == locationname && a.status == enumStatus.Confirmed).ToList();
            numberOfBookingExist = check.Count;
            if (numberOfBookingExist > 0)
            {
                isExist = true;
            }

            return isExist;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class JqueryDatatableParam
    {
        public string sEcho { get; set; }
        public string sSearch { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iColumns { get; set; }
        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; }
    }
}
