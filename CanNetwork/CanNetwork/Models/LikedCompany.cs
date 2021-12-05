using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class LikedCompany
    {
        public int Id { get; set; }

        public DateTime LikedCompanyDate { get; set; }

        public int SeekerRegistrationId { get; set; }
        public virtual SeekerRegistration SeekerRegistration { get; set; }
        public int RegisterUserId { get; set; }
        public virtual RegisterUser RegisterUser { get; set; }
    }
}