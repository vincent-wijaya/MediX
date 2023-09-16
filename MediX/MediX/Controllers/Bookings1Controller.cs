using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MediX.Models;

namespace MediX.Controllers
{
    public class Bookings1Controller : Controller
    {
        private MediX db = new MediX();

        // GET: Bookings1
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.Patient).Include(b => b.Staff).Include(b => b.XRayRoom);
            return View(bookings.ToList());
        }

        // GET: Bookings1/Details/5
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

        // GET: Bookings1/Create
        public ActionResult Create()
        {
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "FirstName");
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "FirstName");
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber");
            return View();
        }

        // POST: Bookings1/Create
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

            ViewBag.PatientId = new SelectList(db.Patients, "Id", "FirstName", booking.PatientId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "FirstName", booking.StaffId);
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber", booking.XRayRoomId);
            return View(booking);
        }

        // GET: Bookings1/Edit/5
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
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "FirstName", booking.PatientId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "FirstName", booking.StaffId);
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber", booking.XRayRoomId);
            return View(booking);
        }

        // POST: Bookings1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateTime,Notes,IsCompleted,DateCreated,PatientId,StaffId,XRayRoomId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "FirstName", booking.PatientId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "FirstName", booking.StaffId);
            ViewBag.XRayRoomId = new SelectList(db.XRayRooms, "Id", "RoomNumber", booking.XRayRoomId);
            return View(booking);
        }

        // GET: Bookings1/Delete/5
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

        // POST: Bookings1/Delete/5
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
