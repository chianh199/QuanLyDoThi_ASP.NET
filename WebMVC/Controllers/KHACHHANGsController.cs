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
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class KHACHHANGsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();
        
        // GET: api/KHACHHANGs
        public IEnumerable<KHACHHANG> Get()
        {
            TTTT3Entities1 db = new TTTT3Entities1();
            List<KHACHHANG> lkh = db.KHACHHANGs.ToList();
            List<TUYENTHU> tt = db.TUYENTHUs.ToList();
            List<QUANHUYEN> qh = db.QUANHUYENs.ToList();
            List<XAPHUONG> xp = db.XAPHUONGs.ToList();
            List<LOAIKH> llkh = db.LOAIKHs.ToList();
            for (int i = 0; i < lkh.Count; i++)
            {

                TUYENTHU ttt = tt.Find(s => s.IDTUYENTHU == lkh[i].IDTUYENTHU);
                LOAIKH lkhh = llkh.Find(s => s.IDLOAIKH == lkh[i].IDLOAIKH);
                lkh[i].LOAIKH = lkhh;
                lkh[i].LOAIKH.KHACHHANGs = null;
                lkh[i].TUYENTHU.IDTUYENTHU = ttt.IDTUYENTHU;
                lkh[i].TUYENTHU.TENTUYENTHU = ttt.TENTUYENTHU;
                lkh[i].TUYENTHU.MATUYENTHU = ttt.MATUYENTHU;
                lkh[i].TUYENTHU.KHACHHANGs = null;
                lkh[i].TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;

            }
            IEnumerable<KHACHHANG> lkh1 = lkh.OrderByDescending(kh => kh.IDKHACHHANG);
            return lkh1;
        }
        // GET: api/KHACHHANGs/5
        [ResponseType(typeof(KHACHHANG))]
        public async Task<IHttpActionResult> GetKHACHHANG(int id)
        {
            KHACHHANG kHACHHANG = await db.KHACHHANGs.FindAsync(id);
            if (kHACHHANG == null)
            {
                return NotFound();
            }
          
            LOAIKH lkh = db.LOAIKHs.FirstOrDefault(x => x.IDLOAIKH == kHACHHANG.IDLOAIKH);
            kHACHHANG.LOAIKH = lkh;
            TUYENTHU tt = db.TUYENTHUs.FirstOrDefault(x => x.IDTUYENTHU == kHACHHANG.IDTUYENTHU);
            kHACHHANG.TUYENTHU = tt;
            XAPHUONG qh = db.XAPHUONGs.FirstOrDefault(x => x.IDXAPHUONG == tt.IDXAPHUONG);
            tt.XAPHUONG = qh;
            QUANHUYEN rqh = db.QUANHUYENs.FirstOrDefault(x => x.IDQUANHUYEN == qh.IDQUANHUYEN);
            tt.XAPHUONG.QUANHUYEN = rqh;
            
            return Ok(kHACHHANG);
        }

        [Route("kh_tuyenthu/{idtuyen}")]
        public IEnumerable<KHACHHANG> GetKhachhangtheotuyen(int idtuyen)
        {
            
            List<KHACHHANG> lkh = db.KHACHHANGs.ToList().FindAll(c => c.IDTUYENTHU == idtuyen);
            List<TUYENTHU> tt = db.TUYENTHUs.ToList();
            List<QUANHUYEN> qh = db.QUANHUYENs.ToList();
            List<XAPHUONG> xp = db.XAPHUONGs.ToList();
            List<LOAIKH> llkh = db.LOAIKHs.ToList();
            for (int i = 0; i < lkh.Count; i++)
            {

                TUYENTHU ttt = tt.Find(s => s.IDTUYENTHU == lkh[i].IDTUYENTHU);
                LOAIKH lkhh = llkh.Find(s => s.IDLOAIKH == lkh[i].IDLOAIKH);
                lkh[i].LOAIKH = lkhh;
                lkh[i].LOAIKH.KHACHHANGs = null;
                lkh[i].TUYENTHU.IDTUYENTHU = ttt.IDTUYENTHU;
                lkh[i].TUYENTHU.TENTUYENTHU = ttt.TENTUYENTHU;
                lkh[i].TUYENTHU.MATUYENTHU = ttt.MATUYENTHU;
                lkh[i].TUYENTHU.KHACHHANGs = null;
                lkh[i].TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;

            }

            IEnumerable<KHACHHANG> lkh1 = lkh.OrderByDescending(kh => kh.IDKHACHHANG);
            return lkh1;
        }

        // PUT: api/KHACHHANGs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutKHACHHANG(int id, KHACHHANG kHACHHANG)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kHACHHANG.IDKHACHHANG)
            {
                return BadRequest();
            }

            if (!ktrachuoi(kHACHHANG.MAKHACHHANG))
            {
                ModelState.AddModelError("inputcheck", "Mã khách hàng không được có dấu và khoảng cách!");
                return BadRequest(ModelState);
            }
            var dem = db.KHACHHANGs.Count(e => e.MAKHACHHANG.Equals(kHACHHANG.MAKHACHHANG) && e.IDKHACHHANG != id);
            if (dem > 0)
            {
                ModelState.AddModelError("makh", "Mã khách hàng đã tồn tại!");
                return BadRequest(ModelState);
            }

            db.Entry(kHACHHANG).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KHACHHANGExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
       
        // POST: api/KHACHHANGs
        [ResponseType(typeof(KHACHHANG))]
        public async Task<IHttpActionResult> PostKHACHHANG(KHACHHANG kHACHHANG)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!ktrachuoi(kHACHHANG.MAKHACHHANG))
            {
                ModelState.AddModelError("inputcheck", "Mã khách hàng không được có dấu và khoảng cách!");
                return BadRequest(ModelState);
            }
            var dem = db.KHACHHANGs.Count(e => e.MAKHACHHANG.Equals(kHACHHANG.MAKHACHHANG));
            if (dem > 0)
            {
                ModelState.AddModelError("makh", "Mã khách hàng đã tồn tại!");
                return BadRequest(ModelState);
            }

            List<TUYENTHU> ltt = db.TUYENTHUs.ToList();
            kHACHHANG.IDXAPHUONG = ltt.Find(c => c.IDTUYENTHU == kHACHHANG.IDTUYENTHU).IDXAPHUONG;
            kHACHHANG.TRANGTHAI = true;
            db.KHACHHANGs.Add(kHACHHANG);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = kHACHHANG.IDKHACHHANG }, kHACHHANG);
        }

        public bool ktrachuoi(string username)
        {
            bool flag = true;
            foreach (char a in username)
            {
                int asciiValue = (int)a;
                if ((asciiValue >= 38 && asciiValue <= 57) || (asciiValue >= 65 && asciiValue <= 90) || (asciiValue >= 97 && asciiValue <= 122))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                    break;
                }
            }
            if (flag == true)
                return true;
            else
                return false;
        }

        // DELETE: api/KHACHHANGs
        //[ResponseType(typeof(KHACHHANG))]
        public async Task<IHttpActionResult> DeleteKHACHHANG(int id)
        {


            KHACHHANG kHACHHANG = await db.KHACHHANGs.FindAsync(id);
            if (kHACHHANG != null)
            {
                db.KHACHHANGs.Remove(kHACHHANG);
                await db.SaveChangesAsync();
            }

            return Ok("Đã xóa thành công");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KHACHHANGExists(int id)
        {
            return db.KHACHHANGs.Count(e => e.IDKHACHHANG == id) > 0;
        }
    }
}