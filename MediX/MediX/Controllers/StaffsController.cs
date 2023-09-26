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
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;

namespace MediX.Controllers
{
    public class StaffsController : Controller
    {
        private MediX_Entities db = new MediX_Entities();

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
                    string accountRole = Request.Form["AccountRole"];

                    await UserManager.AddToRoleAsync(user.Id, accountRole);

                    staff.AccountId = user.Id;

                    db.Staffs.Add(staff);
                    db.SaveChanges();

                    EmailController emailController = new EmailController();
                    emailController.SendLoginDetails(staff.Email, staff.FullName, temporaryPassword);

                    return RedirectToAction("Index");
                }
                else
                {
                    result.Errors.ForEach(error => ModelState.AddModelError("", error.ToString()));
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
            const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string numericChars = "0123456789";
            const string specialChars = "~`!@#$%^&*()_-+={[}]|\\:;\"'<,>.?/";

            Random random = new Random();

            // Ensure at least one character from each category
            string password =
                $"{uppercaseChars[random.Next(uppercaseChars.Length)]}" +
                $"{lowercaseChars[random.Next(lowercaseChars.Length)]}" +
                $"{numericChars[random.Next(numericChars.Length)]}" +
                $"{specialChars[random.Next(specialChars.Length)]}";

            // Generate remaining characters
            int remainingLength = 10 - password.Length; // Adjust the length as needed
            const string allChars = uppercaseChars + lowercaseChars + numericChars + specialChars;

            password += new string(Enumerable.Repeat(allChars, remainingLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            // Shuffle the characters in the password
            password = new string(password.ToCharArray().OrderBy(c => random.Next()).ToArray());

            return password;
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
