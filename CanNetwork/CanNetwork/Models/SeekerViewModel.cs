using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class SeekerViewModel
    {
        public IEnumerable<Job> Jobs { get; set; }
        public IEnumerable<ApplyedJob> ApplyedJobs { get; set; }
        public IEnumerable<LikedJobs> LikedJobs { get; set; }
        public IEnumerable<LikedCompany> LikedCompanies { get; set; }
    }
}