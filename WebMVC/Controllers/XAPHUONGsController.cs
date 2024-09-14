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
    public class XAPHUONGsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/XAPHUONGs
        public IEnumerable<XAPHUONG> GetXAPHUONGs()
        {
            List<XAPHUONG> xp = db.XAPHUONGs.ToList();
            
            foreach (XAPHUONG xp1 in xp)
            {

                QUANHUYEN q = db.QUANHUYENs.FirstOrDefault(x => x.IDQUANHUYEN == xp1.IDQUANHUYEN);
                xp1.QUANHUYEN = q;
                xp1.QUANHUYEN.XAPHUONGs = null;
                
            }
            IEnumerable<XAPHUONG> xps = xp.OrderByDescending(x => x.IDXAPHUONG);
            return xps;
        }

        // GET: api/XAPHUONGs/5
        [ResponseType(typeof(XAPHUONG))]
        public async Task<IHttpActionResult> GetXAPHUONG(int id)
        {
            XAPHUONG xAPHUONG = await db.XAPHUONGs.FindAsync(id);
            if (xAPHUONG == null)
            {
                return NotFound();
            }
            QUANHUYEN q = db.QUANHUYENs.FirstOrDefault(x => x.IDQUANHUYEN == xAPHUONG.IDQUANHUYEN);
            xAPHUONG.QUANHUYEN = q;
            xAPHUONG.QUANHUYEN.XAPHUONGs = null;

            return Ok(xAPHUONG);
        }

        // PUT: api/XAPHUONGs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutXAPHUONG(int id, XAPHUONG xAPHUONG)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != xAPHUONG.IDXAPHUONG)
            {
                return BadRequest();
            }
            XAPHUONG xAPHUONG1 = db.XAPHUONGs.SingleOrDefault(x => x.IDQUANHUYEN == xAPHUONG.IDQUANHUYEN && x.TENXAPHUONG.Equals(xAPHUONG.TENXAPHUONG));
            if (xAPHUONG1 != null)
            {
                ModelState.AddModelError("xp", "Xã phường đã tồn tại!");
                return BadRequest(ModelState);
            }

            XAPHUONG xp = db.XAPHUONGs.Where(x => x.IDXAPHUONG == xAPHUONG.IDXAPHUONG).FirstOrDefault();
            
            xp.IDQUANHUYEN = xAPHUONG.IDQUANHUYEN;
            xp.TENXAPHUONG = xAPHUONG.TENXAPHUONG;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!XAPHUONGExists(id))
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

        // POST: api/XAPHUONGs
        [ResponseType(typeof(XAPHUONG))]
        public async Task<IHttpActionResult> PostXAPHUONG(XAPHUONG xAPHUONG)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            XAPHUONG xAPHUONG1 = db.XAPHUONGs.SingleOrDefault(x => x.IDQUANHUYEN == xAPHUONG.IDQUANHUYEN && x.TENXAPHUONG.Equals(xAPHUONG.TENXAPHUONG));
            if(xAPHUONG1 != null)
            {
                ModelState.AddModelError("xp", "Xã phường đã tồn tại!");
                return BadRequest(ModelState);
            }
            db.XAPHUONGs.Add(xAPHUONG);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = xAPHUONG.IDXAPHUONG }, xAPHUONG);
        }

        // DELETE: api/XAPHUONGs/5
        [ResponseType(typeof(XAPHUONG))]
        public async Task<IHttpActionResult> DeleteXAPHUONG(int id)
        {
            XAPHUONG xAPHUONG = await db.XAPHUONGs.FindAsync(id);
            if (xAPHUONG == null)
            {
                return NotFound();
            }

            db.XAPHUONGs.Remove(xAPHUONG);
            await db.SaveChangesAsync();

            return Ok(xAPHUONG);
        }

        private bool XAPHUONGExists(int id)
        {
            return db.XAPHUONGs.Count(e => e.IDXAPHUONG == id) > 0;
        }
    }
}