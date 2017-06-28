using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NautilusREST.Logic.util
{
    public class ResultObject<T> : Result
    {
        public T Content { set; get; }

        public ResultObject() { }

        public ResultObject(string msg, T content,CodigoRespuesta respuesta)
        {
            Mensaje = msg;
            codigoRespuesta = respuesta;
            Content = content;           
        }

        public static ResultObject<T> OK(string msg, T content, Type tipo)
        {
            return new ResultObject<T>(msg, content, CodigoRespuesta.OK);
        }

        public static ResultObject<T> Error(string msg, T content, Type tipo)
        {
            return new ResultObject<T>(msg, content, CodigoRespuesta.OK);
        }

        public static ResultObject<T> Fail(string msg, T content, Type tipo)
        {
            return new ResultObject<T>(msg, content, CodigoRespuesta.OK);
        }
    }
}