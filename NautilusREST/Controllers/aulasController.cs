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
    public class aulasController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/aulas
        public IQueryable<aula> Getaula()
        {
            return db.aula;
        }

        // GET: api/aulas/5
        [ResponseType(typeof(aula))]
        public async Task<IHttpActionResult> Getaula(int id)
        {
            aula aula = await db.aula.FindAsync(id);
            if (aula == null)
            {
                return NotFound();
            }

            return Ok(aula);
        }

        // PUT: api/aulas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putaula(int id, aula aula)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aula.id)
            {
                return BadRequest();
            }

            db.Entry(aula).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!aulaExists(id))
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

        // POST: api/aulas
        [ResponseType(typeof(aula))]
        public async Task<IHttpActionResult> Postaula(aula aula)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.aula.Add(aula);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = aula.id }, aula);
        }

        // DELETE: api/aulas/5
        [ResponseType(typeof(aula))]
        public async Task<IHttpActionResult> Deleteaula(int id)
        {
            aula aula = await db.aula.FindAsync(id);
            if (aula == null)
            {
                return NotFound();
            }

            db.aula.Remove(aula);
            await db.SaveChangesAsync();

            return Ok(aula);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool aulaExists(int id)
        {
            return db.aula.Count(e => e.id == id) > 0;
        }
    }
}