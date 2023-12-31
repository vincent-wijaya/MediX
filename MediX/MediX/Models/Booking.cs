//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MediX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            this.Ratings = new HashSet<Rating>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Booking date and time are required")]
        [DisplayName("Session")]
        public System.DateTime DateTime { get; set; }
        [Required(ErrorMessage = "Information regarding the X-ray imaging is required.")]
        public string Notes { get; set; }
        [DisplayName("Status")]
        public bool IsCompleted { get; set; }
        [DisplayName("Created on")]
        public System.DateTime DateCreated { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Patient is required.")]
        public int PatientId { get; set; }
        public int StaffId { get; set; }
        [Required(ErrorMessage = "Medical center is required.")]
        public int MedicalCenterId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual Patient Patient { get; set; }
        [DisplayName("Booked by")]
        public virtual Staff Staff { get; set; }
        [DisplayName("Medical Center")]
        public virtual MedicalCenter MedicalCenter { get; set; }
    }
}