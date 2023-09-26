using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MediX.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MediX.Controllers
{
    public class PatientsController : Controller
    {
        private Entities db = new Entities();

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        // GET: Patients
        [Authorize(Roles = "MedicalStaff, FacilityManager, Administrator")]
        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }

        // GET: Patients/Details/5
        [Authorize(Roles = "MedicalStaff, FacilityManager, Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        [Authorize(Roles = "MedicalStaff, FacilityManager, Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "MedicalStaff, FacilityManager, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Email,Address,PhoneNumber")] Patient patient)
        {
            string temporaryPassword = GenerateRandomPassword();

            var user = new ApplicationUser { UserName = patient.Email, Email = patient.Email, PhoneNumber = patient.PhoneNumber };
            var result = await UserManager.CreateAsync(user, temporaryPassword);

            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(user.Id, "Standard");

                patient.AccountId = user.Id;

                db.Patients.Add(patient);
                db.SaveChanges();

                EmailController emailController = new EmailController();
                emailController.SendLoginDetails(patient.Email, patient.FullName, temporaryPassword);

                return RedirectToAction("Index");
            }
            else
            {
                result.Errors.ForEach(error => ModelState.AddModelError("", error.ToString()));
            }

            return View(patient);
        }

        // GET: Patients/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Address,Email,PhoneNumber,LastUpdated,Role")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        [Authorize(Roles = "MedicalStaff, FacilityManager, Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [Authorize(Roles = "MedicalStaff, FacilityManager, Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
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
