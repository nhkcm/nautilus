using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NautilusREST.Models.DTO
{
    public class Profesor
    {
        public int id { get; set; }
        public string documento { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string email { get; set; }
        public Nullable<int> usuario_id { get; set; }
        public int estado { get; set; }
    }
}