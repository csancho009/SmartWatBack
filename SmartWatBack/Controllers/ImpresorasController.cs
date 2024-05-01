using LogicaSmartWat.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartWatBack.Controllers
{
    public class ImpresorasController : ApiController
    {
        [Route("api/Impresoras/CierreCaja")]
        [HttpGet]
        public IHttpActionResult CierreCaja(int Cierre, string BDCia)
        {
            Impresiones I = new Impresiones();
            return Ok(I.CierreCaja(Cierre, BDCia));
        }
    }
}
