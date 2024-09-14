using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVC.Models
{
    public class ModelLogin
    {
        public string token { get; set; }
        public int IDNHANVIEN { get; set; }
        public string MANHANVIEN { get; set; }
        public string HOTEN { get; set; }
        public Nullable<decimal> SDT { get; set; }
        public Nullable<System.DateTime> NGAYSINH { get; set; }
        public string DIACHI { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public List<CHITIETPHANQUYEN> CHITIETPHANQUYENs { get; set; }
    }
}