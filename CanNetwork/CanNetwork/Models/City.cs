using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }

        public virtual Country Country { get; set; }
        public int CountryId { get; set; }

        public IEnumerable<RegisterUser> RegisterUsers { get; set; }
        public IEnumerable<SeekerRegistration> SeekerRegistrations { get; set; }
        public IEnumerable<Job> Jobs { get; set; }

    }
}