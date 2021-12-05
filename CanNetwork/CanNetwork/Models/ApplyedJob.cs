using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class ApplyedJob
    {
        public int Id { get; set; }
        
        public DateTime ApplyDate { get; set; }
        
        public int JobId { get; set; }
        public virtual Job Job { get; set; }

        public int SeekerRegistrationId { get; set; }
        public virtual SeekerRegistration SeekerRegistration { get; set; }
    }
}