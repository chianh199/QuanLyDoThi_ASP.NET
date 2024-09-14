using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVC.Models
{
    public class NHANVIENDTO
    {
        public int IDNHANVIEN { get; set; }
        public string MANHANVIEN { get; set; }
        public string HOTEN { get; set; }
        public Nullable<decimal> SDT { get; set; }
        public List<CHITIETPHANQUYENDTO> CHITIETPHANQUYENs { get; set; }
    }
}