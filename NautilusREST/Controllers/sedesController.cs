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
using NautilusREST.Models;
using System.Web.Http.Cors;

namespace NautilusREST.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class sedesController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/sedes
        public IEnumerable<dynamic> Getsede()
        {
            return db.sede.Select(x => new {
                x.id,
                institucion_nombre = x.institucion.nombre,
                x.telefono,
                x.direccion,
                aula_count = x.aula.Count()
            });
        }


        // GET: api/sedes/5
        [ResponseType(typeof(sede))]
        public async Task<IHttpActionResult> Getsede(int id)
        {
            sede sede = await db.sede.FindAsync(id);
            if (sede == null)
            {
                return NotFound();
            }

            return Ok(sede);
        }

        // PUT: api/sedes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putsede(int id, sede sede)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sede.id)
            {
                return BadRequest();
            }

            db.Entry(sede).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sedeExists(id))
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

        // POST: api/sedes
        [ResponseType(typeof(sede))]
        public async Task<IHttpActionResult> Postsede(sede sede)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.sede.Add(sede);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = sede.id }, sede);
        }

        // DELETE: api/sedes/5
        [ResponseType(typeof(sede))]
        public async Task<IHttpActionResult> Deletesede(int id)
        {
            sede sede = await db.sede.FindAsync(id);
            if (sede == null)
            {
                return NotFound();
            }

            db.sede.Remove(sede);
            await db.SaveChangesAsync();

            return Ok(sede);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool sedeExists(int id)
        {
            return db.sede.Count(e => e.id == id) > 0;
        }
    }
}