using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class JobType
    {
        public int Id { get; set; }
        public string JobTypeName { get; set; }
        public IEnumerable<Job> Jobs { get; set; }

    }
}