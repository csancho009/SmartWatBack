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
    [RoutePrefix("api/zonas")]
    public class ZonasController : ApiController
    {
        [HttpGet]
        [Route("ObtenerZonas")]
        public Respuesta ObtenerZonas()
        {
            Respuesta R = new Respuesta();
            try
            {
                ZonaController zona = new ZonaController();
                R.Codigo = 1;
                R.Nombre = "Ok";
                R.Detalle = zona.ObtenerZonas();
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Nombre = "Alerta, la conexion fallo" + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        [HttpPost]
        [Route("IngresarZonas")]
        public Respuesta IngresarZonas(ZONAS zona)
        {
            Respuesta R = new Respuesta();
            try
            {
                ZonaController zonas = new ZonaController();
                R.Codigo = 1;
                R.Nombre = "Ok";
                R.Detalle = zonas.IngresarZonas(zona);
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Nombre = "Alerta, la conexion fallo" + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public class Respuesta
        {
            public int Codigo { get; set; }
            public string Nombre { get; set; }
            public object Detalle = new object();
        }
    }
}
