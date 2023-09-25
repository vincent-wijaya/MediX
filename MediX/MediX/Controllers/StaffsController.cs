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
    public class StaffsController : Controller
    {
        private Entities db = new Entities();

        // GET: Staffs
        [Authorize(Roles = "FacilityManager, Administrator")]
        public ActionResult Index()
        {
            var staffs = db.Staffs.Include(s => s.MedicalCenter);
            return View(staffs.ToList());
        }

        // GET: Staffs/Details/5
        [Authorize(Roles = "FacilityManager, Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: Staffs/Create
        [Authorize(Roles = "FacilityManager, Administrator")]
        public ActionResult Create()
        {
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "FacilityManager, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Address,Email,PhoneNumber,LastUpdated,Role,MedicalCenterId")] Staff staff)
        {
            //var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
            //var result = await UserManager.CreateAsync(user, model.Password);
            //await UserManager.AddToRoleAsync(user.Id, "Standard");

             if (ModelState.IsValid)
            {
                db.Staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name", staff.MedicalCenterId);
            return View(staff);
        }

        // GET: Staffs/Edit/5
        [Authorize(Roles = "FacilityManager, Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name", staff.MedicalCenterId);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "FacilityManager, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id,FirstName,LastName,DateOfBirth,Address,Email,PhoneNumber,LastUpdated,Role,MedicalCenterId")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicalCenterId = new SelectList(db.MedicalCenters, "Id", "Name", staff.MedicalCenterId);
            return View(staff);
        }

        // GET: Staffs/Delete/5
        [Authorize(Roles = "FacilityManager, Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [Authorize(Roles = "FacilityManager, Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);
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
