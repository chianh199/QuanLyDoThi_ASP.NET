using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class PHIEUTHUsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/PHIEUTHUs
        public IEnumerable<PHIEUTHU> GetPHIEUTHUs()
        {
            List<PHIEUTHU> pt = db.PHIEUTHUs.ToList();
            foreach(PHIEUTHU pt1 in pt)
            {
                KHACHHANG kh = db.KHACHHANGs.Where(x => x.IDKHACHHANG == pt1.IDKHACHHANG).FirstOrDefault();
                KYTHU kt = db.KYTHUs.Where(x => x.IDKYTHU == pt1.IDKYTHU).FirstOrDefault();
                NHANVIEN nv = db.NHANVIENs.Where(x => x.IDNHANVIEN == pt1.IDNHANVIEN).FirstOrDefault();
                List<CHITIETPHIEUTHU> ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt1.IDPHIEU).ToList();
                pt1.KHACHHANG = kh;
                pt1.KYTHU = kt;
                pt1.NHANVIEN = nv;
                pt1.KYTHU.PHIEUTHUs = null;
                pt1.CHITIETPHIEUTHUs = ct;
                pt1.NHANVIEN.PHIEUTHUs = null;
                pt1.KHACHHANG.PHIEUTHUs = null;
                LOAIKH lkh = db.LOAIKHs.Where(x => x.IDLOAIKH == kh.IDLOAIKH).FirstOrDefault();
                pt1.KHACHHANG.LOAIKH = lkh;
                pt1.KHACHHANG.LOAIKH.KHACHHANGs = null;
                TUYENTHU tt = db.TUYENTHUs.Where(x => x.IDTUYENTHU == kh.IDTUYENTHU).FirstOrDefault();
                pt1.KHACHHANG.TUYENTHU = tt;
                pt1.KHACHHANG.TUYENTHU.KHACHHANGs = null;
                List<PHANQUYENTUYENTHU> ltt = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == kh.IDTUYENTHU).ToList();
                tt.PHANQUYENTUYENTHUs = ltt;
                foreach(PHANQUYENTUYENTHU ltt1 in ltt)
                {
                    NHANVIEN ngt = db.NHANVIENs.Where(u => u.IDNHANVIEN == ltt1.IDNHANVIEN).FirstOrDefault();
                    ltt1.NHANVIEN = ngt;
                    ngt.PHANQUYENTUYENTHUs = null;
                }
                XAPHUONG xp = db.XAPHUONGs.SingleOrDefault(x => x.IDXAPHUONG == tt.IDXAPHUONG);
                tt.XAPHUONG = xp;
                QUANHUYEN qh = db.QUANHUYENs.SingleOrDefault(x => x.IDQUANHUYEN == xp.IDQUANHUYEN);
                xp.QUANHUYEN = qh;
                qh.XAPHUONGs = null;
            }
            IEnumerable<PHIEUTHU> pts = pt.OrderByDescending(p => p.IDPHIEU);
            return pts;
        }

        // GET: api/PHIEUTHUs/5
        [ResponseType(typeof(PHIEUTHU))]
        public async Task<IHttpActionResult> GetPHIEUTHU(int id)
        {
            PHIEUTHU pt1 = await db.PHIEUTHUs.FindAsync(id);
            if (pt1 == null)
            {
                return NotFound();
            }
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(x => x.IDKHACHHANG == pt1.IDKHACHHANG);
            KYTHU kt = db.KYTHUs.SingleOrDefault(x => x.IDKYTHU == pt1.IDKYTHU);
            NHANVIEN nv = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == pt1.IDNHANVIEN);
            List<CHITIETPHIEUTHU> ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt1.IDPHIEU).ToList();
            pt1.KHACHHANG = kh;
            pt1.KYTHU = kt;
            pt1.NHANVIEN = nv;
            pt1.KYTHU.PHIEUTHUs = null;
            pt1.CHITIETPHIEUTHUs = ct;
            pt1.NHANVIEN.PHIEUTHUs = null;
            LOAIKH lkh = db.LOAIKHs.SingleOrDefault(x => x.IDLOAIKH == kh.IDLOAIKH);
            pt1.KHACHHANG.LOAIKH = lkh;
            TUYENTHU tt = db.TUYENTHUs.SingleOrDefault(x => x.IDTUYENTHU == kh.IDTUYENTHU);
            pt1.KHACHHANG.TUYENTHU = tt;

            return Ok(pt1);
        }

        [Route("capnhatphieuthu/{id}/{idnhanvien}")]
        public IHttpActionResult PatchCapnhat(int id, PHIEUTHU pHIEUTHU, int idnhanvien)
        {
            var dem = db.PHIEUTHUs.Count(e => e.IDKHACHHANG.Equals(pHIEUTHU.IDKHACHHANG) && (id != e.IDPHIEU) && (e.IDKYTHU == pHIEUTHU.IDKYTHU));
            if (dem > 0)
            {
                ModelState.AddModelError("khachhang", "Khách hàng này đã được tạo phiếu !");
                return BadRequest(ModelState);
            }

            List<PHIEUTHU> lpt = db.PHIEUTHUs.ToList();
            PHIEUTHU pt = new PHIEUTHU();
            pt = lpt.Find(s => s.IDPHIEU == pHIEUTHU.IDPHIEU);
            pt.NGAYTAO = pHIEUTHU.NGAYTAO;
            pt.IDKHACHHANG = pHIEUTHU.IDKHACHHANG;
            pt.IDKYTHU = pHIEUTHU.IDKYTHU;
            pt.IDNHANVIEN = pHIEUTHU.IDNHANVIEN;
            pt.MAUSOPHIEU = pHIEUTHU.MAUSOPHIEU;
            pt.TRANGTHAIPHIEU = pHIEUTHU.TRANGTHAIPHIEU;
            pt.KYHIEU = pHIEUTHU.KYHIEU;
       
            pt.NGUOICAPNHAT = db.NHANVIENs.ToList().Find(c => c.IDNHANVIEN == idnhanvien).HOTEN;
            pt.NGAYCAPNHAT = DateTime.Now;
            CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.SingleOrDefault(x => x.IDPHIEU == pHIEUTHU.IDPHIEU);
            ct.IDCHITIETPHIEUTHU = pHIEUTHU.CHITIETPHIEUTHUs.SingleOrDefault().IDCHITIETPHIEUTHU;
            ct.NOIDUNG = pHIEUTHU.CHITIETPHIEUTHUs.SingleOrDefault().NOIDUNG;
            ct.SOTIEN = pHIEUTHU.CHITIETPHIEUTHUs.SingleOrDefault().SOTIEN;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PHIEUTHUs
        [ResponseType(typeof(PHIEUTHU))]
        public async Task<IHttpActionResult> PostPHIEUTHU(PHIEUTHU pHIEUTHU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dem = db.PHIEUTHUs.Where(u => u.IDKHACHHANG == pHIEUTHU.IDKHACHHANG && u.IDKYTHU == pHIEUTHU.IDKYTHU && u.TRANGTHAIHUY == false).Count();
            if (dem > 0)
            {
                ModelState.AddModelError("phieu", "Khách hàng này đã được tạo phiếu!");
                return BadRequest(ModelState);
            }
            if (db.KHACHHANGs.ToList().Find(x => x.IDKHACHHANG == pHIEUTHU.IDKHACHHANG).TRANGTHAI == true)
            {
                List<PHIEUTHU> pts = new List<PHIEUTHU>();
                int max = db.PHIEUTHUs.Max(x => x.IDPHIEU);
                pHIEUTHU.MASOPHIEU = "MS0" + (max + 1);
                pHIEUTHU.TRANGTHAIPHIEU = false;
                pHIEUTHU.TRANGTHAIHUY = false;
                pHIEUTHU.NGAYCAPNHAT = DateTime.Now;
                db.PHIEUTHUs.Add(pHIEUTHU);
                await db.SaveChangesAsync();
            }
            else
            {
                ModelState.AddModelError("khachhang", "Khách hàng này không còn hoạt động!");
                return BadRequest(ModelState);
            }
            return CreatedAtRoute("DefaultApi", new { id = pHIEUTHU.IDPHIEU }, pHIEUTHU);
        }

        // DELETE: api/PHIEUTHUs/5
        [ResponseType(typeof(PHIEUTHU))]
        public async Task<IHttpActionResult> DeletePHIEUTHU(int id, NHANVIEN idnhanvien)
        {
            PHIEUTHU pHIEUTHU = await db.PHIEUTHUs.FindAsync(id);
            if(pHIEUTHU.TRANGTHAIPHIEU == true)
            {
                ModelState.AddModelError("phieu", "Phiếu đã thu không thể xóa!");
                return BadRequest(ModelState);
            }
            if (pHIEUTHU == null)
            {
                return NotFound();
            }
            List<CHITIETPHANQUYEN> lctpq = db.CHITIETPHANQUYENs.Where(x => x.IDNHANVIEN == idnhanvien.IDNHANVIEN).ToList();
            foreach(CHITIETPHANQUYEN ctpq in lctpq)
            {
                var ten = db.QUYENs.Where(x => x.IDQUYEN == ctpq.IDQUYEN).FirstOrDefault().TENQUYEN;
                if(ten == "Admin")
                {
                    pHIEUTHU.TRANGTHAIHUY = true;
                    await db.SaveChangesAsync();

                    return Ok(pHIEUTHU);                    
                }
            }
            ModelState.AddModelError("phieu", "Bạn không có quyền xóa phiếu!");
            return BadRequest(ModelState);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PHIEUTHUExists(int id)
        {
            return db.PHIEUTHUs.Count(e => e.IDPHIEU == id) > 0;
        }
    }
}