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
    public class PHANQUYENMENUsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/PHANQUYENMENUs
        public IQueryable<PHANQUYENMENU> GetPHANQUYENMENUs()
        {
            return db.PHANQUYENMENUs;
        }

        // GET: api/PHANQUYENMENUs/5
        [ResponseType(typeof(PHANQUYENMENU))]
        public async Task<IHttpActionResult> GetPHANQUYENMENU(int id)
        {
            PHANQUYENMENU pHANQUYENMENU = await db.PHANQUYENMENUs.FindAsync(id);
            if (pHANQUYENMENU == null)
            {
                return NotFound();
            }

            return Ok(pHANQUYENMENU);
        }

        // PUT: api/PHANQUYENMENUs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPHANQUYENMENU(int id, PHANQUYENMENU pHANQUYENMENU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pHANQUYENMENU.IDPHANQUYENMENU)
            {
                return BadRequest();
            }

            db.Entry(pHANQUYENMENU).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PHANQUYENMENUExists(id))
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

        // POST: api/PHANQUYENMENUs
        [ResponseType(typeof(PHANQUYENMENU))]
        public void Post(List<PHANQUYENMENU> lpqMenu)
        {
            foreach (PHANQUYENMENU addpqMenu in lpqMenu)
            {
                var dem = db.PHANQUYENMENUs.Count(e => e.IDQUYEN == addpqMenu.IDQUYEN && (e.IDMENU == addpqMenu.IDMENU));
                if (dem <= 0)
                {
                    db.PHANQUYENMENUs.Add(addpqMenu);
                    db.SaveChanges();
                }
            }
        }


        // DELETE: api/PHANQUYENMENUs/5
        [ResponseType(typeof(PHANQUYENMENU))]
        public async Task<IHttpActionResult> DeletePHANQUYENMENU(int id)
        {
            PHANQUYENMENU pHANQUYENMENU = await db.PHANQUYENMENUs.FindAsync(id);
            if (pHANQUYENMENU == null)
            {
                return NotFound();
            }

            db.PHANQUYENMENUs.Remove(pHANQUYENMENU);
            await db.SaveChangesAsync();

            return Ok(pHANQUYENMENU);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PHANQUYENMENUExists(int id)
        {
            return db.PHANQUYENMENUs.Count(e => e.IDPHANQUYENMENU == id) > 0;
        }
    }
}