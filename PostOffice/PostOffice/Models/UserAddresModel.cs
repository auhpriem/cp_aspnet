using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PostOffice.Models
{
    public class UserAddresModel
    {
        [Display(Name = "Output Address")]
        public string Address1 { get; set; }
        [Display(Name = "Input Address")]
        public string Address2 { get; set; }
    }
}