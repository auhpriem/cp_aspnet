using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PostOffice.Models
{
    public class SubscriptionModel
    {
        [Required]
        [Display(Name = "User (Sender)"), StringLength(20)]
        public string s_iduser { get; set; }

        [Required]
        [Display(Name = "Edition"), StringLength(20)]
        public string Edition { get; set; }

        [Required]
        [Range(1,99)]
        [Display(Name = "Count of months")]
        public int Period { get; set; }
    }
    public class SendToUserModel
    {
        [Required]
        [Display(Name = "User (Sender)"), StringLength(20)]
        public string s_iduser { get; set; }

        [Required]
        [Display(Name = "User (Recipient)"), StringLength(20)]
        public string r_iduser { get; set; }

        [Required]
        [Display(Name = "Delivery Address"), StringLength(220)]
        public string DeliveryAddress { get; set; }

        [Required, Display(Name = "Sending Type"), StringLength(20)]
        public string SendingType { get; set; }

        [Required]
        [Display(Name = "Sending Cost"), Range(0, 99999.99)]
        public double SendingCost { get; set; }
    }
    public class SendToClientModel
    {
        [Required]
        [Display(Name = "User (Sender)"), StringLength(20)]
        public string s_iduser { get; set; }

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

        [Required]
        [Display(Name = "Delivery Address"), StringLength(200)]
        public string DeliveryAddress { get; set; }

        [Required, Display(Name = "Sending Type"), StringLength(20)]
        public string SendingType { get; set; }

        [Required]
        [Display(Name = "Sending Cost"), Range(0, 99999.99)]
        public double SendingCost { get; set; }
    }
    public class UserOperationModel
    {
        public SubscriptionModel subscription;
        public SendToUserModel sendToUser;
        public SendToClientModel sendToClient;

        public string ValidationMessage { get; set; }

        public UserOperationModel()
        {
            subscription = new SubscriptionModel();
            sendToUser = new SendToUserModel();
            sendToClient = new SendToClientModel();
        }
    }
}