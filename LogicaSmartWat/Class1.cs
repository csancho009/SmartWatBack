using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat
{
    public class Class1
    {

        public object ObtenerTarifas()
        {
            using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities()) {

                var x = from T in db.TARIFAS select new { T.ID_TAR, T.NOMBRE, T.RESIDENCIAL, T.COMERCIAL };
                
                //var lsTarifa = db.TARIFAS.Select(e => new AtributosTarifa
                //{
                //    Id_TAR = e.ID_TAR,
                //    Nombre = e.NOMBRE,
                //    Residencial = (decimal)e.RESIDENCIAL,
                //    Comercial = (decimal)e.COMERCIAL
                //}).ToList();



            return x.ToList();
            }
        }




        public class AtributosTarifa
        {
            private string ID_TAR;
            private string NOMBRE;
            private decimal RESIDENCIAL;
            private decimal COMERCIAL;

            public string Id_TAR { get => ID_TAR; set => ID_TAR = value; }
            public string Nombre { get => NOMBRE; set => NOMBRE = value; }
            public decimal Residencial { get => RESIDENCIAL; set => RESIDENCIAL = value; }
            public decimal Comercial { get => COMERCIAL; set => COMERCIAL = value; }
        }

    }
}
