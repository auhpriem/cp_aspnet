using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PostOffice.Models
{
    public class EditionTypeModel
    {
        [Required]
        [StringLength(maximumLength:5)]
        public string EditionID { get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string Edition { get; set; }
        [Required]
        [Range(0.01, 99.99)]
        public decimal CostForMonth { get; set; }
    }
    public class PackageTypeModel
    {
        [Required]
        [StringLength(maximumLength: 20)]
        public string PackageID { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string PackageDescription { get; set; }
        [Required]
        [Range(0.01, 99.99)]
        public decimal? Cost { get; set; }
    }
    public class EditionPackageTypeModel
    {
        public string ValidationMessage_Edition { get; set; }
        public string ValidationMessage_Package { get; set; }

        public EditionTypeModel editionType = new EditionTypeModel();
        public PackageTypeModel packageType = new PackageTypeModel();
    }
}