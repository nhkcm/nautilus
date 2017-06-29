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

        [Route("api/estudiantes/{documento}")]
        [HttpGet]
        public HttpResponseMessage Getestudiante(string documento)
        {
            estudiante estudiante = db.estudiante.Where(x => x.documento == documento).FirstOrDefault();
            if (estudiante == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, estudiante);
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

        
        public HttpResponseMessage Postestudiante(estudiante estudiante)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.estudiante.Add(estudiante);
            db.SaveChanges();


            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                estudiante.id,
                estudiante.nombre,
                estudiante.apellido,
                estudiante.documento,
                estudiante.sexo,
                estudiante.telefono,
                estudiante.fecha_nacimiento,
                estudiante.direccion,
                estudiante.email,
                estudiante.estado
            });
        }

        // POST: api/estudiantes
        [Route("api/estudiantes/{documento}/matricular/{curso}")]
        public HttpResponseMessage Postestudiante(string documento, int curso)
        {
            var estudiante = db.estudiante.Where(x => x.documento == documento).FirstOrDefault();
            var horarios = db.horarios.Where(x => x.curso_id == curso).ToList();

            if (estudiante == null) {
                return Request.CreateResponse(HttpStatusCode.NotFound, "estudante no encontrado!");
            }

            if (horarios == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "problemas para encotrar horarios validos");
            }

            if (horarios.Count() < 1)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "horarios no disponibles");
            }

            foreach (var item in horarios)
            {
                notas n = new notas()
                {
                    estudiante_id = estudiante.id,
                    horarios_id = item.id,
                    n1 = 0,
                    n2 = 0,
                    n3 = 0,
                    n4 = 0                    
                };
                db.notas.Add(n);
            }

            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
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