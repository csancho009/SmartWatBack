using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using LogicaSmartWat.Datos;
using LogicaSmartWat.RPTs;
using SmartWatBack.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat.Controllers
{
    public class Impresiones
    {
        public Respuesta Factura (int Numero, string BD)
        {
            Respuesta R = new Respuesta();
            ReportDocument Reporte = new ReportDocument();
            try
            {
                Reporte = new FacturaTiquete();
                Reporte.SetParameterValue(0, Numero);
                Conecta_Reporte(ref Reporte, BD);
                Stream reportStream = Reporte.ExportToStream(ExportFormatType.PortableDocFormat);
                byte[] pdfBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    reportStream.CopyTo(memoryStream);
                    pdfBytes = memoryStream.ToArray();
                }
                R.Objeto = Convert.ToBase64String(pdfBytes);
                R.Codigo = 0;
                R.Mensaje = "OK";
                Reporte.Dispose();
                Reporte.Close();
                Reporte = null;
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = ex.Message;
            }
            return R;

        }

        public Respuesta NCDevolucion(int Numero, int IdCia, string BD)
        {
            Respuesta R = new Respuesta();
            ReportDocument Reporte = new ReportDocument();
            try
            {
                Reporte = new NCdevolucion();
                Reporte.SetParameterValue(0, Numero);
                Reporte.SetParameterValue(1, IdCia);
                Conecta_Reporte(ref Reporte, BD);
                Stream reportStream = Reporte.ExportToStream(ExportFormatType.PortableDocFormat);
                byte[] pdfBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    reportStream.CopyTo(memoryStream);
                    pdfBytes = memoryStream.ToArray();
                }
                R.Objeto = Convert.ToBase64String(pdfBytes);
                R.Codigo = 0;
                R.Mensaje = "OK";
                Reporte.Dispose();
                Reporte.Close();
                Reporte = null;
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = ex.Message;
            }
            return R;

        }

        public Respuesta PrefacturaFactura(int NumeroCoti, int NumPaja, int NumLectura, string BD)
        {
            Respuesta R = new Respuesta();
            ReportDocument Reporte = new ReportDocument();
            try
            {
                Reporte = new CotiPreventa();
                Reporte.SetParameterValue(0, NumeroCoti);
                Reporte.SetParameterValue(1, NumPaja);
                Reporte.SetParameterValue(2, NumLectura);
                Conecta_Reporte(ref Reporte, BD);
                Stream reportStream = Reporte.ExportToStream(ExportFormatType.PortableDocFormat);
                byte[] pdfBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    reportStream.CopyTo(memoryStream);
                    pdfBytes = memoryStream.ToArray();
                }
                R.Objeto = Convert.ToBase64String(pdfBytes);
                R.Codigo = 0;
                R.Mensaje = "OK";
                Reporte.Dispose();
                Reporte.Close();
                Reporte = null;
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = ex.Message +"  NumCoti "+  NumeroCoti.ToString() + "  Num Paja " + NumPaja.ToString() + "  Num Lectura " + NumLectura.ToString() + "  BD " + BD;
            }
            return R;

        }
        public Respuesta PrefacturaFacturaAlCorreo(int NumeroCoti, int NumPaja, int NumLectura, string BD, string Destinatario)
        {
            Respuesta R = new Respuesta();
            ReportDocument Reporte = new ReportDocument();
            try
            {
                Reporte = new CotiPreventa();
                Reporte.SetParameterValue(0, NumeroCoti);
                Reporte.SetParameterValue(1, NumPaja);
                Reporte.SetParameterValue(2, NumLectura);
                Conecta_Reporte(ref Reporte, BD);
               
                DiskFileDestinationOptions crDiskFileDestinationOptions;
                crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                ExportOptions crExportOptions;
                string Archivo = @"\\server\c$\exports\" + BD + @"\Cobro Paja #" + NumPaja.ToString() + " Gestión " + NumeroCoti.ToString() + ".pdf";
                crDiskFileDestinationOptions.DiskFileName = Archivo  ;
                crExportOptions = Reporte.ExportOptions;
                {
                    var withBlock = crExportOptions;
                    withBlock.DestinationOptions = crDiskFileDestinationOptions;
                    withBlock.ExportDestinationType = ExportDestinationType.DiskFile;
                    withBlock.ExportFormatType = ExportFormatType.PortableDocFormat;
                }
                Reporte.Export();

                CorreoElectronico C = new CorreoElectronico();
                var EC = C.EnviarCorreo(Destinatario, "", "<h4>Estimado Abonado</h4><p>Se adjunta la orden de cobro correspondiente al consumo de agua</p>", "Consumo de agua", Archivo);
                File.Delete(Archivo);

                if (EC.Codigo==0)
                {
                    R.Codigo = 0;
                    R.Mensaje = "Correo enviado con éxito";
                }
                else
                {
                    R.Codigo = -1;
                    R.Mensaje =EC.Mensaje;
                }
                    
                
                Reporte.Dispose();
                Reporte.Close();
                Reporte = null;
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = ex.Message + "  NumCoti " + NumeroCoti.ToString() + "  Num Paja " + NumPaja.ToString() + "  Num Lectura " + NumLectura.ToString() + "  BD " + BD;
            }
            return R;

        }
        public Respuesta CierreCaja(int Numero, string BD)
        {
            Respuesta R = new Respuesta();
            ReportDocument Reporte = new ReportDocument();
            try
            {
                Reporte = new CierreTiquete();
                Reporte.SetParameterValue(0, Numero);
                Conecta_Reporte(ref Reporte, BD);
                Stream reportStream = Reporte.ExportToStream(ExportFormatType.PortableDocFormat);
                byte[] pdfBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    reportStream.CopyTo(memoryStream);
                    pdfBytes = memoryStream.ToArray();
                }
                R.Objeto = Convert.ToBase64String(pdfBytes);
                R.Codigo = 0;
                R.Mensaje = "OK";
                Reporte.Dispose();
                Reporte.Close();
                Reporte = null;
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = ex.Message;
            }
            return R;

        }

        public Respuesta ImpresionMovTransac(int Miusuario, DateTime Desde, DateTime Hasta, int Vendedor, string Salida, string BD)
        {
            Respuesta R = new Respuesta();
            string NombreImpresora = "";
            try
            {
                using (POLTAEntities db = new POLTAEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BD);
                    List<IMPRESORASxUSUARIO> iu = db.IMPRESORASxUSUARIO.Where(d => d.USUARIO == Miusuario).ToList();
                    foreach (var I in iu)
                    {
                        if (NombreImpresora == "")
                        {
                            NombreImpresora = I.IMPRESORAS.NOMBRE;
                        }
                        if (I.IMPRESORAS.REDUCIDA == 0)
                        {
                            NombreImpresora = I.IMPRESORAS.NOMBRE;
                        }
                    }

                    ReportDocument Reporte = new ReportDocument();
                    Reporte = new IngEgrDetallados();
                    Reporte.SetParameterValue(0, "DIA");
                    Reporte.SetParameterValue(1, Vendedor);
                    Reporte.SetParameterValue(2, Desde);
                    Reporte.SetParameterValue(3, Hasta);

                    Conecta_Reporte(ref Reporte, BD);
                    Reporte.PrintOptions.PrinterName = NombreImpresora;
                    switch (Salida)
                    {
                        case "IMP":
                            Reporte.PrintToPrinter(1, false, 0, 0);
                            R.Mensaje = "Impresión Exitosa";
                            break;
                        case "PDF":
                            Stream reportStream = Reporte.ExportToStream(ExportFormatType.PortableDocFormat);
                            byte[] pdfBytes;
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                reportStream.CopyTo(memoryStream);
                                pdfBytes = memoryStream.ToArray();
                            }
                            R.Mensaje = Convert.ToBase64String(pdfBytes);
                            break;
                        case "XLS":
                            Stream reportStream2 = Reporte.ExportToStream(ExportFormatType.ExcelWorkbook);
                            byte[] pdfBytes2;
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                reportStream2.CopyTo(memoryStream);
                                pdfBytes2 = memoryStream.ToArray();
                            }
                            R.Mensaje = Convert.ToBase64String(pdfBytes2);
                            break;
                    }

                    R.Codigo = 0;

                    Reporte.Dispose();
                    Reporte.Close();
                    Reporte = null;
                }
                    
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = ex.Message;
            }
            return R;
        }

        public Respuesta ImprimeIngresoEgreso(int NumMov, int usuario, string BD)
        {
            Respuesta R = new Respuesta();
           // string NombreImpresora = "";
            try
            {
                using (POLTAEntities db = new POLTAEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BD);
                    //List<IMPRESORASxUSUARIO> iu = db.IMPRESORASxUSUARIO.Where(d => d.USUARIO == usuario).ToList();
                    //foreach (var I in iu)
                    //{
                    //    if (NombreImpresora == "")
                    //    {
                    //        NombreImpresora = I.IMPRESORAS.NOMBRE;
                    //    }
                    //    if (I.IMPRESORAS.REDUCIDA == 0)
                    //    {
                    //        NombreImpresora = I.IMPRESORAS.NOMBRE;
                    //    }
                    //}
                    ReportDocument Reporte = new ReportDocument();
                    try
                    {
                        Reporte = new IngresoEgresoCaja();
                        Reporte.SetParameterValue(0, NumMov);
                        Conecta_Reporte(ref Reporte, BD);

                        Stream reportStream2 = Reporte.ExportToStream(ExportFormatType.PortableDocFormat);
                        byte[] pdfBytes2;
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            reportStream2.CopyTo(memoryStream);
                            pdfBytes2 = memoryStream.ToArray();
                        }
                        R.Objeto = Convert.ToBase64String(pdfBytes2);
                        R.Codigo = 0;
                        R.Mensaje = "Impresión Exitosa";

                    }
                    catch (Exception)
                    {


                    }
                    finally
                    {
                        Reporte.Dispose();
                        Reporte.Close();
                        Reporte = null;
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

        public Respuesta MovimientosCaja(int usuario, string Tipo, int MiUsuario, string BD)
        {
            Respuesta R = new Respuesta();
            //string NombreImpresora = "";
            try
            {
                using (POLTAEntities db = new POLTAEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BD);
                    //List<IMPRESORASxUSUARIO> iu = db.IMPRESORASxUSUARIO.Where(d => d.USUARIO == MiUsuario).ToList();
                    //foreach (var I in iu)
                    //{
                    //    if (NombreImpresora == "")
                    //    {
                    //        NombreImpresora = I.IMPRESORAS.NOMBRE;
                    //    }
                    //    if (I.IMPRESORAS.REDUCIDA == 0)
                    //    {
                    //        NombreImpresora = I.IMPRESORAS.NOMBRE;
                    //    }
                    //}

                    ReportDocument Reporte = new ReportDocument();
                    if (Tipo == "Deta")
                    {
                        Reporte = new MovCajaDetallados();
                        Reporte.SetParameterValue(1, usuario);
                    }
                    else
                    {
                        Reporte = new MovCajaResumidos();
                    }

                    Reporte.SetParameterValue(0, usuario);

                    Conecta_Reporte(ref Reporte, BD);
                    Stream reportStream2 = Reporte.ExportToStream(ExportFormatType.PortableDocFormat);
                        byte[] pdfBytes2;
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            reportStream2.CopyTo(memoryStream);
                            pdfBytes2 = memoryStream.ToArray();
                        }
                    R.Objeto = Convert.ToBase64String(pdfBytes2);
                    R.Codigo = 0;
                    R.Mensaje = "Impresión Exitosa";
                    Reporte.Dispose();
                    Reporte.Close();
                    Reporte = null;
                }
                    
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = ex.Message;
            }
            return R;
        }



        private void Conecta_Reporte(ref ReportDocument Reporte, string BD)
        {
            TableLogOnInfo rptcon = new TableLogOnInfo();
            foreach (Table table in Reporte.Database.Tables)
            {
                rptcon.ConnectionInfo.DatabaseName = BD;
                rptcon.ConnectionInfo.ServerName = "Server";
                rptcon.ConnectionInfo.UserID = "usrrh";
                rptcon.ConnectionInfo.Password = "12345";
                table.ApplyLogOnInfo(rptcon);
            }
        }
    }
}
