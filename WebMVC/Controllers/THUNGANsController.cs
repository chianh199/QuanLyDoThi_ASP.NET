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
    public class THUNGANsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();
        // GET: pt/1/1
        [Route("phieuthu/{idNhanvien}")]
        public IEnumerable<PHIEUTHU> GetPhieuthu(int idNhanvien)
        {

            List<PHANQUYENTUYENTHU> lpqtt = db.PHANQUYENTUYENTHUs.ToList();
            List<PHANQUYENTUYENTHU> lpqtt1 = new List<PHANQUYENTUYENTHU>();
            foreach (PHANQUYENTUYENTHU l in lpqtt)
            {
                if (idNhanvien == l.IDNHANVIEN)
                {
                    lpqtt1.Add(l);
                }
            }
            List<KHACHHANG> lkh = db.KHACHHANGs.ToList();
            List<KHACHHANG> lkh1 = new List<KHACHHANG>();
            foreach (PHANQUYENTUYENTHU l in lpqtt1)
            {
                foreach (KHACHHANG k in lkh)
                {
                    // (k.IDTUYENTHU == l.IDTUYENTHU) && k.TRANGTHAI == true
                    if (k.IDTUYENTHU == l.IDTUYENTHU)
                    {
                        lkh1.Add(k);
                    }
                }
            }

            List<PHIEUTHU> lpt = db.PHIEUTHUs.ToList();
            List<PHIEUTHU> lpt1 = new List<PHIEUTHU>();
            //KYTHU kt = db.KYTHUs.ToList().Find(s => s.IDKYTHU == idkythu);
            List<CHITIETPHIEUTHU> ctp = db.CHITIETPHIEUTHUs.ToList();
            // List<CHITIETPHIEUTHU> ctp1 = new List<CHITIETPHIEUTHU>();
            List<NHANVIEN> lnv = db.NHANVIENs.ToList();
            foreach (KHACHHANG kh in lkh1)
            {
                List<PHIEUTHU> lptt = new List<PHIEUTHU>();
                lptt = lpt.FindAll(s => s.IDKHACHHANG == kh.IDKHACHHANG);

                foreach (PHIEUTHU p in lptt)
                {
                    
                    p.CHITIETPHIEUTHUs = ctp.FindAll(c => c.IDPHIEU == p.IDPHIEU);
                    p.NHANVIEN = lnv.Find(c => c.IDNHANVIEN == idNhanvien);
                    p.NHANVIEN.PHANQUYENTUYENTHUs = null;
                    p.NHANVIEN.PHIEUTHUs = null;
                    p.KYTHU = db.KYTHUs.ToList().Find(c => c.IDKYTHU == p.IDKYTHU);
                    p.KHACHHANG.LOAIKH = db.LOAIKHs.Where(x => x.IDLOAIKH == p.KHACHHANG.IDLOAIKH).FirstOrDefault();
                    p.KHACHHANG.TUYENTHU = db.TUYENTHUs.ToList().Find(x => x.IDTUYENTHU == p.KHACHHANG.IDTUYENTHU);
                    p.KHACHHANG.TUYENTHU.KHACHHANGs = null;
                    p.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = null;
                    p.KYTHU.PHIEUTHUs = null;
                    p.KHACHHANG.LOAIKH.KHACHHANGs = null;
                    lpt1.Add(p);

                }
            }
            IEnumerable<PHIEUTHU> lpts = lpt1.OrderByDescending(t => t.IDPHIEU);

            return lpts;
        }

        // GET: kh/5
        [Route("kh/{idNhanvien}")]
        public IEnumerable<KHACHHANG> Get(int idNhanvien)
        {

            List<PHANQUYENTUYENTHU> lpqtt = db.PHANQUYENTUYENTHUs.ToList();
            List<PHANQUYENTUYENTHU> lpqtt1 = new List<PHANQUYENTUYENTHU>();
            foreach (PHANQUYENTUYENTHU l in lpqtt)
            {
                if (idNhanvien == l.IDNHANVIEN)
                {
                    lpqtt1.Add(l);
                }
            }
            List<LOAIKH> llkh = db.LOAIKHs.ToList();
            List<TUYENTHU> ltt = db.TUYENTHUs.ToList();
            List<KHACHHANG> lkh = db.KHACHHANGs.ToList();
            List<XAPHUONG> lxp = db.XAPHUONGs.ToList();
            List<KHACHHANG> lkh1 = new List<KHACHHANG>();
            List<QUANHUYEN> lqh = db.QUANHUYENs.ToList();
            foreach (PHANQUYENTUYENTHU l in lpqtt1)
            {
                foreach (KHACHHANG k in lkh)
                {
                    if ((k.IDTUYENTHU == l.IDTUYENTHU) && k.TRANGTHAI == true)
                    {
                        k.LOAIKH = llkh.Find(c => c.IDLOAIKH == k.IDLOAIKH);
                        k.LOAIKH.KHACHHANGs = null;
                        k.TUYENTHU.KHACHHANGs = null;
                        k.TUYENTHU.PHANQUYENTUYENTHUs = null;
                        k.TUYENTHU.XAPHUONG = lxp.Find(c => c.IDXAPHUONG == k.TUYENTHU.IDXAPHUONG);
                        k.TUYENTHU.XAPHUONG.TUYENTHUs = null;
                        k.TUYENTHU.XAPHUONG.QUANHUYEN = lqh.Find(c => c.IDQUANHUYEN == k.TUYENTHU.XAPHUONG.IDQUANHUYEN);
                        k.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;
                        lkh1.Add(k);
                    }
                }
            }
            IEnumerable<KHACHHANG> lkhs = lkh1.OrderByDescending(k => k.IDKHACHHANG);
            return lkhs;
        }

        // PUT: api/THUNGANs/15
        public async Task<IHttpActionResult> PutNHANVIEN(int id, NHANVIEN idnhanvien)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PHIEUTHU pt1 = db.PHIEUTHUs.Where(x => x.IDPHIEU == id).FirstOrDefault();
            if (pt1==null)
            {
                return NotFound();
            }
            if(pt1.TRANGTHAIPHIEU == true)
            {
                ModelState.AddModelError("phieu", "Phiếu đã được xác nhận!");
                return BadRequest(ModelState);
            }
            KYTHU kt = db.KYTHUs.SingleOrDefault(x => x.IDKYTHU == pt1.IDKYTHU);
            if(kt.TRANGTHAIKYTHU == false)
            {
                ModelState.AddModelError("kythu", "Kỳ thu đã đóng!");
                return BadRequest(ModelState);
            }
                pt1.IDNGUOITHU = idnhanvien.IDNHANVIEN;
                pt1.NGUOICAPNHAT = db.NHANVIENs.Where(x => x.IDNHANVIEN == idnhanvien.IDNHANVIEN).FirstOrDefault().HOTEN;
                pt1.NGUOITHU = db.NHANVIENs.Where(x => x.IDNHANVIEN == idnhanvien.IDNHANVIEN).FirstOrDefault().HOTEN;
                pt1.NGAYCAPNHAT = DateTime.Now;
                pt1.TRANGTHAIPHIEU = true;
                db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/THUNGANs
        [ResponseType(typeof(NHANVIEN))]
        public async Task<IHttpActionResult> PostNHANVIEN(NHANVIEN nHANVIEN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NHANVIENs.Add(nHANVIEN);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = nHANVIEN.IDNHANVIEN }, nHANVIEN);
        }

        // DELETE: api/THUNGANs/5
        [ResponseType(typeof(NHANVIEN))]
        public async Task<IHttpActionResult> DeleteNHANVIEN(int id)
        {
            NHANVIEN nHANVIEN = await db.NHANVIENs.FindAsync(id);
            if (nHANVIEN == null)
            {
                return NotFound();
            }

            db.NHANVIENs.Remove(nHANVIEN);
            await db.SaveChangesAsync();

            return Ok(nHANVIEN);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NHANVIENExists(int id)
        {
            return db.NHANVIENs.Count(e => e.IDNHANVIEN == id) > 0;
        }
    }
}