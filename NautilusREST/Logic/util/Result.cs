using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NautilusREST.Logic.util
{
    public class Result
    {
        public CodigoRespuesta codigoRespuesta { set; get; }
        public string Mensaje { set; get; }        

        public bool isOk { get { return (codigoRespuesta == CodigoRespuesta.OK); } }
        public bool isFail { get { return (codigoRespuesta == CodigoRespuesta.Fail); } }
        public bool isError { get { return (codigoRespuesta == CodigoRespuesta.Error); } }
        public bool isBad { get { return (codigoRespuesta != CodigoRespuesta.OK); } }

        public static Result OK() { return new Result(CodigoRespuesta.OK); }
        public static Result Fail() { return new Result(CodigoRespuesta.Fail); }
        public static Result Error() { return new Result(CodigoRespuesta.Error); }

        public static Result OK(string str) { return new Result(CodigoRespuesta.OK) { Mensaje = str }; }
        public static Result Fail(string str) { return new Result(CodigoRespuesta.Fail) { Mensaje = str }; }
        public static Result Error(string str) { return new Result(CodigoRespuesta.Error) { Mensaje = str }; }

        public enum CodigoRespuesta : int { OK, Fail, Error }

        public Result()
        {

        }
        public Result(CodigoRespuesta tipo)
        {
            switch (tipo)
            {
                case CodigoRespuesta.OK:
                    this.codigoRespuesta = CodigoRespuesta.OK;
                    break;
                case CodigoRespuesta.Error:
                    this.codigoRespuesta = CodigoRespuesta.Error;
                    break;
                case CodigoRespuesta.Fail:
                    this.codigoRespuesta = CodigoRespuesta.Fail;
                    break;
                default:
                    break;
            }
        }        
    }
}