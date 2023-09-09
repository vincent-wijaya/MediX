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
        private MediX_DatabaseModelContainer db = new MediX_DatabaseModelContainer();


        // GET: Bookings
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();

            IEnumerable<Booking> bookings;
            if (User.IsInRole("Standard"))
            {
                int patientId = db.Patients.First(m => m.AccountId == currentUserId).Id;
                bookings = db.Bookings.Include(b => b.Patient).Include(b => b.BookerStaff).Include(b => b.XRayRoom).Include(b => b.Rating)
                    .Where(m => m.PatientId == patientId);
            }
            else
            {
                bookings = db.Bookings.Include(b => b.Patient).Include(b => b.BookerStaff).Include(b => b.XRayRoom).Include(b => b.Rating);
            }
            return View(bookings.ToList());
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

        //[Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Id");
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber");
            ViewBag.MedicalCenterName = new SelectList(db.MedicalCenters, "Id", "Name");
            ViewBag.Id = new SelectList(db.Ratings, "Id", "Comment");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateTime,Notes,IsCompleted,DateCreated,PatientId,StaffId,XRayRoomId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Id", booking.PatientId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Id", booking.StaffId);
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber", booking.XRayRoomId);
            ViewBag.Id = new SelectList(db.Ratings, "Id", "Comment", booking.Id);
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
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Id", booking.PatientId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Id", booking.StaffId);
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber", booking.XRayRoomId);
            ViewBag.Id = new SelectList(db.Ratings, "Id", "Comment", booking.Id);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
