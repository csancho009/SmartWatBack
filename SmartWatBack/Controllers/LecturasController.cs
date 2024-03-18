using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using LogicaSmartWat;
using LogicaSmartWat.Controllers;

namespace SmartWatBack.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Authorize]
    public class LecturasController : ApiController
    {
        [Route("api/Lecturas/TablaLecturas")]
        [HttpGet]
        public IHttpActionResult TablaLecturas(int mes, int annio, string BDCia)
        {
            Lecturas L = new Lecturas();
            return Ok(L.TablaLecturas(mes,annio,BDCia));
        }

        [Route("api/Lecturas/IniciaDatos")]
        [HttpGet]
        public IHttpActionResult IniciaDatos()
        {
            DatosIniciales D = new DatosIniciales();
            D.mes = DateTime.Now.Month.ToString();
            D.annio = DateTime.Now.Year.ToString();
            return Ok(D);
        }

        [Route("api/Lecturas/IngresaLectura")]
        [HttpPost]
        public IHttpActionResult IngresaLectura([FromBody]LECTURAS Gestor, string BDCia)
        {
            Lecturas L = new Lecturas();
            return Ok(L.IngresaLectura(Gestor, BDCia));
        }
        [Route("api/Lecturas/ActualizaLectura")]
        [HttpGet]
        public IHttpActionResult ActualizaLectura(int NumLectura, int Consumo, int usuario, string BDCia)
        {
            Lecturas L = new Lecturas();
            return Ok(L.ActualizaLectura( NumLectura,  Consumo,usuario,  BDCia));
        }
        [Route("api/Lecturas/GenerarCobro")]
        [HttpGet]
        public IHttpActionResult GenerarCobro(int Usuario, string BDCia)
        {
            Lecturas L = new Lecturas();
            return Ok(L.GenerarCobro(Usuario, BDCia));
        }

        public class DatosIniciales
        {
         
            public string mes { get; set; }
            public string annio { get; set; }
        }
    }
}
