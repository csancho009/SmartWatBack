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
                using (POLTAEntities db = new POLTAEntities())
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
                R.Mensaje = "Alerta ObtenerZonas " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) +" Det "+ ex.Message;
            }
            return R;
        }

        public Respuesta IngresarZonas(ZONAS zona, string BDCia)
        {
            Respuesta R = new Respuesta();
            try
            {
                using (POLTAEntities db = new POLTAEntities())
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

        public Respuesta ActualizarZonas(ZONAS zona, String BaseDeDatos)
        {
            Respuesta R = new Respuesta();
            try
            {
                using (POLTAEntities db = new POLTAEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BaseDeDatos);
                    ZONAS zonaE = db.ZONAS.First(b => b.ID_ZON == zona.ID_ZON);
                    if (zonaE != null)
                    {
                        zonaE.ID_ZON = zona.ID_ZON;
                        zonaE.NOMBRE = zona.NOMBRE;
                        db.SaveChanges();

                        R.Codigo = 1;
                        R.Mensaje = "Se ha actualizado con éxito";
                    }
                    else
                    {
                        R.Objeto = zona;
                        R.Codigo = 0;
                        R.Mensaje = "La zona actualizada no existe";
                    }
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.Message +  " " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }

            return R;
        }

        public Respuesta EliminarZona(int IdZona, String BaseDeDatos)
        {
            Respuesta R = new Respuesta();
            try
            {
                using (POLTAEntities db = new POLTAEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BaseDeDatos);
                    ZONAS zonaE = db.ZONAS.First(b => b.ID_ZON == IdZona);

                    if (zonaE != null)
                    {
                        db.Entry(zonaE).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();

                        R.Codigo = 1;
                        R.Mensaje = "Se ha Eliminado con éxito";
                    }
                    else
                    {
                        
                        R.Codigo = -2;
                        R.Mensaje = "La zona no existe";
                    }
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.Message + " " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }

            return R;
        }

    }
}
