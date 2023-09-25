using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using MediX.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MediX.Controllers
{
    public class StaffsController : Controller
    {
        private Entities db = new Entities();

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

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
        public async Task<ActionResult> Create([Bind(Include = "FirstName,LastName,DateOfBirth,Address,Email,MedicalCenterId")] Staff staff)
        {
            //var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
            //var result = await UserManager.CreateAsync(user, model.Password);
            //await UserManager.AddToRoleAsync(user.Id, "Standard");

             if (ModelState.IsValid)
            {
                string temporaryPassword = GenerateRandomPassword();


                var user = new ApplicationUser { UserName = staff.Email, Email = staff.Email };
                var result = await UserManager.CreateAsync(user, temporaryPassword);


                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "MedicalStaff");

                    staff.AccountId = user.Id;

                    db.Staffs.Add(staff);
                    db.SaveChanges();

                    string medicalCenterName = db.MedicalCenters.Where(mc => mc.Id == staff.MedicalCenterId).ToList().First().Name;
                    EmailController emailController = new EmailController();
                    emailController.SendLoginDetails(staff.Email, staff.FullName, medicalCenterName, temporaryPassword);
                    return RedirectToAction("Index");
                }
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

        public string GenerateRandomPassword()
        {
            Random random = new Random();
            int length = 10;

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~`! @#$%^&*()_-+={[}]|\\:;\"'<,>.?/";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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
