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
    public class XRayRoomsController : Controller
    {
        private Entities db = new Entities();

        // GET: XRayRooms
        [Authorize(Roles = "MedicalStaff, FacilityManager, Administrator")]
        public ActionResult Index()
        {
            var xRayRooms = db.XRayRooms.Include(x => x.MedicalCenter);
            return View(xRayRooms.ToList());
        }

        // GET: XRayRooms/Details/5
        [Authorize(Roles = "MedicalStaff, FacilityManager, Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            XRayRoom xRayRoom = db.XRayRooms.Find(id);
            if (xRayRoom == null)
            {
                return HttpNotFound();
            }
            return View(xRayRoom);
        }

        // GET: XRayRooms/Create
        [Authorize(Roles = "FacilityManager, Administrator")]
        public ActionResult Create()
        {
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name");
            return View();
        }

        // POST: XRayRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "FacilityManager, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RoomNumber,MedicalCenterId")] XRayRoom xRayRoom)
        {
            if (ModelState.IsValid)
            {
                db.XRayRooms.Add(xRayRoom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name", xRayRoom.MedicalCenterId);
            return View(xRayRoom);
        }

        // GET: XRayRooms/Edit/5
        [Authorize(Roles = "FacilityManager, Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            XRayRoom xRayRoom = db.XRayRooms.Find(id);
            if (xRayRoom == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name", xRayRoom.MedicalCenterId);
            return View(xRayRoom);
        }

        // POST: XRayRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "FacilityManager, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoomNumber,MedicalCenterId")] XRayRoom xRayRoom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(xRayRoom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name", xRayRoom.MedicalCenterId);
            return View(xRayRoom);
        }

        // GET: XRayRooms/Delete/5
        [Authorize(Roles = "FacilityManager, Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            XRayRoom xRayRoom = db.XRayRooms.Find(id);
            if (xRayRoom == null)
            {
                return HttpNotFound();
            }
            return View(xRayRoom);
        }

        // POST: XRayRooms/Delete/5
        [Authorize(Roles = "FacilityManager, Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            XRayRoom xRayRoom = db.XRayRooms.Find(id);
            db.XRayRooms.Remove(xRayRoom);
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
