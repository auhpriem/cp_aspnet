using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostOffice.Models
{
    public class StoryStatusHistoryModel
    {
        public string SENDER { get; set; }
        public string PACKAGEDESCRIPTION { get; set; }
        public decimal? FIN_COST { get; set; }
        public string ADDRES { get; set; }
        public int? INDEX { get; set; }
    }
}