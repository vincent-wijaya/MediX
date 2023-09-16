namespace MediX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            Ratings = new HashSet<Rating>();
        }

        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(512)]
        public string Notes { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime DateCreated { get; set; }

        public int PatientId { get; set; }

        public int StaffId { get; set; }

        public int XRayRoomId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual Staff Staff { get; set; }

        public virtual XRayRoom XRayRoom { get; set; }
    }
}
