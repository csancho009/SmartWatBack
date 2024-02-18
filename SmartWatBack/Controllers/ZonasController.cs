using LogicaSmartWat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SmartWatBack.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Authorize]
    [RoutePrefix("api/zonas")]
    public class ZonasController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ObtenerZonas(string BDCia)
        {
            ZonaController zona = new ZonaController();
            return Ok(zona.ObtenerZonas(BDCia));
        }

        [HttpPost]
        public IHttpActionResult IngresarZonas([FromBody] ZONAS zona, string BDCia)
        {
            ZonaController zonas = new ZonaController();
            return Ok(zonas.IngresarZonas(zona, BDCia));
        }
    }
}
