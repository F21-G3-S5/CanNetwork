using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CanNetwork.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "User Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "Please Write E-mail in Right Format!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}