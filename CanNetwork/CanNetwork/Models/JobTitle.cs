using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class JobTitle
    {
        public int Id { get; set; }
        public string JobTitleName { get; set; }
        public IEnumerable<RegisterUser> RegisterUsers { get; set; }
    }
}