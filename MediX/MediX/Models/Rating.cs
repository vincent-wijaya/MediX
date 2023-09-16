namespace MediX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rating
    {
        public int Id { get; set; }

        public short Value { get; set; }

        [Required]
        [StringLength(512)]
        public string Comment { get; set; }

        public int MedicalCenterId { get; set; }

        public int PatientId { get; set; }

        public int Booking_Id { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual MedicalCenter MedicalCenter { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
