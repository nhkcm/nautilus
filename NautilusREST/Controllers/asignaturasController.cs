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
    public class asignaturasController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/asignaturas
        public dynamic Getasignatura()
        {
            var data = db.asignatura.ToList().Select(x => { return new {
                x.id,
                x.nombre                                
            }; }).ToList();
            return data;
        }

        // GET: api/asignaturas/5
        [ResponseType(typeof(asignatura))]
        public async Task<IHttpActionResult> Getasignatura(int id)
        {
            asignatura asignatura = await db.asignatura.FindAsync(id);
            if (asignatura == null)
            {
                return NotFound();
            }
            //asignatura.asignatura_grado = null;
            return Ok(asignatura);
        }

        // PUT: api/asignaturas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putasignatura(int id, asignatura asignatura)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != asignatura.id)
            {
                return BadRequest();
            }

            db.Entry(asignatura).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!asignaturaExists(id))
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

        // POST: api/asignaturas
        [ResponseType(typeof(asignatura))]
        public async Task<IHttpActionResult> Postasignatura(asignatura asignatura)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            asignatura.fecha_registro = DateTime.Now;
            db.asignatura.Add(asignatura);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = asignatura.id }, asignatura);
        }

        // DELETE: api/asignaturas/5
        [ResponseType(typeof(asignatura))]
        public async Task<IHttpActionResult> Deleteasignatura(int id)
        {
            asignatura asignatura = await db.asignatura.FindAsync(id);
            if (asignatura == null)
            {
                return NotFound();
            }

            db.asignatura.Remove(asignatura);
            await db.SaveChangesAsync();

            return Ok(asignatura);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool asignaturaExists(int id)
        {
            return db.asignatura.Count(e => e.id == id) > 0;
        }
    }
}