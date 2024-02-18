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
    [RoutePrefix("api/bloques")]
    public class BloquesController : ApiController
    {

        [HttpGet]
        public IHttpActionResult ObtenerBloques(int id_zon, string BDCia)
        {
            BloqueController bloques = new BloqueController();
            return Ok(bloques.ObtenerBloques(id_zon, BDCia));
        }

        [HttpPost]
        public IHttpActionResult IngresarBloques([FromBody] BLOQUES bloque, string BDCia)
        {
            BloqueController bloques = new BloqueController();
            return Ok(bloques.IngresarBloques(bloque, BDCia));
        }

      
    }
}
