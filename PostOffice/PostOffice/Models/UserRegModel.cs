using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PostOffice.Models
{
    public class UserRegModel
    {
        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Password ")]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[\W]).{6,20})", ErrorMessage = "Incorrect Password ")]
        public string UserPassword { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Admin Code")]
        public string AdminCode { get; set; }
    }
}