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
    [RoutePrefix("api/pajas")]
    public class PajasController : ApiController
    {

        [HttpGet]
        public IHttpActionResult ObtenerPajas(int Paja, string BDCia)
        {
            PajaController pajaC = new PajaController();
            return Ok(pajaC.ObtenerPajas(Paja, BDCia));
        }

        [HttpGet]
        public IHttpActionResult BUSCARPAJAPARAMETROS(string paja, string Cedula, string Nombre, string BDCia, string Medidor)
        {
            PajaController pajaC = new PajaController();
            return Ok(pajaC.BUSCAR_PAJA_PARAMETROS(paja, Cedula,Nombre,BDCia, Medidor));
        }

        [HttpPost]
        public IHttpActionResult IngresarPajas([FromBody]PAJAS paja, string BDCia)
        {
            PajaController pajaC = new PajaController();
            return Ok(pajaC.IngresarPaja(paja, BDCia));
        }
    }
}
