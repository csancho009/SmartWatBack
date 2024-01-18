using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogicaSmartWat;
using LogicaSmartWat.Controllers;

namespace SmartWatBack.Controllers
{
    [Authorize]
    [RoutePrefix("api/Clientes")]
    public class ClientesController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id, string BDCia)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.UnCliente(id, BDCia));
        }
        [HttpPost]
        public IHttpActionResult ListaClientes([FromBody] ParametroBusquedaCLiente P)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.ListaClientes(P));
        }
        [HttpGet]
        public IHttpActionResult ParametrosInicialesCarga(string BD)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.ParametrosInicialesCarga(BD));
        }

        [HttpPost]
        public IHttpActionResult NuevoCliente([FromBody] CLIENTES Cliente, string BaseDeDatos)
        {
            GestionClientes C = new GestionClientes();
            return Ok(C.NuevoCliente(Cliente, BaseDeDatos));
        }
    }
}
