using LogicaSmartWat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SmartWatBack.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Authorize]
 
    public class TarifasController : ApiController
    {
        [Route("api/tarifas/ObtenerTarifa")]
        [HttpGet]
        public IHttpActionResult ObtenerTarifa(string BaseDeDatos)
        {
            TarifaController tarifa = new TarifaController();
            return Ok(tarifa.ObtenerTarifas(BaseDeDatos));

        }
        [Route("api/tarifas/ObtenerPrecioTarifa")]
        [HttpGet]
        public IHttpActionResult ObtenerPrecioTarifa(string BaseDeDatos, string CodigoTarifa)
        {
            TarifaController tarifa = new TarifaController();
            return Ok(tarifa.ObtenerPrecioTarifa(BaseDeDatos, CodigoTarifa));

        }
        [Route("api/tarifas/ActualizarTarifa")]
        [HttpPut]
        public IHttpActionResult ActualizarTarifa([FromBody] List<TARIFAS> tarifas, string BaseDeDatos)
        {
            TarifaController tarifa = new TarifaController();
            return Ok(tarifa.ActualizarTarifas(tarifas, BaseDeDatos));
        }

    }
}
