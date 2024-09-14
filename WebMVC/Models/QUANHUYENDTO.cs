using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVC.Models
{
    public class QUANHUYENDTO
    {
        public int IDQUANHUYEN { get; set; }
        public string TENQUANHUYEN { get; set; }
        public virtual ICollection<XAPHUONG> XAPHUONGs { get; set; }

    }
}