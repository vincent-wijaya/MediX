using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.UI.WebControls;

namespace MediX.Models
{

    public class MedicalCenterViewModel
    {
        [DisplayName("Rating")]
        public double AverageRating { get; set; }

        public int RatingsCount { get; set; }

        [DisplayName("Medical Center")]
        public MedicalCenter MedicalCenter { get; set; }
    }
}
