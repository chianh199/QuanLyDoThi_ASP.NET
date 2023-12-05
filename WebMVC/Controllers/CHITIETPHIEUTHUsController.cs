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
    public class CHITIETPHIEUTHUsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/CHITIETPHIEUTHUs
        public IQueryable<CHITIETPHIEUTHU> GetCHITIETPHIEUTHUs()
        {
            return db.CHITIETPHIEUTHUs;
        }

        // GET: api/CHITIETPHIEUTHUs/5
        [ResponseType(typeof(CHITIETPHIEUTHU))]
        public async Task<IHttpActionResult> GetCHITIETPHIEUTHU(int id)
        {
            CHITIETPHIEUTHU cHITIETPHIEUTHU = await db.CHITIETPHIEUTHUs.FindAsync(id);
            if (cHITIETPHIEUTHU == null)
            {
                return NotFound();
            }

            return Ok(cHITIETPHIEUTHU);
        }

        // PUT: api/CHITIETPHIEUTHUs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCHITIETPHIEUTHU(int id, CHITIETPHIEUTHU cHITIETPHIEUTHU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cHITIETPHIEUTHU.IDCHITIETPHIEUTHU)
            {
                return BadRequest();
            }

            db.Entry(cHITIETPHIEUTHU).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CHITIETPHIEUTHUExists(id))
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

        // POST: api/CHITIETPHIEUTHUs
        [ResponseType(typeof(CHITIETPHIEUTHU))]
        public async Task<IHttpActionResult> PostCHITIETPHIEUTHU(CHITIETPHIEUTHU cHITIETPHIEUTHU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CHITIETPHIEUTHUs.Add(cHITIETPHIEUTHU);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cHITIETPHIEUTHU.IDCHITIETPHIEUTHU }, cHITIETPHIEUTHU);
        }

        // DELETE: api/CHITIETPHIEUTHUs/5
        [ResponseType(typeof(CHITIETPHIEUTHU))]
        public async Task<IHttpActionResult> DeleteCHITIETPHIEUTHU(int id)
        {
            CHITIETPHIEUTHU cHITIETPHIEUTHU = await db.CHITIETPHIEUTHUs.FindAsync(id);
            if (cHITIETPHIEUTHU == null)
            {
                return NotFound();
            }

            db.CHITIETPHIEUTHUs.Remove(cHITIETPHIEUTHU);
            await db.SaveChangesAsync();

            return Ok(cHITIETPHIEUTHU);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CHITIETPHIEUTHUExists(int id)
        {
            return db.CHITIETPHIEUTHUs.Count(e => e.IDCHITIETPHIEUTHU == id) > 0;
        }
    }
}