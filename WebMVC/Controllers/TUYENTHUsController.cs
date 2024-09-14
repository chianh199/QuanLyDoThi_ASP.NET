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
    public class TUYENTHUsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/TUYENTHUs
        public IEnumerable<TUYENTHU> GetTUYENTHUs()
        {
            List<TUYENTHU> xp = db.TUYENTHUs.ToList();
            List<PHANQUYENTUYENTHU> ctpq = db.PHANQUYENTUYENTHUs.ToList();
            foreach (TUYENTHU xp1 in xp)
            {

                XAPHUONG q = db.XAPHUONGs.FirstOrDefault(x => x.IDXAPHUONG == xp1.IDXAPHUONG);
                xp1.XAPHUONG = q;
                xp1.XAPHUONG.TUYENTHUs = null;
                QUANHUYEN qh = db.QUANHUYENs.FirstOrDefault(x => x.IDQUANHUYEN == q.IDQUANHUYEN);
                xp1.XAPHUONG.QUANHUYEN = qh;
                q.QUANHUYEN.XAPHUONGs = null;

                List<PHANQUYENTUYENTHU> ctpq1 = ctpq.FindAll(m => m.IDTUYENTHU == xp1.IDTUYENTHU);
                foreach (PHANQUYENTUYENTHU ct in ctpq1)
                {
                    xp1.PHANQUYENTUYENTHUs.Add(ct);
                    NHANVIEN nv = db.NHANVIENs.FirstOrDefault(x => x.IDNHANVIEN == ct.IDNHANVIEN);
                    ct.NHANVIEN = nv;
                }
                    
            }

            IEnumerable<TUYENTHU> tt = xp.OrderByDescending(t => t.IDTUYENTHU);
            return tt;
        }

        // GET: api/TUYENTHUs/5
        [ResponseType(typeof(TUYENTHU))]
        public async Task<IHttpActionResult> GetTUYENTHU(int id)
        {
            TUYENTHU tUYENTHU = await db.TUYENTHUs
                .Include(x => x.PHANQUYENTUYENTHUs)
                .FirstOrDefaultAsync(x => x.IDTUYENTHU == id);
            if (tUYENTHU == null)
            {
                return NotFound();
            }
            List<PHANQUYENTUYENTHU> ctpq = db.PHANQUYENTUYENTHUs.ToList();

            XAPHUONG q = db.XAPHUONGs.FirstOrDefault(x => x.IDXAPHUONG == tUYENTHU.IDXAPHUONG);
            tUYENTHU.XAPHUONG = q;
            tUYENTHU.XAPHUONG.TUYENTHUs = null;
            QUANHUYEN qh = db.QUANHUYENs.FirstOrDefault(x => x.IDQUANHUYEN == q.IDQUANHUYEN);
            tUYENTHU.XAPHUONG.QUANHUYEN = qh;

            List<PHANQUYENTUYENTHU> ctpq1 = ctpq.FindAll(m => m.IDTUYENTHU == tUYENTHU.IDTUYENTHU);
            foreach (PHANQUYENTUYENTHU ct in ctpq1)
            {
                tUYENTHU.PHANQUYENTUYENTHUs.Add(ct);
                NHANVIEN nv = db.NHANVIENs.FirstOrDefault(x => x.IDNHANVIEN == ct.IDNHANVIEN);
                ct.NHANVIEN = nv;
            }
            return Ok(tUYENTHU);
        }

        // PUT: api/TUYENTHUs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTUYENTHU(int id, TUYENTHU tUYENTHU)
        {
            

            if (id != tUYENTHU.IDTUYENTHU)
            {
                return BadRequest();
            }

            db.Entry(tUYENTHU).State = EntityState.Modified;

            try
            {   

                await db.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TUYENTHUExists(id))
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

        // POST: api/TUYENTHUs
        [ResponseType(typeof(TUYENTHU))]
        public async Task<IHttpActionResult> PostTUYENTHU(TUYENTHU tUYENTHU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TUYENTHUs.Add(tUYENTHU);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUYENTHU.IDTUYENTHU }, tUYENTHU);
        }

        // DELETE: api/TUYENTHUs/5
        [ResponseType(typeof(TUYENTHU))]
        public async Task<IHttpActionResult> DeleteTUYENTHU(int id)
        {
            TUYENTHU tUYENTHU = await db.TUYENTHUs.FindAsync(id);
            if (tUYENTHU == null)
            {
                return NotFound();
            }

            db.TUYENTHUs.Remove(tUYENTHU);
            await db.SaveChangesAsync();

            return Ok(tUYENTHU);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TUYENTHUExists(int id)
        {
            return db.TUYENTHUs.Count(e => e.IDTUYENTHU == id) > 0;
        }
    }
}