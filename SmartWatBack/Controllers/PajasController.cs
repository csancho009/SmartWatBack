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
    
    public class PajasController : ApiController
    {
        [Route("api/Pajas/ObtenerPajas")]
        [HttpGet]
        public IHttpActionResult ObtenerPajas(int Paja, string BDCia)
        {
            PajaController pajaC = new PajaController();
            return Ok(pajaC.ObtenerPajas(Paja, BDCia));
        }
        [Route("api/Pajas/BUSCARPAJAPARAMETROS")]
        [HttpGet]
        public IHttpActionResult BUSCARPAJAPARAMETROS(string paja, string Cedula, string Nombre, string BDCia, string Medidor)
        {
            PajaController pajaC = new PajaController();
            return Ok(pajaC.BUSCAR_PAJA_PARAMETROS(paja, Cedula,Nombre,BDCia, Medidor));
        }
        [Route("api/Pajas/IngresarPajas")]
        [HttpPost]
        public IHttpActionResult IngresarPajas([FromBody]PAJAS paja, string BDCia)
        {
            PajaController pajaC = new PajaController();
            return Ok(pajaC.IngresarPaja(paja, BDCia));
        }
        [Route("api/Pajas/PendientesPago")]
        [HttpGet]
        public IHttpActionResult PendientesPago(string Nombre, string Cedula, string BDCia, int Estado)
        {
            PajaController pajaC = new PajaController();
            return Ok(pajaC.PendientesPago(Nombre, Cedula,BDCia, Estado));
        }

        [Route("api/Pajas/Datafonos")]
        [HttpGet]
        public IHttpActionResult Datafonos(string BDCia)
        {
            PajaController pajaC = new PajaController();
            return Ok(pajaC.Datafonos(BDCia));
        }
        [Route("api/Pajas/CuentasDeBanco")]
        [HttpGet]
        public IHttpActionResult CuentasDeBanco(string BDCia)
        {
            PajaController pajaC = new PajaController();
            return Ok(pajaC.CuentasDeBanco(BDCia));
        }
    }
}
