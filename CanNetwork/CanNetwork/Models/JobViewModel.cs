using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class JobViewModel
    {
        public string JobTitle { get; set; }
        public IEnumerable<ApplyedJob> ApplyedJobs { get; set; }
    }
}