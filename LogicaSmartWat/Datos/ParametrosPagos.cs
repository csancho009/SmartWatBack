using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat.Datos
{
    public class ParametrosPagos
    {
        public int NunCoti { get; set; }
        public int Usuario { get; set; }
        public float EfectivoReal { get; set; }
        public float Transfer { get; set; }
        public int Cuenta { get; set; }
        public string NumTransfer { get; set; }
        public float Tarjeta { get; set; }
        public int Datafono { get; set; }
        public string Voucher { get; set; }
        public string BDCia { get; set; }
    }
}
