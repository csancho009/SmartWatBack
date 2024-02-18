using LogicaSmartWat.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat
{
    public class ZonaController
    {
        public object ObtenerZonas(string BDCia)
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
                    var zonas = from T in db.ZONAS select new { T.ID_ZON, T.NOMBRE };
                    R.Codigo = 75;
                    R.Mensaje = "Ok";
                    R.Objeto = zonas.ToList();
                    db.Database.Connection.Close();
                }
            } catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta IngresarZonas(ZONAS zona, string BDCia)
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
                    bool zonaE = db.ZONAS.Any(b => b.NOMBRE == zona.NOMBRE);
                    if (!zonaE)
                    {
                        db.ZONAS.Add(zona);
                        db.SaveChanges();

                        R.Codigo = 1;
                        R.Mensaje = "Se ha insertado con exito";
                    }
                    else
                    {
                        R.Objeto = zona;
                        R.Codigo = 0;
                        R.Mensaje = "La zona ingresada ya existe";
                    }
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
