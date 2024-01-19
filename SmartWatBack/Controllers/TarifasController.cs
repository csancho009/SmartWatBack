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
        public IHttpActionResult ObtenerTarifa(string BaseDeDatos)
        {
            TarifaController tarifa = new TarifaController();
            return Ok(tarifa.ObtenerTarifas(BaseDeDatos));

        }

        [HttpPut]
        public IHttpActionResult ActualizarTarifa([FromBody] List<TARIFAS> tarifas, string BaseDeDatos)
        {
            TarifaController tarifa = new TarifaController();
            return Ok(tarifa.ActualizarTarifas(tarifas, BaseDeDatos));
        }

    }
}
