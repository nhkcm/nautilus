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
    public class detalle_horarioController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/detalle_horario
        public IQueryable<detalle_horario> Getdetalle_horario()
        {
            return db.detalle_horario;
        }

        // GET: api/detalle_horario/5
        [ResponseType(typeof(detalle_horario))]
        public async Task<IHttpActionResult> Getdetalle_horario(int id)
        {
            detalle_horario detalle_horario = await db.detalle_horario.FindAsync(id);
            if (detalle_horario == null)
            {
                return NotFound();
            }

            return Ok(detalle_horario);
        }

        // PUT: api/detalle_horario/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putdetalle_horario(int id, detalle_horario detalle_horario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != detalle_horario.id)
            {
                return BadRequest();
            }

            db.Entry(detalle_horario).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalle_horarioExists(id))
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

        // POST: api/detalle_horario
        [ResponseType(typeof(detalle_horario))]
        public async Task<IHttpActionResult> Postdetalle_horario(detalle_horario detalle_horario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.detalle_horario.Add(detalle_horario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = detalle_horario.id }, detalle_horario);
        }

        // DELETE: api/detalle_horario/5
        [ResponseType(typeof(detalle_horario))]
        public async Task<IHttpActionResult> Deletedetalle_horario(int id)
        {
            detalle_horario detalle_horario = await db.detalle_horario.FindAsync(id);
            if (detalle_horario == null)
            {
                return NotFound();
            }

            db.detalle_horario.Remove(detalle_horario);
            await db.SaveChangesAsync();

            return Ok(detalle_horario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool detalle_horarioExists(int id)
        {
            return db.detalle_horario.Count(e => e.id == id) > 0;
        }
    }
}