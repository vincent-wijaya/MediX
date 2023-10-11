using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.UI.WebControls;

namespace MediX.Models
{

    public class SendEmailViewModel
    {
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Emails")]
        public string Emails { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Body")]
        public string Body { get; set; }

        public HttpPostedFileBase Attachment { get; set; }

        [Display(Name = "Attachment File Name")]
        public string FilePath { get; set; }

        [Display(Name = "Attachment")]
        public string FileName { get; set; }
    }
}
