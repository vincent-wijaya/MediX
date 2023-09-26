using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        private Entities db = new Entities();

        // GET: Bookings
        [Authorize]
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            if (User.IsInRole("Standard"))
            {
                int patientId = db.Patients.First(m => m.AccountId == currentUserId).Id;
                return View(db.Bookings.Include(b => b.Patient).Include(b => b.Staff).Include(b => b.XRayRoom)
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

        [Authorize]
        public JsonResult GetXRayRooms(int medicalCenterId)
        {
            var xRayRooms = db.XRayRooms.Where(x => x.MedicalCenterId == medicalCenterId).Select(x => new { Id = x.Id, RoomNumber = x.RoomNumber }).OrderBy(x => x.RoomNumber).ToList();
            xRayRooms.Insert(0, new { Id = 0, RoomNumber = "Select X-Ray Room Number" });

            return Json(xRayRooms, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetXRayRoomBookTimes(int xRayRoomId)
        {
            
        }

        // GET: Bookings/Create
        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
        public ActionResult Create()
        {
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "FullName");
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name");
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber");
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "FullName");

            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateTime,Notes,IsCompleted,DateCreated,PatientId,XRayRoomId")] Booking booking)
        {
            booking.StaffId = db.Staffs.Where(s => s.AccountId == User.Identity.GetUserId()).First().Id;
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientId = new SelectList(db.Patients, "Id", "FullName");
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name");
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber");
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
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber", booking.XRayRoomId);
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
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber", booking.XRayRoomId);
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
