﻿using System;
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
using System.Dynamic;

namespace NautilusREST.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class profesoresController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();

        // GET: api/profesores
        public IEnumerable<dynamic> Getprofesor()
        {            
            return db.profesor.Select(x => new {                
                x.id,
                x.nombre,
                x.sexo,
                x.telefono,
                x.apellido,
                x.direccion,
                x.documento,
                x.email,
                x.estado                
            });        
        }

        // GET: api/profesores/5        
        public async Task<IHttpActionResult> Getprofesor(int id)
        {
            profesor profesor = await db.profesor.FindAsync(id);
            if (profesor == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                profesor.id,
                profesor.nombre,
                profesor.sexo,
                profesor.telefono,
                profesor.apellido,
                profesor.direccion,
                profesor.documento,
                profesor.email,
                profesor.estado                
            });
        }

        [Route("api/profesores/{id}/horarios")]
        public async Task<IHttpActionResult> GetprofesorHorarios(int id)
        {
            profesor profesor = await db.profesor.FindAsync(id);
            if (profesor == null)
            {
                return NotFound();
            }



            return Ok(profesor.horarios.Select(x => new {
                x.id,
                asignatura_nombre = x.asignatura.nombre,
                curso_nombre = x.curso.nombre
            }));
        }

        // PUT: api/profesores/5
        [Route("api/profesores/{id}")]        
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