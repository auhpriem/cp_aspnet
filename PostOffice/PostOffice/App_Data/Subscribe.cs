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
    
    public partial class Subscribe
    {
        public int KEY { get; set; }
        public string IDEDITION { get; set; }
        public string IDCLIENT { get; set; }
        public int PERIOD { get; set; }
        public Nullable<System.DateTime> DATEACTIVATION { get; set; }
    
        public virtual Clients Clients { get; set; }
        public virtual Edition Edition { get; set; }
    }
}