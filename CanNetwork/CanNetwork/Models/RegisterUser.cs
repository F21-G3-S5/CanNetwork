using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class RegisterUser
    {
        public int Id { get; set; }

        public string ProviderImage { get; set; }

        [Display(Name = "User Name")]
        public string Name { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Field")]
        public string CompanyField { get; set; }

        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }

        public virtual JobTitle JobTitle { get; set; }
        public int JobTitleId { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
        
        [Display(Name = "What Is Your Favourite Team?")]
        public string SecurityQuestion { get; set; }

    }
}