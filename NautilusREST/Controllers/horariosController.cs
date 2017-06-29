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
        public dynamic Posthorarios(horarios horarios)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            db.horarios.Add(horarios);
            db.SaveChanges();


            var horario = (from _horario in db.horarios
                           join _profesor in db.profesor on _horario.profesor_id equals _profesor.id
                           join _asignatura in db.asignatura on _horario.asignatura_id equals _asignatura.id
                           where _horario.id == horarios.id
                           select new
                           {
                               id = _horario.id,
                               profesor_id = _profesor.id,
                               profesor_nombre = _profesor.nombre + " " + _profesor.apellido,
                               asignatura_id = _asignatura.id,
                               asignatura_nombre = _asignatura.nombre
                           }).ToList().FirstOrDefault();

            return horario;
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