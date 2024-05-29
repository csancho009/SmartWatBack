using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat.Datos
{
    public class ImpresionWeb
    {
        public string NombreCia {get;set;}
        public string Direccion  {get;set;}
        public string Telefono  {get;set;}
        public string Cedula  {get;set;}
        public string NumeroCoti { get; set; }
        public string TipoTarifa { get; set; }
        public string CodigoPaja { get; set; }
        public string Abonado { get; set; }
        public string Medidor { get; set; }
        public string NumLectura { get; set; }
        public DateTime FechaPrefactura { get; set; }
        public string Periodo { get; set; }
        public string LecturaAnterior { get; set; }
        public string LecturaActual { get; set; }
        public int Consumo { get; set; }
        public float  SubTotal { get; set; }
        public float Descuento { get; set; }
        public float IVA { get; set; }
        public float Total { get; set; }
        public List<DetallePrefactura> Detalle = new List<DetallePrefactura>();
    }

    public class DetallePrefactura
    {
        public string Descripcion { get; set; }
        public float Precio { get; set; }
        public float Cantidad { get; set; }
        public float Valor { get; set; }
    }
}
