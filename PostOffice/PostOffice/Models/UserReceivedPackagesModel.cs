using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostOffice.Models
{
    public class UserReceivedPackagesModel
    {
        public string PACKAGEDESCRIPTION { get; set; }
        public string SENDER { get; set; }
        public string RECIPIENT { get; set; }
        public decimal FINALCOST { get; set; }
    }
}