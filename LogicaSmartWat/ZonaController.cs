using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat
{
    public class ZonaController
    {
        public object ObtenerZonas()
        {
             using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
            {
                var zonas = from T in db.ZONAS select new { T.ID_ZON, T.NOMBRE};
                return zonas.ToList();
            }
            return null;
        }

        public object IngresarZonas(ZONAS zona)
        {
            using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
            {
                db.ZONAS.Add(zona);
                db.SaveChanges();

                return "Ingreso de zona correctamente";
            }
            return null;
        }

    }
}
