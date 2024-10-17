using LogicaSmartWat.Controllers;
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
    public class GestionBodegasController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/GestionBodegas/ListaBodegas")]
        public IHttpActionResult ListaBodegas(string BDCia)
        {
            BodegasBD U = new BodegasBD();
            return Ok(U.ListaBodegas(BDCia));
        }
    }
}
