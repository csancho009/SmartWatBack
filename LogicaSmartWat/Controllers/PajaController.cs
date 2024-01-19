using LogicaSmartWat.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat
{
    public class PajaController
    {

        public Respuesta ObtenerPajas(string paja)
        {
            Respuesta R = new Respuesta();

            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    R.Codigo = 0;
                    R.Mensaje = "Ok";
                    R.Objeto = db.Sp_BuscarPaja(paja).ToList();
                }
            } catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta IngresarPaja(PAJAS paja) 
        {
            Respuesta R = new Respuesta();

            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    paja.FECHA_INSTALACION = DateTime.Now;
                    db.PAJAS.Add(paja);
                    db.SaveChanges();

                    R.Codigo = 75;
                    R.Mensaje = "Ok";
                    R.Objeto = paja;
                }
            } catch (Exception ex) 
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }

            return R;
        }


    }
}
