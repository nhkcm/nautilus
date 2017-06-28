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
    public class cursosController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/cursos
        public IQueryable<curso> Getcurso()
        {
            return db.curso;
        }

        // GET: api/cursos/5
        [ResponseType(typeof(curso))]
        public async Task<IHttpActionResult> Getcurso(int id)
        {
            curso curso = await db.curso.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            return Ok(curso);
        }

        // PUT: api/cursos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putcurso(int id, curso curso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != curso.id)
            {
                return BadRequest();
            }

            db.Entry(curso).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cursoExists(id))
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

        // POST: api/cursos
        [ResponseType(typeof(curso))]
        public async Task<IHttpActionResult> Postcurso(curso curso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.curso.Add(curso);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = curso.id }, curso);
        }

        // DELETE: api/cursos/5
        [ResponseType(typeof(curso))]
        public async Task<IHttpActionResult> Deletecurso(int id)
        {
            curso curso = await db.curso.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            db.curso.Remove(curso);
            await db.SaveChangesAsync();

            return Ok(curso);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool cursoExists(int id)
        {
            return db.curso.Count(e => e.id == id) > 0;
        }
    }
}