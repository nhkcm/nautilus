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
    public class estudiantesController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/estudiantes
        public IQueryable<estudiante> Getestudiante()
        {
            return db.estudiante;
        }

        // GET: api/estudiantes/5
        [ResponseType(typeof(estudiante))]
        public async Task<IHttpActionResult> Getestudiante(int id)
        {
            estudiante estudiante = await db.estudiante.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return Ok(estudiante);
        }
        
        [HttpGet]
        [Route("api/estudiantes")]
        public HttpResponseMessage Getestudiante(string documento, string nombre)
        {
            var data = db.estudiante.Where(x => x.documento.Contains(documento) || x.nombre.Contains(nombre)).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        // PUT: api/estudiantes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putestudiante(int id, estudiante estudiante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != estudiante.id)
            {
                return BadRequest();
            }

            db.Entry(estudiante).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!estudianteExists(id))
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

        // POST: api/estudiantes
        [ResponseType(typeof(estudiante))]
        public async Task<IHttpActionResult> Postestudiante(estudiante estudiante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.estudiante.Add(estudiante);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = estudiante.id }, estudiante);
        }

        // DELETE: api/estudiantes/5
        [ResponseType(typeof(estudiante))]
        public async Task<IHttpActionResult> Deleteestudiante(int id)
        {
            estudiante estudiante = await db.estudiante.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }

            db.estudiante.Remove(estudiante);
            await db.SaveChangesAsync();

            return Ok(estudiante);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool estudianteExists(int id)
        {
            return db.estudiante.Count(e => e.id == id) > 0;
        }
    }
}