//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PostOffice.App_Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class SentPackage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SentPackage()
        {
            this.StatusPack = new HashSet<StatusPack>();
        }
    
        public int KEY { get; set; }
        public string SENDER { get; set; }
        public string RECIPIENT { get; set; }
        public string SADDRES { get; set; }
        public Nullable<int> SINDEX { get; set; }
        public string IDPACKAGE { get; set; }
        public Nullable<decimal> PRICE { get; set; }
    
        public virtual Clients Clients { get; set; }
        public virtual Clients Clients1 { get; set; }
        public virtual INDEX INDEX { get; set; }
        public virtual Package Package { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StatusPack> StatusPack { get; set; }
    }
}