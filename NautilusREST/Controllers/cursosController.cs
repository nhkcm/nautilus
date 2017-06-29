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
        public IQueryable<dynamic> Getcurso()
        {
            return db.curso.Select(x => new {
                x.id,
                x.nombre,
                grado_nombre = x.grado.nombre,
                x.periodo,
                estado = (x.estado == 0) ? "Inactivo" : "Activo"
            });
        }

        [Route("api/cursos/horarios/{id}")]
        public IEnumerable<dynamic> gethorarios(int id)
        {
            //var curso = db.curso.Find(id);

            //if (curso == null) return null;

            //var horarios = curso.horarios.Select(x => new {
            //    x.id,

            //    asignatura_id = x.asignatura_id,
            //    asignatura_nombre = x.asignatura.nombre,

            //    curso_id = x.curso_id,
            //    curso_nombre = x.curso.nombre,

            //    profesor_id = x.profesor_id,
            //    profesor_nombre = x.profesor.nombre,
            //});

            //mejorar perfomance
            var horarios = (from _horario in db.horarios
                            join _profesor in db.profesor on _horario.profesor_id equals _profesor.id
                            join _asignatura in db.asignatura on _horario.asignatura_id equals _asignatura.id
                            where _horario.curso_id == id
                            select new {
                                id = _horario.id,
                                profesor_id = _profesor.id,
                                profesor_nombre = $"{_profesor.nombre} {_profesor.apellido}",
                                asignatura_id = _asignatura.id,
                                asignatura_nombre = _asignatura.nombre
                            });


            return horarios;
        }



        // GET: api/cursos/5       
        public async Task<IHttpActionResult> Getcurso(int id)
        {
            curso curso = await db.curso.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            return Ok(new {
                curso.id,
                curso.nombre,
                grado_nombre = curso.grado.nombre,
                curso.periodo,
                estado = (curso.estado == 0) ? "Inactivo" : "Activo"                
            });
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