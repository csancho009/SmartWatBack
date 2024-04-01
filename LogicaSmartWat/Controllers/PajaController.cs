using LogicaSmartWat.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat
{
    public class PajaController
    {


        public Respuesta ObtenerPajas(int paja, string BDCia)
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
                    var Ls = from P in db.PAJAS.Where(d => d.ID_PAJ == paja)
                             join C in db.CLIENTES on P.ID_CLI equals C.CODIGO
                             select new { P.ID_PAJ, P.MEDIDOR, P.ID_CLI, P.ID_BLO, P.DIRECCION, P.TIPO_TARIFA,P.ESTADO, P.FECHA_INSTALACION, objectCli= new ObjCliente {value= C.CODIGO.ToString(), label=C.NOMBRE } };
                    R.Objeto =Ls.ToList();
                    R.Codigo = 0;
                    R.Mensaje = "Ok";
                    db.Database.Connection.Close();
                }
            } catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta ObtenerPajas " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta BUSCAR_PAJA_PARAMETROS(string paja, string Cedula, string Nombre, string BDCia, string Medidor)
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
                    R.Codigo = 0;
                    R.Mensaje = "Ok";
                    R.Objeto = db.SP_BUSCAR_PAJA_PARAMETROS(paja, Cedula, Nombre, Medidor).ToList();
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta BUSCAR_PAJA_PARAMETROS " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta IngresarPaja(PAJAS paja, string BDCia) 
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
                    if (paja.ID_PAJ == 0)
                    {
                        paja.FECHA_INSTALACION = DateTime.Now;
                        db.PAJAS.Add(paja);
                        db.SaveChanges();
                        R.Codigo = paja.ID_PAJ;
                    }
                    else
                    {
                        var P = db.PAJAS.Where(d => d.ID_PAJ == paja.ID_PAJ).FirstOrDefault();
                        db.Entry(P).State = System.Data.Entity.EntityState.Modified;
                        P.MEDIDOR = paja.MEDIDOR;
                        P.ID_CLI = paja.ID_CLI;
                        P.ID_BLO = paja.ID_BLO;
                        P.DIRECCION = paja.DIRECCION;
                        P.TIPO_TARIFA = paja.TIPO_TARIFA;
                        P.ESTADO = paja.ESTADO;
                        db.SaveChanges();
                        R.Codigo= paja.ID_PAJ;
                    }

                    
                    R.Mensaje = "Ok";
                    R.Objeto = paja;
                    db.Database.Connection.Close();
                }
            } catch (Exception ex) 
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta GuardaPaja " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }

            return R;
        }

        public Respuesta PendientesPago(string  Nombre, string Cedula, string BDCia, int Estado)
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
                    List<Cobros> C = new List<Cobros>();
                    var Ps = db.PAJAS_PENDIENTES_PAGO(Nombre, Cedula, Estado);
                    foreach(var P in Ps)
                    {
                        var Ts = db.TablaPrecios(P.CODIGO).ToList();
                        C.Add(new Cobros
                        {
                            CODIGO = P.CODIGO,
                            NOMBRE = P.NOMBRE,
                            Periodo = P.Periodo,
                            Total = (float)P.Total,
                            MEDIDOR=P.MEDIDOR,
                            IDENTIFICACION= P.IDENTIFICACION,
                            Factura=P.Factura,
                            Detalle = (from L in Ts select new DetalleCobros { cantidad = (float)L.cantidad, Codigo=L.Codigo, Descuento= (float)L.Descuento, 
                                Nombre=L.Nombre, NumeroLin= (int) L.C_, Precio= L.Precio, Sub=L.Sub, Vendedor=L.Vendedor }).ToList()
                        }); ;
                    }

                    R.Mensaje = "Ok";
                    R.Objeto = C;
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta PendientesPago " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }

            return R;
        }

        public Respuesta CuentasDeBanco(string BDCia)
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
                    var Ds = db.CUENTAS_BANCO;

                    R.Mensaje = "Ok";
                    R.Objeto = (from D in Ds select new { D.CODIGO, D.CUENTA }).ToList();
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta Datafonos " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta Datafonos(string BDCia)
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
                    var Ds = db.DATAFONOS;

                    R.Mensaje = "Ok";
                    R.Objeto = (from D in Ds select new {D.CODIGO,D.NOMBRE }).ToList();
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta Datafonos " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta ActualizarPaja(PAJAS paja, String BaseDeDatos)
        {
            Respuesta R = new Respuesta();

            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    db.Database.Connection.ChangeDatabase(BaseDeDatos);
                    PAJAS pajaE = db.PAJAS.First(b => b.ID_PAJ == paja.ID_PAJ);
                    if (pajaE != null)
                    {
                        pajaE.MEDIDOR = paja.MEDIDOR;
                        pajaE.ID_CLI = paja.ID_CLI;
                        pajaE.ID_BLO = paja.ID_BLO;
                        pajaE.DIRECCION = paja.DIRECCION;
                        pajaE.TIPO_TARIFA = paja.TIPO_TARIFA;
                        pajaE.ESTADO = paja.ESTADO;
                        db.SaveChanges();
                        R.Codigo = 1;
                        R.Mensaje = "Se ha actualizado con éxito";
                    }
                    else
                    {
                        R.Objeto = paja;
                        R.Codigo = 0;
                        R.Mensaje = "La zona actualizada no existe";
                    }

                    R.Codigo = 75;
                    R.Mensaje = "Ok";
                    R.Objeto = paja;
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta ActualizarPaja " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }

            return R;
        }


    }
    public class ObjCliente
    {
        public string value { get; set; }
        public string label { get; set; }
    }
}
