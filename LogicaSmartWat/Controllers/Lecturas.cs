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
                using (POLTAEntities db = new POLTAEntities())
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
                using (POLTAEntities db = new POLTAEntities())
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
                using (POLTAEntities db = new POLTAEntities())
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
                using (POLTAEntities db = new POLTAEntities())
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

        public Respuesta AnularFactura(int NumFact, string BDCia)
        {
            Respuesta R= new Respuesta();
            try
            {
                using (POLTAEntities db = new POLTAEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);

                    var Sp = db.ANULA_FACTURA(NumFact);
                   
                    foreach (var S in Sp)
                    {
                        R= new Respuesta { Codigo = (int) S.id, Mensaje = S.Mensaje };
                    }
                    db.Database.Connection.Close();
                    if (R.Codigo >= 0)
                    {
                        Impresiones I = new Impresiones();
                        var Emp = db.CIA.FirstOrDefault();
                        Respuesta RI = I.NCDevolucion(R.Codigo, Emp.CODIGO, BDCia);
                        if (RI.Codigo >= 0)
                        {
                            R.Objeto = RI.Objeto;
                        }
                        else
                        {
                            R.Objeto = "NA";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                R= new Respuesta { Codigo = -1, Mensaje = "Alerta  Anular Factura " + ex.Message + " NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) };
            }
            return R;
        }

        public Respuesta AplicaPago(ParametrosPagos P)
        {
            Respuesta R = new Respuesta();

            try
            {
                using (POLTAEntities db = new POLTAEntities())
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

        public Respuesta ReciboEspecial(ParametrosReciboEspecial P)
        {
            Respuesta R = new Respuesta();

            try
            {
                using (POLTAEntities db = new POLTAEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(P.BDCia);
                    USUARIOS usr = db.USUARIOS.Where(d => d.CODIGO == P.Usuario).FirstOrDefault();
                    if (usr.CAJA == 1)
                    {
                        var Repos = db.PAGO_RECIBO_ESPECIAL(P.Usuario,P.CodigoArticulo,P.Cliente,P.Monto,P.Nota,P.MedioPago,P.CtaBanco,P.Datafono);
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
                R.Mensaje = "Alerta Generar ReciboEspecial " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) +" msg "+ ex.InnerException ;
            }
            return R;
        }

        public Respuesta PreImpresion(ParametrosPagos P)
        {

            Respuesta R = new Respuesta();
            Impresiones I = new Impresiones();
            try
            {
                using (POLTAEntities db = new POLTAEntities())
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

        public Respuesta EnviarCobroCorreo(ParametrosPagos P)
        {

            Respuesta R = new Respuesta();
            Impresiones I = new Impresiones();
            try
            {
                using (POLTAEntities db = new POLTAEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(P.BDCia);
                    COTIZACIONES Ct = db.COTIZACIONES.Where(d => d.CANCELADO == P.NunCoti).FirstOrDefault();
                    if (Ct.ESTADO == 4) //Ya se facturo
                    {
                        //FACTURAS F = db.FACTURAS.Where(d => d.USU_CAN == Ct.CODIGO).FirstOrDefault();
                        //Respuesta R2 = I.Factura(F.CODIGO, P.BDCia);
                        //if (R2.Codigo == 0)
                        //{
                        //    R.Codigo = 0;
                        //    R.Mensaje = "OK";
                        //    R.Objeto = R2.Objeto;
                        //}
                        //else
                        //{
                        //    R.Codigo = -1;
                        //    R.Mensaje = "Hubo una situación al generar el PDF -Factura- indica: " + R2.Mensaje; ;
                        //}
                    }
                    else
                    {
                        if (Ct.CLIENTES.CORREO.Contains("@"))
                        {
                            LECTURAS Ls = db.LECTURAS.Where(d => d.ID_LEC == Ct.CANCELADO).FirstOrDefault();
                            Respuesta R2 = I.PrefacturaFacturaAlCorreo(Ct.CODIGO, Ls.ID_PAJ, Ls.ID_LEC, P.BDCia, Ct.CLIENTES.CORREO);
                            if (R2.Codigo == 0)
                            {
                                R.Codigo = 0;
                                R.Mensaje = "OK";
                                R.Objeto = R2.Objeto;
                            }
                            else
                            {
                                R.Codigo = -1;
                                R.Mensaje = "EnviarCobroCorreo -  situación al generar el PDF -Prefactura- indica: " + R2.Mensaje; ;
                            }
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

        public Respuesta ImpresionEnSitio(int NumLectura, string BDCia)
        {
            Respuesta R = new Respuesta();
            if (NumLectura > 0)
            {
              
                try
                {
                    using (POLTAEntities db = new POLTAEntities())
                    {
                        if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                        {
                            db.Database.Connection.Open();
                        }
                        db.Database.Connection.ChangeDatabase(BDCia);
                        COTIZACIONES Ct = db.COTIZACIONES.Where(d => d.CANCELADO == NumLectura).FirstOrDefault();
                        CIA C = db.CIA.FirstOrDefault();
                        LECTURAS Ls = db.LECTURAS.Where(d => d.ID_LEC == Ct.CANCELADO).FirstOrDefault();
                        ImpresionWeb I = new ImpresionWeb();
                        List<DetallePrefactura> LineasCoti = new List<DetallePrefactura>();
                        float  ContaIva = 0;
                        float ContaSub = 0;
                        float ContaDescuento = 0;
                        foreach (var D in Ct.DETALLE_CTZ.OrderBy(O=>O.LIN))
                        {
                            ContaIva += ((float) D.PRECIO / (1 + ( (float)D.INVENTARIO.IMPUESTOS)/100)) * (((float)D.INVENTARIO.IMPUESTOS) / 100) * (float) D.CANTIDAD; //(D.PRECIO /  ( 1 +  D.INVENTARIO.IMPUESTOS)) * D.CANTIDAD
                            ContaSub+= ((float)D.PRECIO / (1 + ((float)D.INVENTARIO.IMPUESTOS) / 100)) * (float)D.CANTIDAD;
                            ContaDescuento+= ((float)D.PRECIO / (1 + ((float)D.INVENTARIO.IMPUESTOS) / 100)) * ((float)D.IMPUESTO / 100)  * (float)D.CANTIDAD;
                            LineasCoti.Add(new DetallePrefactura { Descripcion = D.NOMBRE, Cantidad = (float)D.CANTIDAD, Precio = ((float)D.PRECIO / (1 + ((float)D.INVENTARIO.IMPUESTOS) / 100)), Valor = ((float)D.PRECIO / (1 + ((float)D.INVENTARIO.IMPUESTOS) / 100)) * (float)D.CANTIDAD });
                        }
                        string LecturaAnterior = "";
                        var LA = db.ObtenerLecturaAnterior(Ls.ID_PAJ, NumLectura);
                        foreach(var A in LA)
                        {
                            LecturaAnterior = A.Value.ToString();
                        }

                        string FachaAnterior = "";
                        var FA = db.ObtenernerFechaLecturaAnterior(Ls.ID_PAJ, NumLectura);
                        foreach(var F in FA)
                        {
                            FachaAnterior = F.Value.ToString();
                        }
                        R.Objeto = new ImpresionWeb
                        {
                            NombreCia = C.NOMBRE,
                            Direccion = C.DIRECCION,
                            Telefono = C.TELEFONO,
                            Cedula = C.CEDULA,
                            Abonado= Ct.CLIENTES.NOMBRE,
                            NumeroCoti = Ct.CODIGO.ToString(),
                            TipoTarifa = Ls.PAJAS.TIPO_TARIFA == "R" ? "Residencial" : "Comercial",
                            CodigoPaja = Ls.PAJAS.ID_PAJ.ToString(),
                            Medidor = Ls.PAJAS.MEDIDOR,
                            NumLectura = Ls.ID_LEC.ToString(),
                            FechaPrefactura = (DateTime) Ct.FECHA,
                            Periodo = Ls.MES.ToString() + "-" + Ls.ANIO.ToString(),
                            LecturaAnterior = FachaAnterior + " x " + LecturaAnterior + " M3",
                            LecturaActual = Ls.LECTURA.ToString(),
                            Consumo= (int) Ls.LECTURA- int.Parse(LecturaAnterior),
                            SubTotal=ContaSub,
                            Descuento=ContaDescuento,
                            IVA=ContaIva,
                            Total=ContaSub-ContaDescuento+ContaIva,
                            Detalle= LineasCoti
                        };
                        R.Mensaje = "OK";
                    }

                }
                catch (Exception ex)
                {
                    R.Codigo = -1;
                    R.Mensaje = "Alerta Generar Impresión Masiva " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                }
            }
            else
            {
                R.Codigo = 0;
                R.Mensaje = "No es una lectura";
            }            
           
            return R;
        }
    }
}
