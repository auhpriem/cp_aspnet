using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PostOffice.Models
{
    public class PackageStatusModel
    {

        [Required]
        [Display(Name = "Package Key")]
        public int PackageKey { get; set; }

        [Required]
        [Display(Name = "Address"), StringLength(200)]
        public string Address { get; set; }

        public string ValidationMessage { get; set; }
    }
}