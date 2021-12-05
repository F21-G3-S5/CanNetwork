using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        
        public IEnumerable<City> Cities { get; set; }

        public IEnumerable<SeekerRegistration> SeekerRegistrations { get; set; }
        public IEnumerable<Job> Jobs { get; set; }

    }
}