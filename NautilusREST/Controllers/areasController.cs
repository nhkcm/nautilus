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
    public class areasController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/areas
        public IEnumerable<dynamic> Getarea()
        {                        
            //var data = db.area_with_count_asignaturas.ToList().Select(x => {
            //    return new Models.DTO.Area()
            //    {
            //        id = x.id,
            //        fecha_registro = x.fecha_registro,
            //        nombre = x.nombre,
            //        num_asignaturas = x.num_materias
            //    };
            //}).ToList();
            return db.area.Select(x => new {
                x.id,
                x.nombre,
                x.fecha_registro
            });
        }

        // GET: api/areas/5
        [ResponseType(typeof(area))]
        public async Task<IHttpActionResult> Getarea(int id)
        {
            area area = await db.area.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }

            return Ok(area);
        }

        // PUT: api/areas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putarea(int id, area area)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != area.id)
            {
                return BadRequest();
            }

            db.Entry(area).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!areaExists(id))
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

        // POST: api/areas
        [ResponseType(typeof(area))]
        public async Task<IHttpActionResult> Postarea(area area)
        {
            area.fecha_registro = DateTime.Now.AddHours(2);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.area.Add(area);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = area.id }, area);
        }

        // DELETE: api/areas/5
        [ResponseType(typeof(area))]
        public async Task<IHttpActionResult> Deletearea(int id)
        {
            area area = await db.area.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }

            db.area.Remove(area);
            await db.SaveChangesAsync();

            return Ok(area);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool areaExists(int id)
        {
            return db.area.Count(e => e.id == id) > 0;
        }
    }
}