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
using NautilusREST.Models.DTO;

namespace NautilusREST.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class profesoresController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/profesores
        public List<Profesor> Getprofesor()
        {
            var query = db.profesor.ToList().Select(x => { return new Profesor() {
                id = x.id,
                documento = x.documento,
                nombre = x.nombre,
                email = x.email,
                direccion  = x.direccion,
                estado = x.estado,
                telefono = x.telefono,
                usuario_id = x.usuario_id
            }; }).ToList();
            return query;        
        }

        // GET: api/profesores/5
        [ResponseType(typeof(profesor))]
        public async Task<IHttpActionResult> Getprofesor(int id)
        {
            profesor profesor = await db.profesor.FindAsync(id);
            if (profesor == null)
            {
                return NotFound();
            }

            return Ok(profesor);
        }

        // PUT: api/profesores/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putprofesor(int id, profesor profesor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profesor.id)
            {
                return BadRequest();
            }

            db.Entry(profesor).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!profesorExists(id))
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

        // POST: api/profesores
        [ResponseType(typeof(profesor))]
        public async Task<IHttpActionResult> Postprofesor(profesor profesor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.profesor.Add(profesor);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = profesor.id }, profesor);
        }

        // DELETE: api/profesores/5
        [ResponseType(typeof(profesor))]
        public async Task<IHttpActionResult> Deleteprofesor(int id)
        {
            profesor profesor = await db.profesor.FindAsync(id);
            if (profesor == null)
            {
                return NotFound();
            }

            db.profesor.Remove(profesor);
            await db.SaveChangesAsync();

            return Ok(profesor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool profesorExists(int id)
        {
            return db.profesor.Count(e => e.id == id) > 0;
        }
    }
}