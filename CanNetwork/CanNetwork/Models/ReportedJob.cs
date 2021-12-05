using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class ReportedJob
    {
        public int Id { get; set; }

        public DateTime ReportedDate { get; set; }

        public int JobId { get; set; }
        public virtual Job Job { get; set; }

        public int SeekerRegistrationId { get; set; }
        public virtual SeekerRegistration SeekerRegistration { get; set; }
    }
}