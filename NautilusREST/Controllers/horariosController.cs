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
    public class horariosController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/horarios
        public IQueryable<horarios> Gethorarios()
        {
            return db.horarios;
        }

        // GET: api/horarios/5
        [ResponseType(typeof(horarios))]
        public async Task<IHttpActionResult> Gethorarios(int id)
        {
            horarios horarios = await db.horarios.FindAsync(id);
            if (horarios == null)
            {
                return NotFound();
            }

            return Ok(horarios);
        }

        // PUT: api/horarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puthorarios(int id, horarios horarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != horarios.id)
            {
                return BadRequest();
            }

            db.Entry(horarios).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!horariosExists(id))
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

        // POST: api/horarios
        [ResponseType(typeof(horarios))]
        public async Task<IHttpActionResult> Posthorarios(horarios horarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.horarios.Add(horarios);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = horarios.id }, horarios);
        }

        // DELETE: api/horarios/5
        [ResponseType(typeof(horarios))]
        public async Task<IHttpActionResult> Deletehorarios(int id)
        {
            horarios horarios = await db.horarios.FindAsync(id);
            if (horarios == null)
            {
                return NotFound();
            }

            db.horarios.Remove(horarios);
            await db.SaveChangesAsync();

            return Ok(horarios);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool horariosExists(int id)
        {
            return db.horarios.Count(e => e.id == id) > 0;
        }
    }
}