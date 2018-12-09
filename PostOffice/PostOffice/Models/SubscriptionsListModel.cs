using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PostOffice.Models
{
    public class SubscriptionsListModel
    {
        public string               Edition              { get; set; }
        public string               Client              { get; set; }
        public System.DateTime?     ActivationDate           { get; set; }
        public decimal              FinalPrice              { get; set; }
    }
}