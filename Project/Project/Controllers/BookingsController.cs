using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class BookingsController : Controller
    {
        private MediX_ModelContainer1 db = new MediX_ModelContainer1();

        // GET: Bookings
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.XRayRoom).Include(b => b.BookerStaff).Include(b => b.Patient);
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

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber");
            ViewBag.StaffId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.PatientId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateTime,Notes,XRayRoomId,StaffId,DateCreated,LastUpdated,IsCompleted,PatientId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber", booking.XRayRoomId);
            ViewBag.StaffId = new SelectList(db.Users, "Id", "FirstName", booking.StaffId);
            ViewBag.PatientId = new SelectList(db.Users, "Id", "FirstName", booking.PatientId);
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
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber", booking.XRayRoomId);
            ViewBag.StaffId = new SelectList(db.Users, "Id", "FirstName", booking.StaffId);
            ViewBag.PatientId = new SelectList(db.Users, "Id", "FirstName", booking.PatientId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateTime,Notes,XRayRoomId,StaffId,DateCreated,LastUpdated,IsCompleted,PatientId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber", booking.XRayRoomId);
            ViewBag.StaffId = new SelectList(db.Users, "Id", "FirstName", booking.StaffId);
            ViewBag.PatientId = new SelectList(db.Users, "Id", "FirstName", booking.PatientId);
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
