//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class XAPHUONG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public XAPHUONG()
        {
            this.TUYENTHUs = new HashSet<TUYENTHU>();
        }
    
        public int IDXAPHUONG { get; set; }
        public int IDQUANHUYEN { get; set; }
        public string TENXAPHUONG { get; set; }
    
        public virtual QUANHUYEN QUANHUYEN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TUYENTHU> TUYENTHUs { get; set; }
    }
}
