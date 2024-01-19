using LogicaSmartWat.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat
{
    public class BloqueController
    {
        public Respuesta ObtenerBloques(int id_zon)
        {
            Respuesta R = new Respuesta();
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    R.Codigo = 75;
                    R.Mensaje = "Ok";
                    R.Objeto = db.Sp_ObtenerBloques(id_zon).ToList();
                }
            }
            catch(Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta IngresarBloques(BLOQUES bloque)
        {
            Respuesta R = new Respuesta();
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    bool bloqueE = db.BLOQUES.Any(b => b.NOMBRE == bloque.NOMBRE &&  b.ID_ZON == bloque.ID_ZON);
                    if (!bloqueE)
                    {
                        db.BLOQUES.Add(bloque);
                        db.SaveChanges();
                        R.Codigo = 75;
                        R.Mensaje = "Se ha ingresado con exito";
                    }
                    else
                    {
                        R.Codigo = 0;
                        R.Mensaje = "El bloque ingresado ya existe";
                        R.Objeto = bloque;
                    }
                }
                return R;
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }
    }
}
