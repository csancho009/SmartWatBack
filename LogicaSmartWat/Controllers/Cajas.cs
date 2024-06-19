using LogicaSmartWat.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat.Controllers
{
    public class Cajas
    {
        public Respuesta ListaCierres(string Codigo, string BDCia)
        {
            int CodUsr = int.Parse(Codigo);
            Respuesta M = new Respuesta();
            try
            {
                List<Respuesta> R = new List<Respuesta>();
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    var Cierres = from Cs in db.R_CIERRE.Where(d => d.USUARIO == CodUsr).OrderByDescending(o => o.CODIGO) select new { Cs.CODIGO, Cs.FECHA };
                    foreach (var C in Cierres)
                    {
                        R.Add(new Respuesta { Codigo = C.CODIGO, Mensaje = C.FECHA.ToString() });
                    }
                    M.Codigo = 0;
                    M.Mensaje = "OK";
                    M.Objeto = R;
                }
            }
            catch (Exception ex)
            {

                M.Codigo = -1;
                M.Mensaje = "Alerta Caja-ListaCierres NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) + " Detalle: " + ex.Message;
            }
            
               
            return M;
        }

        public Respuesta SaldosCaja(string Codigo, string BDCia)
        {
            RespuestaCaja R = new RespuestaCaja();
            Respuesta M = new Respuesta();
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    if (!string.IsNullOrEmpty(Codigo))
                    {
                        int CodiInt = int.Parse(Codigo);
                        //var C = from E in db.CIA select new { E.MONEDA_BAR };

                        USUARIOS usr = db.USUARIOS.Where(d => d.CODIGO == CodiInt).FirstOrDefault();
                        if (usr != null)
                        {
                            if (usr.CAJA == 0)
                            {
                                R.Codigo = 0;
                                R.Mensaje = "OK";
                                R.Efectivo = 0;
                                R.Tarjeta = 0;
                                R.Transfer = 0;
                                R.Cheque = 0;
                                R.Apertura = 0;
                                R.VerSaldo = "APE";
                            }
                            else
                            {
                                var SpCaja = db.PROCESO_CAJA(usr.CODIGO);
                                foreach (var L in SpCaja)
                                {
                                    R.Codigo = 0;
                                    R.Mensaje = "OK";
                                    R.Efectivo = (float)L.Efectivo;
                                    R.Tarjeta = (float)L.Tarjeta;
                                    R.Transfer = (float)L.Transfer;
                                    R.Cheque = (float)L.Cheque;
                                    R.Apertura = (float)L.Apertura;
                                    R.VerSaldo = L.VerSaldo;
                                    R.EfectivoDol = (float)L.EfectivoDol;
                                    R.TarjetaDol = (float)L.TarjetaDol;
                                    R.TransferDol = (float)L.TransferDol;
                                    R.ChequeDol = (float)L.ChequeDol;
                                    R.AperturaDol = (float)L.AperturaDol;
                                }
                            }
                            R.MonedaEmpresa = "CRC";
                            //foreach (var E in C)
                            //{
                            //    R.MonedaEmpresa = E.MONEDA_BAR;
                            //}
                            M.Codigo = 0;
                            M.Mensaje = "OK";
                            M.Objeto = R;
                        }
                    }
                    else
                    {
                        M.Codigo = -2;
                        M.Mensaje = "PIN Incorrecto";
                    }
                }
                    
            }
            catch (Exception ex)
            {
                M.Codigo = -1;
                M.Mensaje = ex.Message;
            }
            return M;
        }

        public Respuesta ListaTiposMovTransac(string BDCia)
        {
            
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    var Tss = from Ts in db.TRANSACCIONES select new { Ts.CODIGO, Nombre = Ts.NOMBRE + " (" + (Ts.TIPO == 1 ? "Aum +" : "Dism -") + ")" };
                    return new Respuesta { Codigo = 0, Mensaje = "OK", Objeto = Tss.ToList() };
                }
            }
            catch (Exception ex)
            {

                return new Respuesta { Codigo = -1, Mensaje = "ListaTiposMovTransac "+ ex.Message };
            }
                  
        }

        public Respuesta MoviemientosEnCaja(int CodUsuario, string BDCia)
        {
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    var Salida = db.MOVIMIENTOS_CAJA(CodUsuario);
                    return new Respuesta { Codigo = 0, Mensaje = "OK", Objeto = Salida.ToList() };
                }
            }
            catch (Exception ex)
            {

                return new Respuesta { Codigo = -1, Mensaje = "BarMovimientosEnCaja " + ex.Message };
            }
        }

        public Respuesta RepositorioIngresosyGastos(int Usuario, DateTime Desde, DateTime Hasta, string BDCia)
        {
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    var Salida = db.DESGLOSE_ENTRADAS_SALIDAS("DIA", Usuario, Desde, Hasta);
                    return new Respuesta { Codigo = 0, Mensaje = "OK", Objeto = Salida.ToList() };
                }
            }
            catch (Exception ex)
            {

                return new Respuesta { Codigo = -1, Mensaje = "RepositorioIngresosyGastos " + ex.Message };
            }
           
        }

        public Respuesta Vendedores(string BDCia)
        {
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    var B = from V in db.USUARIOS.Where(d => d.VENDEDOR == 1).OrderBy(o => o.NOMBRE) select new { V.CODIGO, V.NOMBRE };
                    List<Respuesta> R = new List<Respuesta>();
                    R.Add(new Respuesta { Codigo = 0, Mensaje = "Todos" });
                    foreach (var A in B)
                    {
                        R.Add(new Respuesta { Codigo = A.CODIGO, Mensaje = A.NOMBRE });
                    }
                    return new Respuesta { Codigo = 0, Mensaje = "OK", Objeto = R };
                }

            }
            catch (Exception ex)
            {

                return new Respuesta { Codigo = -1, Mensaje = "RepositorioIngresosyGastos " + ex.Message };
            }
        }


        public Respuesta FnNuevoMovOperacion(ParamMovTransaccional Par, int PIN, string BDCia)
        {
            Respuesta R = new Respuesta();
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    if (Par.Monto > 0)
                    {
                        USUARIOS usr = db.USUARIOS.Where(d => d.CODIGO == PIN).FirstOrDefault();
                        if (usr != null)
                        {
                            if (usr.CAJA == 1)
                            {
                                float MontoCaja = (float)Par.Monto;
                                var Trs = db.TRANSACCIONES.Where(d => d.CODIGO == Par.CodMov).FirstOrDefault();
                                System.Data.Entity.Core.Objects.ObjectParameter P = new System.Data.Entity.Core.Objects.ObjectParameter("new_identity", 1);
                                var SM = db.NuevoMovOperacion(Trs.TIPO, Par.CodMov, Par.Monto, Par.Detalle, Par.Usuario, P);
                                int Numcaja = 0;
                                foreach (var S in SM)
                                {
                                    Numcaja = S.Value;
                                }
                                var C = db.NuevaCaja(Trs.TIPO, MontoCaja, Par.Usuario, 13, Par.Medio, DateTime.Now, Numcaja, Par.Moneda);
                                Impresiones I = new Impresiones();
                                var RespImpr = I.ImprimeIngresoEgreso(Numcaja, usr.CODIGO, BDCia);
                                R.Codigo = Numcaja;
                                R.Mensaje = "Moviento creado, " + RespImpr.Mensaje;
                            }
                            else
                            {
                                R.Codigo = -4;
                                R.Mensaje = "Debe abrir caja para pagar este documento";
                            }

                        }
                        else
                        {
                            R.Codigo = -3;
                            R.Mensaje = "Usuario Incorrecto";
                        }
                    }
                    else
                    {
                        R.Codigo = -1;
                        R.Mensaje = "Monto incorrecto";
                    }
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = ex.Message;

            }
            return R;
        }


        public Respuesta AperturaCierre(DatosConteoAperturaCierre Conteo, string BDCia)
        {
            Respuesta R = new Respuesta();
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    USUARIOS usr = db.USUARIOS.Where(d => d.CODIGO == Conteo.usuario).FirstOrDefault();
                    if (usr != null)
                    {
                        if (usr.CAJA == 0)
                        {

                            db.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                            usr.CAJA = 1;
                            db.SaveChanges();
                            db.NuevaCaja(1, Conteo.Apertura, usr.CODIGO, 1, 1, DateTime.Now, 1, "CRC");
                            db.NuevaCaja(1, Conteo.AperturaDol, usr.CODIGO, 1, 1, DateTime.Now, 1, "USD");
                            R.Codigo = 0;
                            R.Mensaje = "La Caja queda abierta, ya puede aplicar pagos";
                        }
                        else
                        {
                            db.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                            usr.CAJA = 0;
                            db.SaveChanges();
                            System.Data.Entity.Core.Objects.ObjectParameter P = new System.Data.Entity.Core.Objects.ObjectParameter("new_identity", 1);
                            int CodCierre = 0;
                            var cierre = db.NuevoCierre(usr.CODIGO,
                                Conteo.Efectivo,
                                Conteo.Tarjeta,
                                Conteo.Transfer,
                                Conteo.Cheque,
                                Conteo.ConteoEfectivo,
                                Conteo.ConteoTarjeta,
                                0,
                                Conteo.ConteoTransfer, 0,
                                Conteo.Apertura, Conteo.ConteoEfectivoDol, Conteo.ConteoTarjetaDol, Conteo.ConteoTransferDol, Conteo.ConteoCheque, Conteo.EfectivoDol, Conteo.TarjetaDol, Conteo.TransferDol, Conteo.ChequeDol,  P);
                            foreach (var C in cierre)
                            {
                                CodCierre = C.Value;
                            }
                            if (Conteo.Efectivo > 0)
                            {
                                db.NuevaCaja(2, Conteo.Efectivo + Conteo.Apertura, usr.CODIGO, 1, 1, DateTime.Now, CodCierre, "CRC");
                            }
                            if (Conteo.Tarjeta > 0)
                            {
                                db.NuevaCaja(2, Conteo.Tarjeta, usr.CODIGO, 1, 2, DateTime.Now, CodCierre, "CRC");
                            }
                            if (Conteo.Transfer > 0)
                            {
                                db.NuevaCaja(2, Conteo.Transfer, usr.CODIGO, 1, 3, DateTime.Now, CodCierre, "CRC");
                            }
                            if (Conteo.Cheque > 0)
                            {
                                db.NuevaCaja(2, Conteo.Cheque, usr.CODIGO, 1, 4, DateTime.Now, CodCierre, "CRC");
                            }
                            //Dolares
                            if (Conteo.EfectivoDol > 0)
                            {
                                db.NuevaCaja(2, Conteo.EfectivoDol + Conteo.AperturaDol, usr.CODIGO, 1, 1, DateTime.Now, CodCierre, "USD");
                            }
                            if (Conteo.TarjetaDol > 0)
                            {
                                db.NuevaCaja(2, Conteo.TarjetaDol, usr.CODIGO, 1, 2, DateTime.Now, CodCierre, "USD");
                            }
                            if (Conteo.TransferDol > 0)
                            {
                                db.NuevaCaja(2, Conteo.TransferDol, usr.CODIGO, 1, 3, DateTime.Now, CodCierre, "USD");
                            }
                            if (Conteo.ChequeDol > 0)
                            {
                                db.NuevaCaja(2, Conteo.ChequeDol, usr.CODIGO, 1, 4, DateTime.Now, CodCierre, "USD");
                            }
                            R.Codigo = 1;
                            R.Mensaje = "Caja ha sido cerrada";

                        }
                    }
                    else
                    {
                        R.Codigo = -2;
                        R.Mensaje = "Error al validar el usuario";
                    }
                }
                    
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = ex.Message;
            }
            return R;
        }

        public class RespuestaCaja
        {
            public int Codigo { get; set; }
            public string Mensaje { get; set; }
            public float Efectivo { get; set; }
            public float Tarjeta { get; set; }
            public float Transfer { get; set; }
            public float Cheque { get; set; }
            public float Apertura { get; set; }
            public string VerSaldo { get; set; }

            public float EfectivoDol { get; set; }
            public float TarjetaDol { get; set; }
            public float TransferDol { get; set; }
            public float ChequeDol { get; set; }
            public float AperturaDol { get; set; }

            public string MonedaEmpresa { get; set; }
        }

        public class DatosConteoAperturaCierre
        {
            public int usuario { get; set; }
            public float Efectivo { get; set; }
            public float Tarjeta { get; set; }
            public float Transfer { get; set; }
            public float Cheque { get; set; }
            public float Apertura { get; set; }

            public float ConteoEfectivo { get; set; }
            public float ConteoTarjeta { get; set; }
            public float ConteoTransfer { get; set; }
            public float ConteoCheque { get; set; }

            public float EfectivoDol { get; set; }
            public float TarjetaDol { get; set; }
            public float TransferDol { get; set; }
            public float ChequeDol { get; set; }
            public float AperturaDol { get; set; }

            public float ConteoEfectivoDol { get; set; }
            public float ConteoTarjetaDol { get; set; }
            public float ConteoTransferDol { get; set; }
            public float ConteoChequeDol { get; set; }

        }

        public class ParamMovTransaccional
        {
            public int CodMov { get; set; }
            public decimal Monto { get; set; }
            public string Detalle { get; set; }
            public int Medio { get; set; }
            public string Moneda { get; set; }
            public int Usuario { get; set; }
        }
    }
}
