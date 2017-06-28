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
    public class institucionesController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/instituciones
        public IQueryable<institucion> Getinstitucion()
        {
            return db.institucion;
        }

        // GET: api/instituciones/5
        [ResponseType(typeof(institucion))]
        public async Task<IHttpActionResult> Getinstitucion(int id)
        {
            institucion institucion = await db.institucion.FindAsync(id);
            if (institucion == null)
            {
                return NotFound();
            }

            return Ok(institucion);
        }

        // PUT: api/instituciones/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putinstitucion(int id, institucion institucion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != institucion.id)
            {
                return BadRequest();
            }

            db.Entry(institucion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!institucionExists(id))
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

        // POST: api/instituciones
        [ResponseType(typeof(institucion))]
        public async Task<IHttpActionResult> Postinstitucion(institucion institucion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.institucion.Add(institucion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = institucion.id }, institucion);
        }

        // DELETE: api/instituciones/5
        [ResponseType(typeof(institucion))]
        public async Task<IHttpActionResult> Deleteinstitucion(int id)
        {
            institucion institucion = await db.institucion.FindAsync(id);
            if (institucion == null)
            {
                return NotFound();
            }

            db.institucion.Remove(institucion);
            await db.SaveChangesAsync();

            return Ok(institucion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool institucionExists(int id)
        {
            return db.institucion.Count(e => e.id == id) > 0;
        }
    }
}