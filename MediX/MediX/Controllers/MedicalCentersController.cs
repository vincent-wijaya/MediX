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
    public class MedicalCentersController : Controller
    {
        private MediX_Entities db = new MediX_Entities();

        // GET: MedicalCenters
        public ActionResult Index()
        {
            var medicalCenters = db.MedicalCenters.ToList();

            var medicalCenterViewModels = new List<MedicalCenterViewModel>();

            foreach (var medicalCenter in medicalCenters)
            {
                var averageRating = CalculateAverageRating(medicalCenter.Id);
                var count = CountRatings(medicalCenter.Id);

                var viewModel = new MedicalCenterViewModel
                {
                    MedicalCenter = medicalCenter,
                    AverageRating = averageRating,
                    RatingsCount = count
                };

                medicalCenterViewModels.Add(viewModel);
            }

            return View(medicalCenterViewModels);
        }

        // GET: MedicalCenters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalCenter medicalCenter = db.MedicalCenters.Find(id);
            if (medicalCenter == null)
            {
                return HttpNotFound();
            }
            return View(medicalCenter);
        }

        // GET: MedicalCenters/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MedicalCenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Longitude,Latitude,OpenTime,CloseTime")] MedicalCenter medicalCenter)
        {
            if (ModelState.IsValid)
            {
                db.MedicalCenters.Add(medicalCenter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicalCenter);
        }

        // GET: MedicalCenters/Edit/5
        [Authorize(Roles = "Administrator,FacilityManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalCenter medicalCenter = db.MedicalCenters.Find(id);
            if (medicalCenter == null)
            {
                return HttpNotFound();
            }
            return View(medicalCenter);
        }

        // POST: MedicalCenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Longitude,Latitude,OpenTime,CloseTime")] MedicalCenter medicalCenter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalCenter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicalCenter);
        }

        // GET: MedicalCenters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalCenter medicalCenter = db.MedicalCenters.Find(id);
            if (medicalCenter == null)
            {
                return HttpNotFound();
            }
            return View(medicalCenter);
        }

        // POST: MedicalCenters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicalCenter medicalCenter = db.MedicalCenters.Find(id);
            db.MedicalCenters.Remove(medicalCenter);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public int CountRatings(int medicalCenterId)
        {
            return db.Ratings.Where(r => r.MedicalCenterId == medicalCenterId).Count();
        }

        // Calculates average rating value of medical center
        public double CalculateAverageRating(int medicalCenterId)
        {
            var ratingsForMedicalCenter = db.Ratings
                .Where(r => r.MedicalCenterId == medicalCenterId)
                .Select(r =>(double) r.Value);

            if (ratingsForMedicalCenter.Any())
            {
                return Math.Round(ratingsForMedicalCenter.Average(), 1);
            }
            else
            {
                return -1; // default value for no raints
            }
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
