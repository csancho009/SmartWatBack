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
    [RoutePrefix("api/tarifas")]
    public class TarifasController : ApiController
    {
        [HttpGet]
        [Route("ObtenerTarifa")]
        public Respuesta ObtenerTarifa(string BaseDeDatos)
        {
            Respuesta R = new Respuesta();

            try
            {
                TarifaController tarifa = new TarifaController();
                R.Codigo = 1;
                R.Nombre = "Ok";
                R.Detalle = tarifa.ObtenerTarifas(BaseDeDatos);
            } catch (Exception ex)
            {
                R.Codigo = -1;
                R.Nombre = "Alerta, la conexion fallo";
            }
            return R;

        }

        [HttpPut]
        [Route("ActualizarTarifa")]
        public Respuesta ActualizarTarifa([FromBody] List<TARIFAS> tarifas, string BaseDeDatos)
        {
            Respuesta R = new Respuesta();

            try
            {
                TarifaController tarifa = new TarifaController();

                R.Codigo = 1;
                R.Nombre = "Ok";
                R.Detalle = tarifa.ActualizarTarifas(tarifas, BaseDeDatos);
            }
            catch(Exception ex)
            {
                R.Codigo = -1;
                R.Nombre = "Alerta, la conexion fallo";
                
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
