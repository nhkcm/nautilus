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
    public class notasController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/notas
        public IQueryable<notas> Getnotas()
        {
            return db.notas;
        }

        // GET: api/notas/5
        [ResponseType(typeof(notas))]
        public async Task<IHttpActionResult> Getnotas(int id)
        {
            notas notas = await db.notas.FindAsync(id);
            if (notas == null)
            {
                return NotFound();
            }

            return Ok(notas);
        }

        // PUT: api/notas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putnotas(int id, notas notas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notas.id)
            {
                return BadRequest();
            }

            db.Entry(notas).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!notasExists(id))
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

        // POST: api/notas
        [ResponseType(typeof(notas))]
        public async Task<IHttpActionResult> Postnotas(notas notas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.notas.Add(notas);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = notas.id }, notas);
        }

        // DELETE: api/notas/5
        [ResponseType(typeof(notas))]
        public async Task<IHttpActionResult> Deletenotas(int id)
        {
            notas notas = await db.notas.FindAsync(id);
            if (notas == null)
            {
                return NotFound();
            }

            db.notas.Remove(notas);
            await db.SaveChangesAsync();

            return Ok(notas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool notasExists(int id)
        {
            return db.notas.Count(e => e.id == id) > 0;
        }
    }
}