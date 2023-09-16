using MediX.Models;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MediX.Controllers
{
    public class EmailController : Controller
    {
        private SendGridClient _sendGridClient;

        public EmailController()
        {
            var apiKey = System.Configuration.ConfigurationManager.AppSettings["SendGridApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                ModelState.AddModelError("", "SendGrid API key is missing or invalid.");
            }

            _sendGridClient = new SendGridClient(apiKey);
        }


        // GET: Email
        public ActionResult SendEmail()
        {
            return View();
        }

        // POST: Email
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmail(SendEmailViewModel model)
        {
            var from = new EmailAddress("vincentwijayaaaaa@gmail.com", "MediX");
            var to = new EmailAddress(model.Email, "to");
            var plainTextContent = model.Body;
            var htmlContent = "<strong>"+ plainTextContent + "</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, model.Subject, plainTextContent, htmlContent);
            var response = await _sendGridClient.SendEmailAsync(msg);
            return View();
        }

    }
}