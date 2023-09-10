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
    public class PatientBookingsController : Controller
    {
        private MediX_DatabaseModelContainer db = new MediX_DatabaseModelContainer();


        // GET: Bookings
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();

            IEnumerable<Booking> bookings;
            if (User.IsInRole("Standard"))
            {
                int patientId = db.Patients.First(m => m.AccountId == currentUserId).Id;
                bookings = db.Bookings.Include(b => b.Patient).Include(b => b.BookerStaff).Include(b => b.XRayRoom).Include(b => b.Rating)
                    .Where(m => m.PatientId == patientId);
            }
            else
            {
                bookings = db.Bookings.Include(b => b.Patient).Include(b => b.BookerStaff).Include(b => b.XRayRoom).Include(b => b.Rating);
            }
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
