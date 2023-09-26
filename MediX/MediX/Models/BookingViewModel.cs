using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class DateTimeIn30MinuteIntervalsAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime dateTime)
        {
            // Check if the minute component is a multiple of 30
            return dateTime.Minute % 30 == 0;
        }

        // If the value is not a DateTime, consider it valid
        return true;
    }
}

namespace MediX.Models
{
    public class CreateBookingViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Booking date and time is required.")]
        [DateTimeIn30MinuteIntervals(ErrorMessage = "Booking time has to be in intervals of 30 minutes.")]
        public System.DateTime DateTime { get; set; }
        [Required(ErrorMessage = "Notes are required.")]
        public string Notes { get; set; }
        [Required(ErrorMessage = "Patient is required.")]
        public int PatientName { get; set; }
        [Required(ErrorMessage = "Booker staff is required.")]
        public int StaffName { get; set; }
        [Required(ErrorMessage = "Please select a medical center.")]
        public int MedicalCenterName { get; set; }
        public int XRayRoomRoomNumber { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Staff BookerStaff { get; set; }
        public virtual Rating Rating { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please confirm your email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Confirm email address")]
        [Compare("Email", ErrorMessage = "The email address and confirmation email address do not match.")]
        public string ConfirmEmail { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public System.DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is rquired.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}
