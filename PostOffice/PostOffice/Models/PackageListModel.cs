using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostOffice.Models
{
    public class PackageListModel
    {
        public int KEY { get; set; }
        public string SENDER { get; set; }
        public string RECIPIENT { get; set; }
        public string SADDRES { get; set; }
        public Nullable<int> SINDEX { get; set; }
        public string IDPACKAGE { get; set; }
        public Nullable<decimal> PRICE { get; set; }

        public string ADDRES { get; set; }
        public Nullable<int> INDEX { get; set; }
    }
}