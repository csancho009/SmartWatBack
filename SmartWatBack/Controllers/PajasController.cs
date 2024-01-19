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
    [RoutePrefix("api/pajas")]
    public class PajasController : ApiController
    {

        [HttpGet]
        public IHttpActionResult ObtenerPajas(string buscador)
        {
            PajaController pajaC = new PajaController();
            return Ok(pajaC.ObtenerPajas(buscador));
        }

        [HttpPost]
        public IHttpActionResult IngresarPajas(PAJAS paja)
        {
            PajaController pajaC = new PajaController();
            return Ok(pajaC.IngresarPaja(paja));
        }
    }
}
