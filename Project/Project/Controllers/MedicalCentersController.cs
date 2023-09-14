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
    public class MedicalCentersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MedicalCenters
        public ActionResult Index()
        {
            return View(db.MedicalCenters.ToList());
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: MedicalCenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
