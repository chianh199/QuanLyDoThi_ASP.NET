using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMVC.Models;

namespace WebMVC.Models
{
    public class ThongKe
    {

        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int idQuan { get; set; }
        public int idXa { get; set; }
        public int idtuyen { get; set; }
        public string tenkythu { get; set; }
        public bool trangthai { get; set; }

    }
    public class soluong
    {
        public String ngay { get; set; }
        public int tongtien { get; set; }
        public long soluongthu { get; set; }
        public List<PHIEUTHU> dsphieuthu { get; set; }
    }
    public class thongkeAll
    {
        public String tenkythu { get; set; }
        public String tentuyenthu { get; set; }
        public String nhanvienthu { get; set; }
        public int idnhanvien { get; set; }
        public int soluongtong { get; set; }
        public int soluongdathu { get; set; }
        public int soluongchuathu { get; set; }
        public int soluongphieuhuy { get; set; }
        public int? tongtien { get; set; }
        public float? phantramdathu { get; set; }
        public float? phantramchuathu { get; set; }

        public List<PHANQUYENTUYENTHU> lpq {get; set;}
    }
}