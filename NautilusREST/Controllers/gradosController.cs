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
    public class gradosController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/grados
        public dynamic Getgrado()
        {
            var data = db.grado.ToList().Select(x => { return new {
                x.id,
                x.nombre                
            }; });
            return data;
        }

        // GET: api/grados/5
        [ResponseType(typeof(grado))]
        public dynamic Getgrado(int id)
        {
            try
            {
                grado grado = db.grado.Find(id);
                if (grado == null)
                {
                    return NotFound();
                }

                var sql = @"select * from asignatura 
                                 join asignatura_grado on asignatura_grado.asignatura_id = asignatura.id
                                 where asignatura_grado.grado_id = " + grado.id;

                var asignaturas = db.asignatura.SqlQuery(sql).ToList().Select(x =>
                {
                    return new
                    {
                        x.id,
                        x.nombre,
                        x.fecha_registro
                    };
                }).ToList();

                grado.asignatura_grado = null;
                var response = new
                {
                    grado = new { grado.id,grado.nombre },
                    asignaturas = asignaturas
                };
                //string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                return Request.CreateResponse(HttpStatusCode.OK,response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);                
            }
        }

        // PUT: api/grados/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putgrado(int id, grado grado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grado.id)
            {
                return BadRequest();
            }

            db.Entry(grado).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!gradoExists(id))
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

        // POST: api/grados
        [ResponseType(typeof(grado))]
        [Route("api/grados/{id}/asignar")]
        public async Task<HttpResponseMessage> Postgrado(int id)
        {
            try
            {
                var _data = await Request.Content.ReadAsStringAsync();
                var _asignaturas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<asignatura>>(_data);
                var _lista = new List<asignatura_grado>();

                foreach (var item in _asignaturas)
                {
                    var _asignatura_grado = new asignatura_grado();
                    _asignatura_grado.asignatura_id = item.id;
                    _asignatura_grado.grado_id = id;
                    _lista.Add(_asignatura_grado);
                }

                db.asignatura_grado.AddRange(_lista);
                db.SaveChanges();                
                return Request.CreateResponse(HttpStatusCode.OK,"Asignaturas guardadas exitosamente " + _asignaturas.Count);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }

        

        // DELETE: api/grados/5
        [ResponseType(typeof(grado))]
        public async Task<IHttpActionResult> Deletegrado(int id)
        {
            grado grado = await db.grado.FindAsync(id);
            if (grado == null)
            {
                return NotFound();
            }

            db.grado.Remove(grado);
            await db.SaveChangesAsync();

            return Ok(grado);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool gradoExists(int id)
        {
            return db.grado.Count(e => e.id == id) > 0;
        }
    }
}