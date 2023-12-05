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
    public class QUYENsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/QUYENs
        public IQueryable<QUYEN> GetQUYENs()
        {
            return db.QUYENs.OrderByDescending(q => q.IDQUYEN);
        }

        // GET: api/QUYENs/5
        [ResponseType(typeof(QUYEN))]
        public async Task<IHttpActionResult> GetQUYEN(int id)
        {
            QUYEN qUYEN = await db.QUYENs.FindAsync(id);
            if (qUYEN == null)
            {
                return NotFound();
            }
            QUYENDTO nHANVIENFiltered = new QUYENDTO
            {
                IDQUYEN = qUYEN.IDQUYEN,
                TENQUYEN = qUYEN.TENQUYEN
            };
            return Ok(nHANVIENFiltered);
        }

        // PUT: api/QUYENs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutQUYEN(int id, QUYEN qUYEN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != qUYEN.IDQUYEN)
            {
                return BadRequest();
            }
            var dem = db.QUYENs.Count(e => e.TENQUYEN.Equals(qUYEN.TENQUYEN));
            if (dem > 0)
            {
                ModelState.AddModelError("q", "Quyền này đã tồn tại!");
                return BadRequest(ModelState);
            }
            db.Entry(qUYEN).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QUYENExists(id))
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

        // POST: api/QUYENs
        [ResponseType(typeof(QUYEN))]
        public async Task<IHttpActionResult> PostQUYEN(QUYEN qUYEN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dem = db.QUYENs.Count(e => e.TENQUYEN.Equals(qUYEN.TENQUYEN));
            if (dem > 0)
            {
                ModelState.AddModelError("q", "Quyền này đã tồn tại!");
                return BadRequest(ModelState);
            }
            db.QUYENs.Add(qUYEN);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = qUYEN.IDQUYEN }, qUYEN);
        }

        // DELETE: api/QUYENs/5
        [ResponseType(typeof(QUYEN))]
        public async Task<IHttpActionResult> DeleteQUYEN(int id)
        {
            QUYEN qUYEN = await db.QUYENs.FindAsync(id);
            if (qUYEN == null)
            {
                return NotFound();
            }

            db.QUYENs.Remove(qUYEN);
            await db.SaveChangesAsync();

            return Ok(qUYEN);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QUYENExists(int id)
        {
            return db.QUYENs.Count(e => e.IDQUYEN == id) > 0;
        }
    }
}