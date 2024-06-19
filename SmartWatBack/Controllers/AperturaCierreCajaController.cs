using LogicaSmartWat.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using static LogicaSmartWat.Controllers.Cajas;

namespace SmartWatBack.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Authorize]
    public class AperturaCierreCajaController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AperturaCierreCaja/ListaCierres")]
        public IHttpActionResult ListaCierres(string Codigo, string BDCia)
        {
            Cajas C = new Cajas();
            return Ok(C.ListaCierres(Codigo, BDCia));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AperturaCierreCaja/SaldosCaja")]
        public IHttpActionResult SaldosCaja(string Codigo, string BDCia)
        {
            Cajas C = new Cajas();
            return Ok(C.SaldosCaja(Codigo, BDCia));
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/AperturaCierreCaja/AperturaCierre")]
        public IHttpActionResult AperturaCierre([FromBody] DatosConteoAperturaCierre Conteo, string BDCia)
        {
            Cajas C = new Cajas();
            return Ok(C.AperturaCierre(Conteo, BDCia));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AperturaCierreCaja/ListaTiposMovTransac")]
        public IHttpActionResult ListaTiposMovTransac(string BDCia)
        {
            Cajas C = new Cajas();
            return Ok(C.ListaTiposMovTransac(BDCia));
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/AperturaCierreCaja/FnNuevoMovOperacion")]
        public IHttpActionResult FnNuevoMovOperacion([FromBody] ParamMovTransaccional Par, int PIN, string BDCia)
        {
            Cajas C = new Cajas();
            return Ok(C.FnNuevoMovOperacion(Par, PIN,BDCia));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AperturaCierreCaja/MovimientosEnCaja")]
        public IHttpActionResult MovimientosEnCaja(int CodUsuario, string BDCia)
        {
            Cajas C = new Cajas();
            return Ok(C.MoviemientosEnCaja(CodUsuario, BDCia));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AperturaCierreCaja/MovimientosCaja")]
        public IHttpActionResult MovimientosCaja(int usuario, string Tipo, int MiUsuario, string BDCia)
        {
            Impresiones I = new Impresiones();
            return Ok(I.MovimientosCaja(usuario,Tipo,MiUsuario, BDCia));
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AperturaCierreCaja/ImpresionMovTransac")]
        public IHttpActionResult ImpresionMovTransac(int Miusuario, DateTime Desde, DateTime Hasta, int Vendedor, string Salida, string BDCia)
        {
            Impresiones I = new Impresiones();
            return Ok(I.ImpresionMovTransac(Miusuario, Desde, Hasta, Vendedor, Salida, BDCia));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AperturaCierreCaja/RepositorioIngresosyGastos")]
        public IHttpActionResult RepositorioIngresosyGastos(int Usuario, DateTime Desde, DateTime Hasta, string BDCia)
        {
            Cajas C = new Cajas();
            return Ok(C.RepositorioIngresosyGastos( Usuario, Desde, Hasta, BDCia));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AperturaCierreCaja/Vendedores")]
        public IHttpActionResult Vendedores(string BDCia)
        {
            Cajas C = new Cajas();
            return Ok(C.Vendedores(BDCia));
        }
    }
}

