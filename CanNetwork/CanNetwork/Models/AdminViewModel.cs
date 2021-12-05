using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class AdminViewModel
    {
        public IEnumerable<RegisterUser> RegisterUser  { get; set; }
        public IEnumerable<SeekerRegistration> SeekerRegistration { get; set; }
        
        public IEnumerable<Job> Jobs { get; set; }
        public IEnumerable<ReportedJob> ReportedJobs { get; set; }
        
        public IEnumerable<Blog> Blogs { get; set; }
        
        public IEnumerable<Contact> Contacts { get; set; }

        public IEnumerable<City> City { get; set; }
        public IEnumerable<JobCategory> JobCategory { get; set; }
        public IEnumerable<Language> Languages { get; set; }
        public IEnumerable<EducationLevel> EducationLevels { get; set; }
        public IEnumerable<University> Universities { get; set; }
        public IEnumerable<JobTitle> JobTitles { get; set; }
        public IEnumerable<JobType> JobTypes { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<Nationality> Nationalities { get; set; }
    }
}