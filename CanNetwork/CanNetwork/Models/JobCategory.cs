using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class JobCategory
    {
        public int Id { get; set; }
        public string JobCategoryName { get; set; }

        public IEnumerable<SeekerRegistration> SeekerRegistrations { get; set; }
        public IEnumerable<RegisterUser> RegisterUsers { get; set; }
        public IEnumerable<Job> Jobs { get; set; }
    }
}