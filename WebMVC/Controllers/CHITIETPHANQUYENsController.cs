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
    public class CHITIETPHANQUYENsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/CHITIETPHANQUYENs
        public IQueryable<CHITIETPHANQUYEN> GetCHITIETPHANQUYENs()
        {
            return db.CHITIETPHANQUYENs;
        }

        // GET: api/CHITIETPHANQUYENs/5
        [ResponseType(typeof(CHITIETPHANQUYEN))]
        public async Task<IHttpActionResult> GetCHITIETPHANQUYEN(int id)
        {
            CHITIETPHANQUYEN cHITIETPHANQUYEN = await db.CHITIETPHANQUYENs.FindAsync(id);
            if (cHITIETPHANQUYEN == null)
            {
                return NotFound();
            }

            return Ok(cHITIETPHANQUYEN);
        }

        // PUT: api/CHITIETPHANQUYENs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCHITIETPHANQUYEN(int id, CHITIETPHANQUYEN cHITIETPHANQUYEN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cHITIETPHANQUYEN.IDCHITIETPHANQUYEN)
            {
                return BadRequest();
            }

            db.Entry(cHITIETPHANQUYEN).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CHITIETPHANQUYENExists(id))
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

        // POST: api/CHITIETPHANQUYENs
        [ResponseType(typeof(CHITIETPHANQUYEN))]
        public void Post(List<CHITIETPHANQUYEN> lctpq)
        {
            foreach (CHITIETPHANQUYEN addctqp in lctpq)
            {
                var dem = db.CHITIETPHANQUYENs.Count(e => e.IDNHANVIEN == addctqp.IDNHANVIEN && (e.IDQUYEN == addctqp.IDQUYEN));
                if (dem <= 0)
                {
                    db.CHITIETPHANQUYENs.Add(addctqp);
                    db.SaveChanges();
                }
            }
        }

        // DELETE: api/CHITIETPHANQUYENs
        [ResponseType(typeof(CHITIETPHANQUYEN))]
        public async Task<IHttpActionResult> DeleteCHITIETPHANQUYEN(List<CHITIETPHANQUYEN> ctpq)
        {


            foreach (CHITIETPHANQUYEN ctpq1 in ctpq)
            {
                CHITIETPHANQUYEN cHITIETPHANQUYEN = db.CHITIETPHANQUYENs.Where(e => e.IDNHANVIEN == ctpq1.IDNHANVIEN && e.IDQUYEN == ctpq1.IDQUYEN).FirstOrDefault();
                if(cHITIETPHANQUYEN != null)
                {
                    db.CHITIETPHANQUYENs.Remove(cHITIETPHANQUYEN);
                    db.SaveChanges();
                }
            }

            return Ok("Xoa thanh cong");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CHITIETPHANQUYENExists(int id)
        {
            return db.CHITIETPHANQUYENs.Count(e => e.IDCHITIETPHANQUYEN == id) > 0;
        }
    }
}