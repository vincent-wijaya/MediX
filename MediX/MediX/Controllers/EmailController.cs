using MediX.Models;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using Microsoft.Ajax.Utilities;

namespace MediX.Controllers
{
    public class EmailController : Controller
    {
        private MediX_Entities db = new MediX_Entities();
        private SendGridClient _sendGridClient;
        public EmailController()
        {
            var apiKey = Environment.GetEnvironmentVariable("SendGridAPIKey");

            if (string.IsNullOrEmpty(apiKey))
            {
                ModelState.AddModelError("", "SendGrid API key is missing or invalid.");
            }

            _sendGridClient = new SendGridClient(apiKey);
        }

        


        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
        public ActionResult Create()
        {
            return View();
        }

        // TODO: Add notification to indicate success/error

        [HttpPost]
        [Authorize(Roles = "Administrator,FacilityManager,MedicalStaff")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Emails,Subject,Body")] SendEmailViewModel model, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                List<string> toEmails = model.Emails.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var subject = model.Subject;
                var body = model.Body;

                List<string> toNames = new List<string>();
                List<string> emailsToRemove = new List<string>();

                foreach (var toEmail in toEmails)
                {

                    var patient = db.Patients.FirstOrDefault(p => p.Email == toEmail);
                    if (patient != null)
                    {
                        toNames.Add(patient.FullName);
                    }
                    else
                    {
                        var staff = db.Staffs.FirstOrDefault(s => s.Email == toEmail);
                        if (staff != null)
                        {
                            toNames.Add(staff.FullName);
                        }
                        else
                        {
                            emailsToRemove.Add(toEmail);
                        }
                    }
                }

                foreach (var email in emailsToRemove) 
                {
                    toEmails.Remove(email);
                }

                var plainTextContent = "Dear MediX user,\r\n\r\n" + body +
                    "\r\nMediX\r\nWellington Rd,\r\nClayton VIC 3800";

                var htmlContent =
                "<!DOCTYPE html><html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" lang=\"en\">" +
                "   <head><title></title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><!--[if mso]><xml>" +
                "<o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]-->" +
                "<style>\r\n*{box-sizing:border-box}body{margin:0;padding:0}a[x-apple-data-detectors]" +
                "{color:inherit!important;text-decoration:inherit!important}#MessageViewBody a{color:inherit;text-decoration:none}p{line-height:inherit}.desktop_hide,.desktop_hide " +
                "table{mso-hide:all;display:none;max-height:0;overflow:hidden}.image_block img+div{display:none} " +
                "@media (max-width:720px){.mobile_hide{display:none}.row-content{width:100%!important}.stack .column{width:100%;display:block}.mobile_hide{min-height:0;max-height:0;max-width:0;overflow:hidden;font-size:0}.desktop_hide,.desktop_hide table{display:table!important;max-height:none!important}}\r\n</style></head><body style=\"background-color:#fff;margin:0;padding:0;-webkit-text-size-adjust:none;text-size-adjust:none\"><table class=\"nl-container\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;background-color:#fff\"><tbody><tr><td><table class=\"row row-1\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0;background-color:#212529;background-size:auto\"><tbody><tr><td><table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;background-size:auto;color:#000;width:700px;margin:0 auto\" width=\"700\"><tbody><tr><td class=\"column column-1\" width=\"100%\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;padding-top:40px;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0\"><table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\" style=\"padding-bottom:10px;padding-left:10px;padding-right:10px;padding-top:30px\"><div style=\"font-family:sans-serif\"><div class \r\nstyle=\"font-size:12px;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;mso-line-height-alt:14.399999999999999px;color:#fff;line-height:1.2\"><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:30px;\"><strong>Medi<em>X</em></strong></span></p></div></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table><table class=\"row row-2\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0\"><tbody><tr><td><table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;color:#000;width:700px;margin:0 auto\" width=\"700\"><tbody><tr><td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0\"><table \r\nclass=\"paragraph_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\"><div style=\"color:#000;direction:ltr;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;font-size:14px;font-weight:400;letter-spacing:0;line-height:120%;text-align:left;mso-line-height-alt:16.8px\"><p style=\"margin:0;margin-bottom:16px\">&nbsp;</p>" +
                "<p style=\"margin:0;margin-bottom:16px\">Dear Medix user,\r\n</p><p style=\"margin:0;margin-bottom:16px\">" + body + "</p></div></td></tr></table>" +
                "<table class=\"paragraph_block block-3\" width=\"100%\" \r\nborder=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\"><div style=\"color:#000;direction:ltr;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;font-size:14px;font-weight:400;letter-spacing:0;line-height:120%;text-align:left;mso-line-height-alt:16.8px\"><p style=\"margin:0\">MediX<br>Wellington Rd,<br>Clayton VIC 3800</p></div></td></tr></table><table class=\"divider_block block-2\" width=\"100%\" border=\"0\" \r\ncellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0\"><tr><td class=\"pad\"><div class=\"alignment\" align=\"center\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\" style=\"mso-table-lspace:0;mso-table-rspace:0\"><tr><td class=\"divider_inner\" style=\"font-size:1px;line-height:1px;border-top:1px solid #bbb\"><span>&#8202;</span></td></tr></table></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table>\r\n<table class=\"row row-3\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0\"><tbody><tr><td><table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;color:#000;width:700px;margin:0 auto\" width=\"700\"><tbody><tr><td class=\"column column-1\" width=\"100%\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;padding-bottom:25px;padding-top:25px;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0\"><table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\"><div style=\"font-family:sans-serif\"><div class \r\nstyle=\"font-size:12px;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;mso-line-height-alt:14.399999999999999px;color:#555;line-height:1.2\"><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\"><strong>Our mailing address:</strong></span></p><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\">support@medix.com</span></p></div></div></td></tr></table><table \r\nclass=\"text_block block-2\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\" style=\"padding-bottom:20px;padding-left:10px;padding-right:10px;padding-top:20px\"><div style=\"font-family:sans-serif\"><div class style=\"font-size:12px;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;mso-line-height-alt:14.399999999999999px;color:#555;line-height:1.2\"><p \r\nstyle=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\"><strong>Want to change how you receive this email?</strong></span></p><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\"><a href=\"http://[updateprofile]/\" target=\"_blank\" rel=\"noopener\" style=\"color: #555555;\">manage preference</a> &nbsp; &nbsp;·&nbsp; &nbsp; <a href=\"http://[globalunsubscribe]/\" target=\"_blank\" rel=\"noopener\" style=\"color: #555555;\">unsubscribe</a></span></p></div></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>\r\n<!-- End --><div style=\"background-color:transparent;\">\r\n    <div style=\"Margin: 0 auto;min-width: 320px;max-width: 500px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;\" class=\"block-grid \">\r\n        <div style=\"border-collapse: collapse;display: table;width: 100%;background-color:transparent;\">\r\n            <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"background-color:transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width: 500px;\"><tr class=\"layout-full-width\" style=\"background-color:transparent;\"><![endif]-->\r\n            <!--[if (mso)|(IE)]><td align=\"center\" width=\"500\" style=\" width:500px; padding-right: 0px; padding-left: 0px; padding-top:15px; padding-bottom:15px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;\" valign=\"top\"><![endif]-->\r\n            <div class=\"col num12\" style=\"min-width: 320px;max-width: 500px;display: table-cell;vertical-align: top;\">\r\n                <div style=\"background-color: transparent; width: 100% !important;\">\r\n                    <!--[if (!mso)&(!IE)]><!--><div style=\"border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent; padding-top:15px; padding-bottom:15px; padding-right: 0px; padding-left: 0px;\">\r\n                        <!--<![endif]-->\r\n\r\n\r\n                        <div align=\"center\" class=\"img-container center  autowidth \" style=\"padding-right: 0px;  padding-left: 0px;\">\r\n                            <!--[if mso]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding-right: 0px; padding-left: 0px;\" align=\"center\"><![endif]-->\r\n\r\n                            <!--[if mso]></td></tr></table><![endif]-->\r\n                        </div>\r\n\r\n\r\n                        <!--[if (!mso)&(!IE)]><!-->\r\n                    </div><!--<![endif]-->\r\n                </div>\r\n            </div>\r\n            <!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->\r\n        </div>\r\n    </div>\r\n</div></body></html>";


                bool success = await SendEmail(toEmails, toNames, subject, plainTextContent, htmlContent, postedFile);

                if (success)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        public void SendLoginDetails(string toEmail, string toName, string password)
        {

            string plainTextContent = "Dear " + toName + ",\r\n\r\nHere are your login details to MediX. " +
                "Password provided is auto-generated and should be changed after logging in.\r\n\r\nEmail: " + toEmail + "\r\nPassword: " + password +
                "\r\n\r\nMediX\r\nWellington Rd,\r\nClayton VIC 3800";

            string htmlContent =
                "<!DOCTYPE html><html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" lang=\"en\">" +
                "   <head><title></title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><!--[if mso]><xml>" +
                "<o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]-->" +
                "<style>\r\n*{box-sizing:border-box}body{margin:0;padding:0}a[x-apple-data-detectors]" +
                "{color:inherit!important;text-decoration:inherit!important}#MessageViewBody a{color:inherit;text-decoration:none}p{line-height:inherit}.desktop_hide,.desktop_hide " +
                "table{mso-hide:all;display:none;max-height:0;overflow:hidden}.image_block img+div{display:none} " +
                "@media (max-width:720px){.mobile_hide{display:none}.row-content{width:100%!important}.stack .column{width:100%;display:block}.mobile_hide{min-height:0;max-height:0;max-width:0;overflow:hidden;font-size:0}.desktop_hide,.desktop_hide table{display:table!important;max-height:none!important}}\r\n</style></head><body style=\"background-color:#fff;margin:0;padding:0;-webkit-text-size-adjust:none;text-size-adjust:none\"><table class=\"nl-container\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;background-color:#fff\"><tbody><tr><td><table class=\"row row-1\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0;background-color:#212529;background-size:auto\"><tbody><tr><td><table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;background-size:auto;color:#000;width:700px;margin:0 auto\" width=\"700\"><tbody><tr><td class=\"column column-1\" width=\"100%\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;padding-top:40px;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0\"><table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\" style=\"padding-bottom:10px;padding-left:10px;padding-right:10px;padding-top:30px\"><div style=\"font-family:sans-serif\"><div class \r\nstyle=\"font-size:12px;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;mso-line-height-alt:14.399999999999999px;color:#fff;line-height:1.2\"><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:30px;\"><strong>Medi<em>X</em></strong></span></p></div></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table><table class=\"row row-2\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0\"><tbody><tr><td><table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;color:#000;width:700px;margin:0 auto\" width=\"700\"><tbody><tr><td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0\"><table \r\nclass=\"paragraph_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\"><div style=\"color:#000;direction:ltr;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;font-size:14px;font-weight:400;letter-spacing:0;line-height:120%;text-align:left;mso-line-height-alt:16.8px\"><p style=\"margin:0;margin-bottom:16px\">&nbsp;</p>" +
                "<p style=\"margin:0;margin-bottom:16px\">Dear " + toName + ",\r\n</p><p style=\"margin:0;margin-bottom:16px\">Here are your login details to <a href=\"https://localhost:44376/\" target=\"_blank\" style=\"text-decoration: underline; color: #0068a5;\" rel=\"noopener\">MediX</a>. Password provided is auto-generated and should be changed after logging in.</p>" +
                "<p style=\"margin:0\">Email: " + toEmail + "<br>Temporary Password: " + password + "</p></div></td></tr></table>" +
                "" +
                "<table class=\"paragraph_block block-3\" width=\"100%\" \r\nborder=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\"><div style=\"color:#000;direction:ltr;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;font-size:14px;font-weight:400;letter-spacing:0;line-height:120%;text-align:left;mso-line-height-alt:16.8px\"><p style=\"margin:0\">MediX<br>Wellington Rd,<br>Clayton VIC 3800</p></div></td></tr></table><table class=\"divider_block block-2\" width=\"100%\" border=\"0\" \r\ncellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0\"><tr><td class=\"pad\"><div class=\"alignment\" align=\"center\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\" style=\"mso-table-lspace:0;mso-table-rspace:0\"><tr><td class=\"divider_inner\" style=\"font-size:1px;line-height:1px;border-top:1px solid #bbb\"><span>&#8202;</span></td></tr></table></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table>\r\n<table class=\"row row-3\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0\"><tbody><tr><td><table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;color:#000;width:700px;margin:0 auto\" width=\"700\"><tbody><tr><td class=\"column column-1\" width=\"100%\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;padding-bottom:25px;padding-top:25px;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0\"><table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\"><div style=\"font-family:sans-serif\"><div class \r\nstyle=\"font-size:12px;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;mso-line-height-alt:14.399999999999999px;color:#555;line-height:1.2\"><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\"><strong>Our mailing address:</strong></span></p><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\">support@medix.com</span></p></div></div></td></tr></table><table \r\nclass=\"text_block block-2\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\" style=\"padding-bottom:20px;padding-left:10px;padding-right:10px;padding-top:20px\"><div style=\"font-family:sans-serif\"><div class style=\"font-size:12px;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;mso-line-height-alt:14.399999999999999px;color:#555;line-height:1.2\"><p \r\nstyle=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\"><strong>Want to change how you receive this email?</strong></span></p><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\"><a href=\"http://[updateprofile]/\" target=\"_blank\" rel=\"noopener\" style=\"color: #555555;\">manage preference</a> &nbsp; &nbsp;·&nbsp; &nbsp; <a href=\"http://[globalunsubscribe]/\" target=\"_blank\" rel=\"noopener\" style=\"color: #555555;\">unsubscribe</a></span></p></div></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>\r\n<!-- End --><div style=\"background-color:transparent;\">\r\n    <div style=\"Margin: 0 auto;min-width: 320px;max-width: 500px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;\" class=\"block-grid \">\r\n        <div style=\"border-collapse: collapse;display: table;width: 100%;background-color:transparent;\">\r\n            <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"background-color:transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width: 500px;\"><tr class=\"layout-full-width\" style=\"background-color:transparent;\"><![endif]-->\r\n            <!--[if (mso)|(IE)]><td align=\"center\" width=\"500\" style=\" width:500px; padding-right: 0px; padding-left: 0px; padding-top:15px; padding-bottom:15px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;\" valign=\"top\"><![endif]-->\r\n            <div class=\"col num12\" style=\"min-width: 320px;max-width: 500px;display: table-cell;vertical-align: top;\">\r\n                <div style=\"background-color: transparent; width: 100% !important;\">\r\n                    <!--[if (!mso)&(!IE)]><!--><div style=\"border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent; padding-top:15px; padding-bottom:15px; padding-right: 0px; padding-left: 0px;\">\r\n                        <!--<![endif]-->\r\n\r\n\r\n                        <div align=\"center\" class=\"img-container center  autowidth \" style=\"padding-right: 0px;  padding-left: 0px;\">\r\n                            <!--[if mso]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding-right: 0px; padding-left: 0px;\" align=\"center\"><![endif]-->\r\n\r\n                            <!--[if mso]></td></tr></table><![endif]-->\r\n                        </div>\r\n\r\n\r\n                        <!--[if (!mso)&(!IE)]><!-->\r\n                    </div><!--<![endif]-->\r\n                </div>\r\n            </div>\r\n            <!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->\r\n        </div>\r\n    </div>\r\n</div></body></html>";

            _ = SendEmail(new List<string> { toEmail }, new List<string> { toName }, "MediX Login Details", plainTextContent, htmlContent);
        }
        

        public void SendBookingDetails(Patient patient, Booking booking, MedicalCenter medicalCenter)
        {
            var toName = patient.FullName;
            var toEmail = patient.Email;

            string plainTextContent = "Dear " + toName + ",\r\n\r\nHere is your recently booked X-ray session." +
                "\r\n\r\nPatient Name: " + toName + 
                "\r\nDate and Time: " + booking.DateTime + 
                "\r\nMedical Center: " + medicalCenter.Name +
                "\r\nAddress: " + medicalCenter.Address + " (" + medicalCenter.Latitude + ", " + medicalCenter.Longitude + ")" +
                "\r\nNotes: " + booking.Notes + 
                "\r\n\r\nMediX\r\nWellington Rd,\r\nClayton VIC 3800";

            string htmlContent =
                "<!DOCTYPE html><html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" lang=\"en\">" +
                "   <head><title></title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><!--[if mso]><xml>" +
                "<o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]-->" +
                "<style>\r\n*{box-sizing:border-box}body{margin:0;padding:0}a[x-apple-data-detectors]" +
                "{color:inherit!important;text-decoration:inherit!important}#MessageViewBody a{color:inherit;text-decoration:none}p{line-height:inherit}.desktop_hide,.desktop_hide " +
                "table{mso-hide:all;display:none;max-height:0;overflow:hidden}.image_block img+div{display:none} " +
                "@media (max-width:720px){.mobile_hide{display:none}.row-content{width:100%!important}.stack .column{width:100%;display:block}.mobile_hide{min-height:0;max-height:0;max-width:0;overflow:hidden;font-size:0}.desktop_hide,.desktop_hide table{display:table!important;max-height:none!important}}\r\n</style></head><body style=\"background-color:#fff;margin:0;padding:0;-webkit-text-size-adjust:none;text-size-adjust:none\"><table class=\"nl-container\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;background-color:#fff\"><tbody><tr><td><table class=\"row row-1\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0;background-color:#212529;background-size:auto\"><tbody><tr><td><table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;background-size:auto;color:#000;width:700px;margin:0 auto\" width=\"700\"><tbody><tr><td class=\"column column-1\" width=\"100%\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;padding-top:40px;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0\"><table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\" style=\"padding-bottom:10px;padding-left:10px;padding-right:10px;padding-top:30px\"><div style=\"font-family:sans-serif\"><div class \r\nstyle=\"font-size:12px;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;mso-line-height-alt:14.399999999999999px;color:#fff;line-height:1.2\"><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:30px;\"><strong>Medi<em>X</em></strong></span></p></div></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table><table class=\"row row-2\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0\"><tbody><tr><td><table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;color:#000;width:700px;margin:0 auto\" width=\"700\"><tbody><tr><td class=\"column column-1\" width=\"100%\" style=\"mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0\"><table \r\nclass=\"paragraph_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\"><div style=\"color:#000;direction:ltr;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;font-size:14px;font-weight:400;letter-spacing:0;line-height:120%;text-align:left;mso-line-height-alt:16.8px\"><p style=\"margin:0;margin-bottom:16px\">&nbsp;</p>" +
                "<p style=\"margin:0;margin-bottom:16px\">Dear " + toName + ",\r\n</p><p style=\"margin:0;margin-bottom:16px\">Here is your recently booked X-ray session." +
                "<p style=\"margin:0\"Patient Name: " + toName + "<br>Date and Time: " + booking.DateTime + 
                "<br>Medical Center: " + booking.MedicalCenter.Name + "<br>Address: " + booking.MedicalCenter.Address + " (" + booking.MedicalCenter.Latitude + ", " + booking.MedicalCenter.Longitude + ")" + 
                "<br>Notes: " + booking.Notes +
                "</p></div></td></tr></table>" +
                "" +
                "<table class=\"paragraph_block block-3\" width=\"100%\" \r\nborder=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\"><div style=\"color:#000;direction:ltr;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;font-size:14px;font-weight:400;letter-spacing:0;line-height:120%;text-align:left;mso-line-height-alt:16.8px\"><p style=\"margin:0\">MediX<br>Wellington Rd,<br>Clayton VIC 3800</p></div></td></tr></table><table class=\"divider_block block-2\" width=\"100%\" border=\"0\" \r\ncellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0\"><tr><td class=\"pad\"><div class=\"alignment\" align=\"center\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\" style=\"mso-table-lspace:0;mso-table-rspace:0\"><tr><td class=\"divider_inner\" style=\"font-size:1px;line-height:1px;border-top:1px solid #bbb\"><span>&#8202;</span></td></tr></table></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table>\r\n<table class=\"row row-3\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0\"><tbody><tr><td><table class=\"row-content stack\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;color:#000;width:700px;margin:0 auto\" width=\"700\"><tbody><tr><td class=\"column column-1\" width=\"100%\" \r\nstyle=\"mso-table-lspace:0;mso-table-rspace:0;font-weight:400;text-align:left;padding-bottom:25px;padding-top:25px;vertical-align:top;border-top:0;border-right:0;border-bottom:0;border-left:0\"><table class=\"text_block block-1\" width=\"100%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\"><div style=\"font-family:sans-serif\"><div class \r\nstyle=\"font-size:12px;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;mso-line-height-alt:14.399999999999999px;color:#555;line-height:1.2\"><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\"><strong>Our mailing address:</strong></span></p><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\">support@medix.com</span></p></div></div></td></tr></table><table \r\nclass=\"text_block block-2\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"mso-table-lspace:0;mso-table-rspace:0;word-break:break-word\"><tr><td class=\"pad\" style=\"padding-bottom:20px;padding-left:10px;padding-right:10px;padding-top:20px\"><div style=\"font-family:sans-serif\"><div class style=\"font-size:12px;font-family:Arial,Helvetica Neue,Helvetica,sans-serif;mso-line-height-alt:14.399999999999999px;color:#555;line-height:1.2\"><p \r\nstyle=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\"><strong>Want to change how you receive this email?</strong></span></p><p style=\"margin:0;font-size:14px;text-align:center;mso-line-height-alt:16.8px\"><span style=\"font-size:12px;\"><a href=\"http://[updateprofile]/\" target=\"_blank\" rel=\"noopener\" style=\"color: #555555;\">manage preference</a> &nbsp; &nbsp;·&nbsp; &nbsp; <a href=\"http://[globalunsubscribe]/\" target=\"_blank\" rel=\"noopener\" style=\"color: #555555;\">unsubscribe</a></span></p></div></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>\r\n<!-- End --><div style=\"background-color:transparent;\">\r\n    <div style=\"Margin: 0 auto;min-width: 320px;max-width: 500px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;\" class=\"block-grid \">\r\n        <div style=\"border-collapse: collapse;display: table;width: 100%;background-color:transparent;\">\r\n            <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"background-color:transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width: 500px;\"><tr class=\"layout-full-width\" style=\"background-color:transparent;\"><![endif]-->\r\n            <!--[if (mso)|(IE)]><td align=\"center\" width=\"500\" style=\" width:500px; padding-right: 0px; padding-left: 0px; padding-top:15px; padding-bottom:15px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;\" valign=\"top\"><![endif]-->\r\n            <div class=\"col num12\" style=\"min-width: 320px;max-width: 500px;display: table-cell;vertical-align: top;\">\r\n                <div style=\"background-color: transparent; width: 100% !important;\">\r\n                    <!--[if (!mso)&(!IE)]><!--><div style=\"border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent; padding-top:15px; padding-bottom:15px; padding-right: 0px; padding-left: 0px;\">\r\n                        <!--<![endif]-->\r\n\r\n\r\n                        <div align=\"center\" class=\"img-container center  autowidth \" style=\"padding-right: 0px;  padding-left: 0px;\">\r\n                            <!--[if mso]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding-right: 0px; padding-left: 0px;\" align=\"center\"><![endif]-->\r\n\r\n                            <!--[if mso]></td></tr></table><![endif]-->\r\n                        </div>\r\n\r\n\r\n                        <!--[if (!mso)&(!IE)]><!-->\r\n                    </div><!--<![endif]-->\r\n                </div>\r\n            </div>\r\n            <!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->\r\n        </div>\r\n    </div>\r\n</div></body></html>";

            _ = SendEmail(new List<string> { toEmail }, new List<string> { toName }, "MediX X-Ray Session Booking", plainTextContent, htmlContent);

        }


        // Function for Send email
        public async Task<bool> SendEmail(List<string> toEmails, List<string> toNames, string subject, string plainTextContent, string htmlContent, HttpPostedFileBase attachment = null)
        {
            try
            {
                var from = new EmailAddress("vincentwijayaaaaa@gmail.com", "MediX");

                var toRecipients = new List<EmailAddress>();
                // Add recipients
                for (int i = 0; i < toEmails.Count; i++)
                {
                    toRecipients.Add(new EmailAddress(toEmails[i], toNames[i]));
                }
                var email = MailHelper.CreateSingleEmailToMultipleRecipients(from, toRecipients, subject, plainTextContent, htmlContent);

                if (attachment != null && attachment.ContentLength > 0)
                {
                    byte[] attachmentBytes = new byte[attachment.ContentLength];
                    attachment.InputStream.Read(attachmentBytes, 0, attachment.ContentLength);
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);
                    email.AddAttachment(new Attachment
                    {
                        Filename = attachment.FileName,
                        Content = attachmentBase64,
                        Type = attachment.ContentType
                    });
                }


                var response = await _sendGridClient.SendEmailAsync(email);

                if (response.IsSuccessStatusCode == true)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}