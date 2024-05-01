using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using LogicaSmartWat.Datos;
using LogicaSmartWat.RPTs;
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
                R.Mensaje = ex.Message;
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
