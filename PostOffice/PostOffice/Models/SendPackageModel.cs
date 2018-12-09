using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PostOffice.Models
{
    public class SendPackageModel
    {
        //Sender Fields
        [Required]
        [Display(Name = "Surname"), StringLength(34)]
        public string s_Surname { get; set; }

        [Required]
        [Display(Name = "Name"), StringLength(34)]
        public string s_Name { get; set; }

        [Display(Name = "Middle Name"), StringLength(30)]
        public string s_MiddleName { get; set; }

        [Required]
        [Display(Name = "Address"), StringLength(220)]
        public string s_Address { get; set; }

        //Recipient Fields
        [Required]
        [Display(Name = "Surname"), StringLength(34)]
        public string r_Surname { get; set; }

        [Required]
        [Display(Name = "Name"), StringLength(34)]
        public string r_Name { get; set; }

        [Display(Name = "Middle Name"), StringLength(30)]
        public string r_MiddleName { get; set; }

        [Required]
        [Display(Name = "Address"), StringLength(200)]
        public string r_Address { get; set; }

        //Package Information
        [Required]
        [Display(Name = "Delivery Address"), StringLength(200)]
        public string DeliveryAddress { get; set; }

        [Required, Display(Name = "Sending Type"), StringLength(20)]
        public string SendingType { get; set; }

        [Required]
        [Display(Name = "Sending Cost"), Range(0,99999.99)]
        public double SendingCost { get; set; }

        
        public string ValidationMessage { get; set; }

    }
}