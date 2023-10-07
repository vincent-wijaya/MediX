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
using Ganss.Xss;    // HTML sanitizer

namespace MediX.Controllers
{
    public class RatingsController : Controller
    {
        private MediX_Entities db = new MediX_Entities();

        // GET: Ratings
        public ActionResult Index()
        {
            var ratings = db.Ratings.Include(r => r.Patient).Include(r => r.MedicalCenter);
            return View(ratings.ToList());
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        private bool IsBookingBelongsToUser(Booking booking)
        {
            string patientId = User.Identity.GetUserId();

            return booking.Patient.AccountId == patientId;
        }

        // GET: Ratings/Create
        public ActionResult Create(int? bookingId)
        {
            if (bookingId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Booking booking = db.Bookings.Find(bookingId);
            if (booking == null)
            {
                return HttpNotFound();
            }

            if (!IsBookingBelongsToUser(booking))
            {
                return new HttpUnauthorizedResult();
            }

            Rating rating = new Rating
            {
                Booking_Id = booking.Id,
                MedicalCenterId = booking.MedicalCenterId,
                PatientId = booking.PatientId
            };
            return View(rating);
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Booking_Id,Value,Comment,MedicalCenterId,PatientId")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                // Sanitise comment
                var sanitizer = new HtmlSanitizer();
                rating.Comment = sanitizer.Sanitize(rating.Comment);

                db.Ratings.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rating);
        }

        // GET: Ratings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Id", rating.PatientId);
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name", rating.MedicalCenterId);
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Value,Comment,LastUpdated,PatientId,MedicalCenterId")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Id", rating.PatientId);
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name", rating.MedicalCenterId);
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
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
