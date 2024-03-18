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
    //[Route("api/Clientes")]    
    
    public class ClientesController : ApiController
    {
        [Route("api/Clientes/GetId")]
        [HttpGet]        
        public IHttpActionResult GetId(int id, string BDCia)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.UnCliente(id, BDCia));
        }

        [Route("api/Clientes/ListaClientes")]
        [HttpPost]
        public IHttpActionResult ListaClientes([FromBody] ParametroBusquedaCLiente P)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.ListaClientes(P));
        }

        [Route("api/Clientes/ParametrosInicialesCarga")]
        [HttpGet]
        public IHttpActionResult ParametrosInicialesCarga(string BD)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.ParametrosInicialesCarga(BD));
        }

        [Route("api/Clientes/ConsultaCedula")]
        [HttpGet]
        public IHttpActionResult ConsultaCedula(string Dato, string BDCia)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.ConsultaCedula(Dato, BDCia));
        }

        [Route("api/Clientes/ListaGeneralClientes")]
        [HttpGet]
        public IHttpActionResult ListaGeneralClientes(string BDCia)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.ListaGeneralClientes( BDCia));
        }

        [Route("api/Clientes/Cantones")]
        [HttpGet]
        public IHttpActionResult Cantones(string BD, int Provincia)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.Cantones(BD, Provincia));
        }

        [Route("api/Clientes/Distritos")]
        [HttpGet]
        public IHttpActionResult Distritos(string BD, int Provincia, int Canton)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.Distritos(BD, Provincia, Canton));
        }

        [Route("api/Clientes/NuevoCliente")]
        [HttpPost]
        public IHttpActionResult NuevoCliente([FromBody] CLIENTES Cliente, string BaseDeDatos)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.NuevoCliente(Cliente, BaseDeDatos));
        }
        [Route("api/Clientes/ActualizaCliente")]
        [HttpPost]
        public IHttpActionResult ActualizaCliente([FromBody] CLIENTES Cliente, string BaseDeDatos)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.ActualizaCliente(Cliente, BaseDeDatos));
        }

        [Route("api/Clientes/GetCliente")]
        [HttpGet]
        public IHttpActionResult GetCliente(string nombre)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.GetCliente(nombre));
        }
    }
}
