using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class SeekerRegistration
    {
        public int Id { get; set; }

        /* Personal Info */

        [Display(Name ="Personal Photo")]
        public string SeekerImage { get; set; }

        [Display(Name = "Seeker Name")]
        public string Name { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [Display(Name="Gender")]
        public string Gender { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string SearchOnJobTitle { get; set; }

        public bool IsActive { get; set; }

        public virtual City City { get; set; }
        public int CityId { get; set; }

        public virtual Country Country { get; set; }
        public int CountryId { get; set; }

        public virtual Nationality Nationality { get; set; }
        public int NationalityId { get; set; }

        /* Education */
        public DateTime GraduationDate { get; set; }

        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }

        public int UniversityId { get; set; }
        public virtual University University { get; set; }

        public virtual EducationLevel EducationLevel { get; set; }
        public int EducationLevelId { get; set; }

        [Display(Name = "What Is Your Favourite Team?")]
        public string SecurityQuestion { get; set; }

        [Display(Name = "CV")]
        public string SeekerCV { get; set; }

        /* Previous Work */
        public string CompanyName { get; set; }
        public int Salary { get; set; }
        public int Duration { get; set; }

        public virtual JobTitle JobTitle { get; set; }
        public int JobTitleId { get; set; }

        public virtual JobType JobType { get; set; }
        public int JobTypeId { get; set; }

        public virtual JobCategory JobCategory { get; set; }
        public int JobCategoryId { get; set; }

        /* Current Work */
        public string CurrentCompanyName { get; set; }
        public int CurrentSalary { get; set; }
        public int CurrentDuration { get; set; }

        [Display(Name = "Job Title")]
        public string CurrentJobTitle { get; set; }

        [Display(Name = "Job Type")]
        public string CurrentJobType { get; set; }

        [Display(Name = "Job Category")]
        public string CurrentJobCategory { get; set; }
        
    }
}