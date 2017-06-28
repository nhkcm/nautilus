using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NautilusREST.Models.DTO
{
    public class Area
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public System.DateTime fecha_registro { get; set; }
        public long num_asignaturas { get; set; }
    }
}