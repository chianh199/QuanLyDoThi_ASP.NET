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
using System.Web.UI.WebControls;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class LOAIKHsController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();

        // GET: api/LOAIKHs
        public IEnumerable<LOAIKH> GetLOAIKHs()
        {
            return db.LOAIKHs.OrderByDescending(lkh => lkh.IDLOAIKH);
        }

        // GET: api/LOAIKHs/5
        [ResponseType(typeof(LOAIKH))]
        public async Task<IHttpActionResult> GetLOAIKH(int id)
        {
            LOAIKH lOAIKH = await db.LOAIKHs.FindAsync(id);
            if (lOAIKH == null)
            {
                return NotFound();
            }

            return Ok(lOAIKH);
        }

        // PUT: api/LOAIKHs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLOAIKH(int id, LOAIKH lOAIKH)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lOAIKH.IDLOAIKH)
            {
                return BadRequest();
            }
            var dem = db.LOAIKHs.Count(e => e.TENLOAI.Equals(lOAIKH.TENLOAI) && e.TENLOAIPHI.Equals(lOAIKH.TENLOAIPHI));
            if (dem > 0)
            {
                ModelState.AddModelError("lkh", "Loại khách hàng này đã tồn tại!");
                return BadRequest(ModelState);
            }
            db.Entry(lOAIKH).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LOAIKHExists(id))
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

        // POST: api/LOAIKHs
        [ResponseType(typeof(LOAIKH))]
        public async Task<IHttpActionResult> PostLOAIKH(LOAIKH lOAIKH)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dem = db.LOAIKHs.Count(e => e.TENLOAI.Equals(lOAIKH.TENLOAI) && e.TENLOAIPHI.Equals(lOAIKH.TENLOAIPHI));
            if (dem > 0)
            {
                ModelState.AddModelError("lkh", "Loại khách hàng này đã tồn tại!");
                return BadRequest(ModelState);
            }
            db.LOAIKHs.Add(lOAIKH);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = lOAIKH.IDLOAIKH }, lOAIKH);
        }

        // DELETE: api/LOAIKHs/5
        [ResponseType(typeof(LOAIKH))]
        public async Task<IHttpActionResult> DeleteLOAIKH(int id)
        {
            LOAIKH lOAIKH = await db.LOAIKHs.FindAsync(id);
            if (lOAIKH == null)
            {
                return NotFound();
            }

            db.LOAIKHs.Remove(lOAIKH);
            await db.SaveChangesAsync();

            return Ok(lOAIKH);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LOAIKHExists(int id)
        {
            return db.LOAIKHs.Count(e => e.IDLOAIKH == id) > 0;
        }
    }
}