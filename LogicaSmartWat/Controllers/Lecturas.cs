using LogicaSmartWat.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat.Controllers
{
    public class Lecturas
    {
        public Respuesta IngresaLectura(LECTURAS L, string BDCia)
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
                    L.FECHA = DateTime.Now;
                    db.LECTURAS.Add(L);
                    db.SaveChanges();

                    R.Codigo = L.ID_LEC;
                    R.Mensaje = "Ok";                    
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta IngresaLectura " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);

            }

            return R;
        }

        public Respuesta ActualizaLectura(int NumLectura, int Consumo, int usuario, string BDCia)
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

                    LECTURAS L = db.LECTURAS.Where(d => d.ID_LEC == NumLectura).FirstOrDefault();
                    if (L.ESTADO=="PEN" || L.ESTADO == "COT")
                    {
                        db.Entry(L).State = System.Data.Entity.EntityState.Modified;
                        L.LECTURA = Consumo;
                        L.ID_USU = usuario;
                        db.SaveChanges();
                        if (L.ESTADO == "COT")
                        {
                            db.RECTIFICA_LECTURA_MEDIDOR(usuario, L.ID_LEC);
                        }
                        R.Codigo = L.ID_LEC;
                        R.Mensaje = "Ok";
                    }
                    else
                    {
                        R.Codigo = -2;
                        R.Mensaje = "Esa lectura no puede cambiarse";
                    }
                    
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta ActualizaLectura " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta TablaLecturas(int mes, int annio, string BDCia)
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

                    var Repos = db.TablaLecturas(mes, annio);
                    R.Codigo = 0;
                    R.Mensaje = "Ok";
                    R.Objeto = Repos.ToList();
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta TablaLecturas " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }
        public Respuesta GenerarCobro(int Usuario, string BDCia)
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

                    var Repos = db.GENERA_FACTURACION(Usuario);
                    R.Codigo = 0;
                    R.Mensaje = "Ok";
                    R.Objeto = Repos.ToList();
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta GenerarCobro " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta AplicaPago(ParametrosPagos P)
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
                    db.Database.Connection.ChangeDatabase(P.BDCia);
                    USUARIOS usr = db.USUARIOS.Where(d => d.CODIGO == P.Usuario).FirstOrDefault();
                    if (usr.CAJA == 1)
                    {
                        var Repos = db.PAGO_LECTURA(P.NunCoti, P.Usuario, P.EfectivoReal, P.Transfer, P.Cuenta,
                        P.NumTransfer, P.Tarjeta, P.Datafono, P.Voucher);
                        foreach (var L in Repos)
                        {
                            if (L.id > 0)
                            {
                                R.Codigo = (int)L.id;

                            }
                            else
                            {
                                R.Codigo = -2;
                                R.Mensaje = "Alerta al intentar aplicar pago " + L.Mensaje;
                            }
                        }
                    }
                    else
                    {
                        R.Codigo = -4;
                        R.Mensaje = "Caja está cerrada";
                    }
                        
                    db.Database.Connection.Close();                    
                }
                if (R.Codigo > 0)
                {
                    Impresiones I = new Impresiones();
                    Respuesta R2 = I.Factura(R.Codigo, P.BDCia);
                    R.Mensaje = "Documento creado exitosamente #" + R.Codigo.ToString();
                    if (R2.Codigo == 0)
                    {
                        R.Objeto = R2.Objeto;
                    }
                    else
                    {
                        R.Mensaje = "Documento creado exitosamente #" + R.Codigo.ToString() + ", sin embargo, hubo al generar el pdf indica: " + R2.Mensaje; ;
                    }
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta Generar Cobro " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta PreImpresion(ParametrosPagos P)
        {

            Respuesta R = new Respuesta();
            Impresiones I = new Impresiones();
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(P.BDCia);
                    COTIZACIONES Ct = db.COTIZACIONES.Where(d => d.CODIGO == P.NunCoti).FirstOrDefault();
                    if (Ct.ESTADO == 4) //Ya se facturo
                    {
                        FACTURAS F = db.FACTURAS.Where(d => d.USU_CAN == Ct.CODIGO).FirstOrDefault();
                        Respuesta R2 = I.Factura(F.CODIGO, P.BDCia);
                        if (R2.Codigo == 0)
                        {
                            R.Codigo = 0;
                            R.Mensaje = "OK";
                            R.Objeto = R2.Objeto;
                        }
                        else
                        {
                            R.Codigo = -1;
                            R.Mensaje = "Hubo una situación al generar el PDF -Factura- indica: " + R2.Mensaje; ;
                        }
                    }
                    else
                    {
                        LECTURAS Ls = db.LECTURAS.Where(d => d.ID_LEC==Ct.CANCELADO).FirstOrDefault();
                        Respuesta R2 = I.PrefacturaFactura(Ct.CODIGO,Ls.ID_PAJ, Ls.ID_LEC, P.BDCia);
                        if (R2.Codigo == 0)
                        {
                            R.Codigo = 0;
                            R.Mensaje = "OK";
                            R.Objeto = R2.Objeto;
                        }
                        else
                        {
                            R.Codigo = -1;
                            R.Mensaje = "Hubo una situación al generar el PDF -Prefactura- indica: " + R2.Mensaje; ;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta Generar Impresión " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }
    }
}
