using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
        
        [Display(Name = "Career Level")]
        public string CareerLevel { get; set; }

        [Display(Name = "Vacancy")]
        public int Vacancy { get; set; }

        [Display(Name = "Needed Experience")]
        public int NeededExperience { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "Salary")]
        public int Salary { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool JobState { get; set; }

        public virtual City City { get; set; }
        public int CityId { get; set; }

        public virtual Country Country { get; set; }
        public int CountryId { get; set; }

        public virtual JobType JobType { get; set; }
        public int JobTypeId { get; set; }

        public int JobCategoryId { get; set; }
        public virtual JobCategory JobCategory { get; set; }

        public int RegisterUserId { get; set; }
        public virtual RegisterUser RegisterUser { get; set; }

    }
}