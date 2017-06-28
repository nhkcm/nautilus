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
using NautilusREST.Logic.util;

namespace NautilusREST.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class usuariosController : ApiController
    {
        private nautilus_entities db = new nautilus_entities();


        [Route("api/usuarios/login/{user}/{pass}")]
        public Result<usuario> Login(string user, string pass) {
            var store_user = db.usuario.Where(x => x.usuario1 == user && x.clave == pass).FirstOrDefault();
            store_user.clave = DateTime.Now.Ticks.ToString();

            if (store_user != null) return Result<usuario>.OK(store_user);
            else return Result<usuario>.Error("credentiales invalidas");                                       
        }

        // GET: api/usuarios
        public IQueryable<usuario> Getusuario()
        {
            return db.usuario;
        }

        // GET: api/usuarios/5
        [ResponseType(typeof(usuario))]
        public async Task<IHttpActionResult> Getusuario(int id)
        {
            usuario usuario = await db.usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/usuarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putusuario(int id, usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.id)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuarioExists(id))
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

        // POST: api/usuarios
        [ResponseType(typeof(usuario))]
        public async Task<IHttpActionResult> Postusuario(usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.usuario.Add(usuario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuario.id }, usuario);
        }

        // DELETE: api/usuarios/5
        [ResponseType(typeof(usuario))]
        public async Task<IHttpActionResult> Deleteusuario(int id)
        {
            usuario usuario = await db.usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.usuario.Remove(usuario);
            await db.SaveChangesAsync();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usuarioExists(int id)
        {
            return db.usuario.Count(e => e.id == id) > 0;
        }
    }
}