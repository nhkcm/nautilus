using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NautilusREST.Logic.util
{
    public class ResultJSON : Result
    {
        public String Content { set; get; }
        public Type ContentType { set; get; }

        public ResultJSON() { }

        public ResultJSON(string msg, string content, Type tipo, CodigoRespuesta respuesta)
        {
            Mensaje = msg;
            codigoRespuesta = respuesta;
            Content = content;
            ContentType = tipo;
        }

        public static ResultJSON OK(string msg, string content, Type tipo) {
            return new ResultJSON(msg,content,tipo,CodigoRespuesta.OK);
        }

        public static ResultJSON Error(string msg, string content, Type tipo){
            return new ResultJSON(msg, content, tipo, CodigoRespuesta.OK);
        }

        public static ResultJSON Fail(string msg, string content, Type tipo){
            return new ResultJSON(msg, content, tipo, CodigoRespuesta.OK);
        }
    }
}