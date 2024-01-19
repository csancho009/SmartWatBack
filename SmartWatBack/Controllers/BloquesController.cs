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
        public IHttpActionResult ObtenerBloques(int id_zon)
        {
            BloqueController bloques = new BloqueController();
            return Ok(bloques.ObtenerBloques(id_zon));
        }

        [HttpPost]
        public IHttpActionResult IngresarBloques(BLOQUES bloque)
        {
            BloqueController bloques = new BloqueController();
            return Ok(bloques.IngresarBloques(bloque));
        }

      
    }
}
