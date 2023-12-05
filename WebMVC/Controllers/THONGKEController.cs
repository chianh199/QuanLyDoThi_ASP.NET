using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI;
using System.Web.WebPages;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class THONGKEController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/THONGKE
        public List<TKNGAY> GetPHIEUTHUs(NGAYPOST tKNGAY)
        {

            DateTime date1 = new DateTime(tKNGAY.ng1.GetValueOrDefault().Year, tKNGAY.ng1.GetValueOrDefault().Month, tKNGAY.ng1.GetValueOrDefault().Day, 0, 0, 0);
            DateTime date2 = new DateTime(tKNGAY.ng2.GetValueOrDefault().Year, tKNGAY.ng2.GetValueOrDefault().Month, tKNGAY.ng2.GetValueOrDefault().Day, 0, 0, 0);
            List<PHIEUTHU> pts = db.PHIEUTHUs.ToList();
            List<PHIEUTHU> pts1 = new List<PHIEUTHU>();
            foreach (PHIEUTHU pt in pts)
            {
                DateTime date3 = new DateTime(pt.NGAYCAPNHAT.GetValueOrDefault().Year, pt.NGAYCAPNHAT.GetValueOrDefault().Month, pt.NGAYCAPNHAT.GetValueOrDefault().Day, 0, 0, 0);
                if (DateTime.Compare(date1, date3) <= 0 && DateTime.Compare(date3, date2) <= 0 && pt.TRANGTHAIPHIEU == true)
                {
                    pts1.Add(pt);
                }
            }
            var kt_pt = (from p in pts1
                         from k in db.KYTHUs
                         where p.IDKYTHU == k.IDKYTHU
                         from h in db.KHACHHANGs
                         where h.IDKHACHHANG == p.IDKHACHHANG
                         from ct in db.CHITIETPHIEUTHUs
                         where ct.IDPHIEU == p.IDPHIEU
                         group p by new { k.IDKYTHU, h.IDTUYENTHU, k.TENKYTHU}
                         into grp
                         from t in db.TUYENTHUs
                         where t.IDTUYENTHU == grp.Key.IDTUYENTHU
                         from x in db.XAPHUONGs
                         where x.IDXAPHUONG == t.IDXAPHUONG
                         from q in db.QUANHUYENs
                         where q.IDQUANHUYEN == x.IDQUANHUYEN
                         select new TKNGAY
                         {
                             tenkythu = grp.Key.TENKYTHU,
                             quanhuyen = q.TENQUANHUYEN,
                             xaphuong = x.TENXAPHUONG,
                             tuyenthu = t.TENTUYENTHU,
                             soluong = grp.Count(),
                         }).ToList();


            return kt_pt;
        }
    }
}