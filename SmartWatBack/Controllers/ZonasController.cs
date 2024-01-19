using LogicaSmartWat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Http;

namespace SmartWatBack.Controllers
{

    [Authorize]
    [RoutePrefix("api/zonas")]
    public class ZonasController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ObtenerZonas()
        {
            ZonaController zona = new ZonaController();
            return Ok(zona.ObtenerZonas());
        }

        [HttpPost]
        public IHttpActionResult IngresarZonas(ZONAS zona)
        {
            ZonaController zonas = new ZonaController();
            return Ok(zonas.IngresarZonas(zona));
        }
    }
}
