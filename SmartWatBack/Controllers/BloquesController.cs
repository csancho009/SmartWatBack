using LogicaSmartWat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartWatBack.Controllers
{

    [Authorize]
    [RoutePrefix("api/bloques")]
    public class BloquesController : ApiController
    {

        [HttpGet]
        [Route("ObtenerBloques")]
        public Respuesta ObtenerBloques()
        {
            Respuesta R = new Respuesta();
            try
            {
                BloqueController bloques = new BloqueController();
                R.Codigo = 1;
                R.Nombre = "Ok";
                R.Detalle = bloques.ObtenerBloques();
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Nombre = "Alerta, la conexion fallo";
            }
            return R;
        }

        [HttpPost]
        [Route("IngresarBloques")]
        public Respuesta IngresarBloques(BLOQUES bloque)
        {
            Respuesta R = new Respuesta();
            try
            {
                BloqueController bloques = new BloqueController();
                R.Codigo = 1;
                R.Nombre = "Ok";
                R.Detalle = bloques.IngresarBloques(bloque);
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Nombre = "Alerta, la conexion fallo";
            }
            return R;
        }

        public class Respuesta
        {
            public int Codigo { get; set; }
            public string Nombre {get; set; }
            public object Detalle = new Object();
        }
    }
}
