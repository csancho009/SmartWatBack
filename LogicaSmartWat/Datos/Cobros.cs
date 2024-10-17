using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat.Datos
{
    public class Cobros
    {
        public int CODIGO { get; set; }
        public string NOMBRE { get; set; }
        public string Periodo { get; set; }
        public float Total { get; set; }
        public string MEDIDOR { get; set; }
        public string IDENTIFICACION { get; set; }
        public int Factura { get; set; }
        public string EstadoFE { get; set; }
        public string EstadoCorreo { get; set; }
        public List<DetalleCobros> Detalle = new List<DetalleCobros>();
    }

    public class DetalleCobros
    {
        public string  Codigo { get; set; }
        public string Nombre { get; set; }
        public string  Precio { get; set; }
        public float cantidad { get; set; }
        public float Descuento { get; set; }
        public string  Sub { get; set; }
        public string Vendedor { get; set; }
        public int NumeroLin { get; set; }
    }
}
