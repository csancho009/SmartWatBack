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

        public Respuesta ObtenerPajas(string paja, string BDCia)
        {
            Respuesta R = new Respuesta();

            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    R.Codigo = 0;
                    R.Mensaje = "Ok";
                    R.Objeto = db.Sp_BuscarPaja(paja).ToList();
                    db.Database.Connection.Close();
                }
            } catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta IngresarPaja(PAJAS paja, string BDCia) 
        {
            Respuesta R = new Respuesta();

            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    paja.FECHA_INSTALACION = DateTime.Now;
                    db.PAJAS.Add(paja);
                    db.SaveChanges();

                    R.Codigo = 75;
                    R.Mensaje = "Ok";
                    R.Objeto = paja;
                    db.Database.Connection.Close();
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
