using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostOffice.Models
{
    public class RecievedPackageModel
    {
        public int KEY { get; set; }
        public string SENDER { get; set; }
        public string RECIPIENT { get; set; }
        public string SADDRES { get; set; }
        public int    SINDEX { get; set; }
        public string IDPACKAGE { get; set; }
        public decimal FINALCOST { get; set; }
    }
}