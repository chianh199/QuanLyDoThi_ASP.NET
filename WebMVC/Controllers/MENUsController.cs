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
    public class MENUsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/MENUs
        public List<MENU> GetMENUs()
        {
            var menu = db.MENUs.ToList();
            foreach (MENU menu1 in menu)
            {
                var pqmn = db.PHANQUYENMENUs.Where(x => x.IDMENU == menu1.IDMENU).ToList();
                menu1.PHANQUYENMENUs = pqmn;
                foreach (PHANQUYENMENU pqmn1 in pqmn)
                {
                    var q = db.QUYENs.Where(x => x.IDQUYEN == pqmn1.IDQUYEN).FirstOrDefault();
                    pqmn1.QUYEN = q;
                    pqmn1.QUYEN.PHANQUYENMENUs = null;
                }
            }

            return menu;
        }
    

        // GET: api/MENUs/5
        [ResponseType(typeof(MENU))]
        public async Task<IHttpActionResult> GetMENU(int id)
        {
            MENU mENU = await db.MENUs.FindAsync(id);
            if (mENU == null)
            {
                return NotFound();
            }

            return Ok(mENU);
        }

        // PUT: api/MENUs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMENU(int id, MENU mENU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mENU.IDMENU)
            {
                return BadRequest();
            }
            var dem = db.MENUs.Count(e => e.TENMENU.Equals(mENU.TENMENU));
            if (dem > 0)
            {
                ModelState.AddModelError("q", "Menu này đã tồn tại!");
                return BadRequest(ModelState);
            }
            db.Entry(mENU).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MENUExists(id))
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

        // POST: api/MENUs
        [ResponseType(typeof(MENU))]
        public async Task<IHttpActionResult> PostMENU(MENU mENU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dem = db.MENUs.Count(e => e.TENMENU.Equals(mENU.TENMENU));
            if (dem > 0)
            {
                ModelState.AddModelError("q", "Menu này đã tồn tại!");
                return BadRequest(ModelState);
            }
            db.MENUs.Add(mENU);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = mENU.IDMENU }, mENU);
        }

        // DELETE: api/MENUs/5
        [ResponseType(typeof(MENU))]
        public async Task<IHttpActionResult> DeleteMENU(int id)
        {
            MENU mENU = await db.MENUs.FindAsync(id);
            if (mENU == null)
            {
                return NotFound();
            }

            db.MENUs.Remove(mENU);
            await db.SaveChangesAsync();

            return Ok(mENU);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MENUExists(int id)
        {
            return db.MENUs.Count(e => e.IDMENU == id) > 0;
        }
    }
}