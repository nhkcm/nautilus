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

        #region utilidades

        private string getTipo(int tipo)
        {
            switch (tipo)
            {
                case 1: return "normal";
                case 2: return "audiovisual";
                case 3: return "deportiva";
                default: return "no definido";
            }
        }

        private string getEstado(int estado)
        {
            switch (estado) {
                case 0: return "Inactiva";
                case 1:return "Activa";
                case 2: return "Mantenimiento";
                default: return "no definido";
            }
        }

        #endregion        

        [Route("api/aulas/filter/{sede_id}")]
        public IEnumerable<dynamic> Getaulafilter(int sede_id)
        {
           

            var query = (from _aula in db.aula
                         join _sede in db.sede on _aula.sede_id equals _sede.id
                         where _aula.sede_id == sede_id
                         select new
                         {
                             _aula.id,
                             _aula.nombre,
                             nombre_sede = _sede.nombre,
                             _aula.capacidad,
                             _aula.tipo,
                             _aula.estado
                         }).ToList();

            var parse = query.Select(x => new
            {
                x.id,
                x.nombre,
                x.nombre_sede,
                x.capacidad,
                tipo = getTipo(x.tipo),
                estado = getEstado(x.estado)
            });

            return parse;
        }

        // GET: api/aulas
        public IEnumerable<dynamic> Getaula()
        {            

            var query = (from _aula in db.aula
                         join _sede in db.sede on _aula.sede_id equals _sede.id
                         select new
                         {
                             _aula.id,
                             _aula.nombre,
                             nombre_sede = _sede.nombre,
                             _aula.capacidad,
                             _aula.tipo,
                             _aula.estado
                         }).ToList();

            var parse = query.Select(x => new 
            {
                x.id,
                x.nombre,
                x.nombre_sede,
                x.capacidad,
                tipo = getTipo(x.tipo),
                estado = getEstado(x.estado)
            });

            return parse;
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