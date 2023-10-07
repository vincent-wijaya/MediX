using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MediX.Models;
using Microsoft.AspNet.Identity;

namespace MediX.Controllers
{
    public class BookingsController : Controller
    {
        private MediX_Entities db = new MediX_Entities();

        // GET: Bookings
        [Authorize]
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            User.Identity.GetUserName();
            if (User.IsInRole("Standard"))
            {
                int patientId = db.Patients.First(m => m.AccountId == currentUserId).Id;
                return View(db.Bookings.Include(b => b.Patient).Include(b => b.Staff)
                    .Where(m => m.PatientId == patientId).ToList());
            }
            return View(db.Bookings.ToList());
        }

        // GET: Bookings/Details/5
        [Authorize]
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

        //public JsonResult GetPossibleBookingTimes(int medicalCenterId)
        //{
        //var xRayRooms = db.XRayRooms.Where(x => x.MedicalCenterId == medicalCenterId).Select(x => new { Id = x.Id, RoomNumber = x.RoomNumber }).OrderBy(x => x.RoomNumber).ToList();
        //xRayRooms.Insert(0, new { Id = 0, RoomNumber = "Select X-Ray Room Number" });

        //return Json(xRayRooms, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult GetPossibleBookingTimes(int medicalCenterId, DateTime date)
        {
            var medicalCenter = db.MedicalCenters
                                  .Where(m => m.Id == medicalCenterId)
                                  .Select(m => new { m.OpenTime, m.CloseTime })
                                  .FirstOrDefault();

            if (medicalCenter == null)
            {
                return Json(new { success = false, message = "Medical center not found." });
            }

            // Generate a list of possible booking times in 30-minute increments
            var possibleBookingTimes = GeneratePossibleBookingTimes(medicalCenter.OpenTime, medicalCenter.CloseTime);

            // Fetch booked times for a session
            var bookedTimes = FetchBookedTimes(medicalCenterId, date);

            return Json(new { success = true, possibleBookingTimes, bookedTimes }, JsonRequestBehavior.AllowGet);
        }

        private List<string> GeneratePossibleBookingTimes(TimeSpan openTime, TimeSpan closeTime)
        {
            List<string> possibleTimes = new List<string>();
            TimeSpan currentTime = openTime;

            while (currentTime <= closeTime)
            {
                possibleTimes.Add(currentTime.ToString(@"hh\:mm"));
                currentTime += TimeSpan.FromMinutes(30); // Increment by 30 minutes
            }

            return possibleTimes;
        }

        private List<string> FetchBookedTimes(int medicalCenterId, DateTime date)
        {
            var bookedTimes = db.Bookings.Where(b => b.MedicalCenterId == medicalCenterId && DbFunctions.TruncateTime(b.DateTime) == date.Date).Select(b => b.DateTime).ToList();

            var bookedTimesStrings = bookedTimes.Select(time => time.ToString(@"HH\:mm")).ToList();
            
            return bookedTimesStrings;
            //return new List<string> { "10:00" };
        }


        // GET: Bookings/Create
        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
        public ActionResult Create()
        {
            var patients = db.Patients.ToList();
            var patientsWithEmail = patients.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.FullName + " (" + p.Email + ")"
            }).ToList();

            ViewBag.PatientId = new SelectList(patientsWithEmail, "Value", "Text");
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters.OrderBy(mc => mc.Name), "Id", "Name");
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "FullName");

            //List<SelectListItem> medicalCenterList = GetMedicalCenters();

            return View();
        }

        //public ActionResult GetMedicalCenterList(string q)
        //{
        //    var list = new List<MedicalCenter>();
        //    if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
        //    {
        //        list = db.MedicalCenters.Where(mc => mc.Name.ToLower().Contains(q.ToLower())).ToList();
        //    }
        //    return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        //}

        //private List<SelectListItem> GetMedicalCenters()
        //{
        //    List<SelectListItem> medicalCenterList = (from mc in db.MedicalCenters.AsEnumerable()
        //                                              select new SelectListItem
        //                                              {
        //                                                  Text = mc.Name,
        //                                                  Value = mc.Id.ToString()
        //                                              }).ToList();

        //    return medicalCenterList; 
        //}

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DateTime,Notes,PatientId,MedicalCenterId")] Booking booking)
        {
            var staffId = User.Identity.GetUserId();
            booking.StaffId = db.Staffs.Where(s => s.AccountId == staffId ).First().Id;
            booking.IsCompleted = false;
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientId = new SelectList(db.Patients, "Id", "FullName");
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name");
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "FullName");
            return View(booking);
        }

        // GET: Bookings/Edit/5
        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
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
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Id", booking.PatientId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Id", booking.StaffId);
            ViewBag.Id = new SelectList(db.Ratings, "Id", "Comment", booking.Id);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateTime,Notes,IsCompleted,DateCreated,LastUpdated,PatientId,StaffId,XRayRoomId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Id", booking.PatientId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Id", booking.StaffId);
            ViewBag.Id = new SelectList(db.Ratings, "Id", "Comment", booking.Id);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
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
        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Bookings/Complete/5
        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
        public ActionResult Complete(int? id)
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

        // POST: Bookings/Complete/5
        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
        [HttpPost, ActionName("Complete")]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            booking.IsCompleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
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
}
