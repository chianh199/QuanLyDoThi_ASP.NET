using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml.Linq;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class NHANVIENsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/NHANVIENs
        public IEnumerable<NHANVIEN> Get()
        {

            List<NHANVIEN> nv = db.NHANVIENs.ToList();
            List<CHITIETPHANQUYEN> ctpq = db.CHITIETPHANQUYENs.ToList();
            foreach (NHANVIEN nv2 in nv)
            {
                List<CHITIETPHANQUYEN> ctpq1 = ctpq.FindAll(m => m.IDNHANVIEN == nv2.IDNHANVIEN);
                foreach (CHITIETPHANQUYEN ct in ctpq1)
                {
                    nv2.CHITIETPHANQUYENs.Add(ct);
                    QUYEN q = db.QUYENs.FirstOrDefault(x => x.IDQUYEN == ct.IDQUYEN);
                    ct.QUYEN = q;
                    ct.QUYEN.CHITIETPHANQUYENs = null;
                }
            }

            IEnumerable<NHANVIEN> nvs = nv.OrderByDescending(n => n.IDNHANVIEN);
            return nvs;
        }

        // GET: api/NHANVIENs/5
        [ResponseType(typeof(NHANVIEN))]
        public async Task<IHttpActionResult> GetNHANVIEN(int id)
        {
            var nHANVIEN = await db.NHANVIENs
                .Include(s => s.CHITIETPHANQUYENs)
                .FirstOrDefaultAsync(s => s.IDNHANVIEN == id);

            if (nHANVIEN == null)
            {
                return NotFound();
            }

            List<CHITIETPHANQUYEN> ctpq = db.CHITIETPHANQUYENs.ToList();

            List<CHITIETPHANQUYEN> ctpq1 = ctpq.FindAll(m => m.IDNHANVIEN == nHANVIEN.IDNHANVIEN);
            foreach (CHITIETPHANQUYEN ct in ctpq1)
            {
                nHANVIEN.CHITIETPHANQUYENs.Add(ct);
                QUYEN q = db.QUYENs.FirstOrDefault(x => x.IDQUYEN == ct.IDQUYEN);
                ct.QUYEN = q;
                ct.QUYEN.CHITIETPHANQUYENs = null;
            }

            return Ok(nHANVIEN);

        }

        // Patch: api/NHANVIENs/5
        public IHttpActionResult Patch(int id, NHANVIEN nvedit)
        {
            if (!ktrachuoi(nvedit.MANHANVIEN))
            {
                ModelState.AddModelError("check", "Mã nhân viên không được có dấu và khoảng cách!");
                return BadRequest(ModelState);
            }
            var dem = db.NHANVIENs.Count(e => e.MANHANVIEN.Equals(nvedit.MANHANVIEN) && (id != e.IDNHANVIEN));
            if (dem > 0)
            {
                ModelState.AddModelError("maNhanvien", "Mã nhân viên đã tồn tại");
                return BadRequest(ModelState);
            }
            var dem1 = db.NHANVIENs.Count(e => e.SDT == nvedit.SDT && (id != e.IDNHANVIEN));
            if (dem1 > 0)
            {
                ModelState.AddModelError("SDT", "SDT đã tồn tại");
                return BadRequest(ModelState);
            }
            List<NHANVIEN> lnv = db.NHANVIENs.ToList();
            NHANVIEN nv = new NHANVIEN();
            nv = lnv.Find(s => s.IDNHANVIEN == nvedit.IDNHANVIEN);
            nv.IDNHANVIEN = nvedit.IDNHANVIEN;
            nv.HOTEN = nvedit.HOTEN;
            nv.MANHANVIEN = nvedit.MANHANVIEN;
            nv.NGAYSINH = nvedit.NGAYSINH;
            nv.DIACHI = nvedit.DIACHI;
            nv.SDT = nvedit.SDT;
            
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/NHANVIENs
        [ResponseType(typeof(NHANVIEN))]
        public async Task<IHttpActionResult> PostNHANVIEN(NHANVIEN nHANVIEN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!ktrachuoi(nHANVIEN.MANHANVIEN))
            {
                ModelState.AddModelError("inputcheck", "Mã nhân viên không được có dấu và khoảng cách!");
                return BadRequest(ModelState);
            }
            var ma = db.NHANVIENs.Count(e => e.MANHANVIEN.Equals(nHANVIEN.MANHANVIEN));
            if (ma > 0)
            {
                ModelState.AddModelError("manhanvien", "Mã nhân viên đã tồn tại!");
                return BadRequest(ModelState);
            }
            
            var sdt = db.NHANVIENs.Count(e => e.SDT == nHANVIEN.SDT);
            if (sdt > 0)
            {
                ModelState.AddModelError("sdt", "Số điện thoại đã tồn tại!");
                return BadRequest(ModelState);
            }
            
            byte[] buffer = Encoding.UTF8.GetBytes("123456");
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);
            nHANVIEN.PASSWORD = null;
            for (int i = 0; i < buffer.Length; i++)
            {
                nHANVIEN.PASSWORD += buffer[i].ToString("x1");
            }

            db.NHANVIENs.Add(nHANVIEN);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = nHANVIEN.IDNHANVIEN }, nHANVIEN);
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

        // DELETE: api/NHANVIENs/5
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