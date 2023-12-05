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
    public class QUANHUYENsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/QUANHUYENs
        public IQueryable<QUANHUYEN> GetQUANHUYENs()
        {
            return db.QUANHUYENs.OrderByDescending(qh => qh.IDQUANHUYEN);
        }

        // GET: api/QUANHUYENs/5
        [ResponseType(typeof(QUANHUYEN))]
        public async Task<IHttpActionResult> GetQUANHUYEN(int id)
        {
            QUANHUYEN qUANHUYEN = await db.QUANHUYENs.FindAsync(id);
            if (qUANHUYEN == null)
            {
                return NotFound();
            }
            

            return Ok(qUANHUYEN);
        }

        // PUT: api/QUANHUYENs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutQUANHUYEN(int id, QUANHUYEN qUANHUYEN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != qUANHUYEN.IDQUANHUYEN)
            {
                return BadRequest();
            }
            var dem = db.QUANHUYENs.Count(e => e.TENQUANHUYEN.Equals(qUANHUYEN.TENQUANHUYEN));
            if (dem > 0)
            {
                ModelState.AddModelError("qh", "Quận huyện đã tồn tại!");
                return BadRequest(ModelState);
            }

            db.Entry(qUANHUYEN).State = EntityState.Modified;
            
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QUANHUYENExists(id))
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

        // POST: api/QUANHUYENs
        [ResponseType(typeof(QUANHUYEN))]
        public async Task<IHttpActionResult> PostQUANHUYEN(QUANHUYEN qUANHUYEN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dem = db.QUANHUYENs.Count(e => e.TENQUANHUYEN.Equals(qUANHUYEN.TENQUANHUYEN));
            if(dem > 0)
            {
                ModelState.AddModelError("qh", "Quận huyện đã tồn tại!");
                return BadRequest(ModelState);
            }
            else
            {
                db.QUANHUYENs.Add(qUANHUYEN);
                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = qUANHUYEN.IDQUANHUYEN }, qUANHUYEN);
            }

         }

        // DELETE: api/QUANHUYENs/5
        [ResponseType(typeof(QUANHUYEN))]
        public async Task<IHttpActionResult> DeleteQUANHUYEN(int id)
        {
            QUANHUYEN qUANHUYEN = await db.QUANHUYENs.FindAsync(id);
            if (qUANHUYEN == null)
            {
                return NotFound();
            }

            db.QUANHUYENs.Remove(qUANHUYEN);
            await db.SaveChangesAsync();

            return Ok(qUANHUYEN);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QUANHUYENExists(int id)
        {
            return db.QUANHUYENs.Count(e => e.IDQUANHUYEN == id) > 0;
        }
    }
}