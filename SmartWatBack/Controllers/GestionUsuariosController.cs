using LogicaSmartWat;
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
    public class GestionUsuariosController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/GestionUsuarios/ListaUsuarios")]
        public IHttpActionResult ListaUsuarios(string Usuario, string Nombre, string BDCia)
        {
            UsuariosBD U = new UsuariosBD();
            return Ok(U.ListaUsuarios(Usuario,Nombre, BDCia));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/GestionUsuarios/RepositorioUsuario")]
        public IHttpActionResult RepositorioUsuario(int Codigo, string BDCia)
        {
            UsuariosBD U = new UsuariosBD();
            return Ok(U.RepositorioUsuario(Codigo, BDCia));
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/GestionUsuarios/GuardarUsuario")]
        public IHttpActionResult GuardarUsuario([FromBody] USUARIOS Usr, string BDCia, int MiIdUsuario)
        {
            UsuariosBD U = new UsuariosBD();
            return Ok(U.GuardarUsuario(Usr, BDCia, MiIdUsuario));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/GestionUsuarios/ListaRoles")]
        public IHttpActionResult ListaRoles(string BDCia)
        {
            UsuariosBD U = new UsuariosBD();
            return Ok(U.ListaRoles(BDCia));
        }
    }
}
