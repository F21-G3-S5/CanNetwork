using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class EducationLevel
    {
        public int Id { get; set; }
        public string EducationLevelName { get; set; }
       
        public IEnumerable<Job> Jobs { get; set; }
    }
}