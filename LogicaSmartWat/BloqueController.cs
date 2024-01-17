using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat
{
    public class BloqueController
    {
        public object ObtenerBloques()
        {
            using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
            {
                return db.ObtenerBloques().ToList();
            }
        }

        public object IngresarBloques(BLOQUES bloque)
        {
            using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
            {
                db.BLOQUES.Add(bloque);
                db.SaveChanges();
                return "Bloque ingresado";
            }
        }
    }
}
