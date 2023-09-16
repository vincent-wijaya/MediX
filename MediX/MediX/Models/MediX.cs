using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MediX.Models
{
    public partial class MediX : DbContext
    {
        public MediX()
            : base("name=MediX")
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<MedicalCenter> MedicalCenters { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<XRayRoom> XRayRooms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasMany(e => e.Ratings)
                .WithRequired(e => e.Booking)
                .HasForeignKey(e => e.Booking_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MedicalCenter>()
                .HasMany(e => e.Ratings)
                .WithRequired(e => e.MedicalCenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MedicalCenter>()
                .HasMany(e => e.Staffs)
                .WithRequired(e => e.MedicalCenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MedicalCenter>()
                .HasMany(e => e.XRayRooms)
                .WithRequired(e => e.MedicalCenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.Bookings)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.Ratings)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Staff>()
                .HasMany(e => e.Bookings)
                .WithRequired(e => e.Staff)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<XRayRoom>()
                .HasMany(e => e.Bookings)
                .WithRequired(e => e.XRayRoom)
                .WillCascadeOnDelete(false);
        }
    }
}
