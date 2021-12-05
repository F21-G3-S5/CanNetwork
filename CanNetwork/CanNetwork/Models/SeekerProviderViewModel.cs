using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class SeekerProviderViewModel
    {
        public IEnumerable<SeekerRegistration> SeekerRegistrations { get; set; }
        public IEnumerable<RegisterUser> RegisterUsers { get; set; }
    }
}