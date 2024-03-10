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
        public Respuesta ObtenerBloques(int id_zon, string BDCia)
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
                    R.Codigo = 75;
                    R.Mensaje = "Ok";
                    R.Objeto = db.Sp_ObtenerBloques(id_zon).ToList();
                    db.Database.Connection.Close();
                }
            }
            catch(Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta IngresarBloques(BLOQUES bloque, string BDCia)
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
                    db.Database.Connection.Close();
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

        public Respuesta ActualizarBloques(BLOQUES bloque, string BaseDeDatos)
        {
            Respuesta R = new Respuesta();
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    db.Database.Connection.ChangeDatabase(BaseDeDatos);
                    BLOQUES bloqueE = db.BLOQUES.First(b => b.ID_BLO == bloque.ID_BLO);
                    if (bloqueE != null)
                    {
                        bloqueE.NOMBRE = bloque.NOMBRE;
                        bloqueE.ID_ZON = bloque.ID_ZON;
                        db.SaveChanges();

                        R.Codigo = 1;
                        R.Mensaje = "Se ha actualizado con éxito";
                    }
                    else
                    {
                        R.Objeto = bloque;
                        R.Codigo = 0;
                        R.Mensaje = "La zona actualizada no existe";
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
