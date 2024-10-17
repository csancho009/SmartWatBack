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
                using (POLTAEntities db = new POLTAEntities())
                {
                    try
                    {
                        if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                        {
                            db.Database.Connection.Open();
                        }
                        db.Database.Connection.ChangeDatabase(BDCia);
                        R.Codigo = 75;
                        R.Mensaje = "Ok";
                        R.Objeto = db.Sp_ObtenerBloques(id_zon).ToList();
                    }
                    catch (Exception)
                    {

                        
                    }
                    finally
                    {
                        db.Database.Connection.Close();
                    }

                }
            }
            catch(Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta TodosBloques( string BDCia)
        {
            Respuesta R = new Respuesta();
            try
            {
                using (POLTAEntities db = new POLTAEntities())
                {
                    try
                    {
                        if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                        {
                            db.Database.Connection.Open();
                        }
                        db.Database.Connection.ChangeDatabase(BDCia);
                        var Bs = from B in db.BLOQUES join Z in db.ZONAS.OrderBy(o => o.NOMBRE) on B.ID_ZON equals Z.ID_ZON select new { B.ID_BLO, Nombre = B.NOMBRE, Z.ID_ZON };
                        R.Objeto = Bs.ToList();
                        R.Codigo = 0;
                        R.Mensaje = "Ok";
                    }
                    catch (Exception)
                    {

                      
                    }
                    finally
                    {
                        db.Database.Connection.Close();
                    }
                   
                   
                }
            }
            catch (Exception ex)
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
                using (POLTAEntities db = new POLTAEntities())
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
                using (POLTAEntities db = new POLTAEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
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
                R.Mensaje = "Alerta  " + ex.Message + "  " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta EliminarBloque(int IdBloque, String BaseDeDatos)
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
                    BLOQUES Bloque = db.BLOQUES.First(b => b.ID_BLO == IdBloque);

                    if (Bloque != null)
                    {
                        db.Entry(Bloque).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();

                        R.Codigo = 1;
                        R.Mensaje = "Se ha Eliminado con éxito";
                    }
                    else
                    {

                        R.Codigo = -2;
                        R.Mensaje = "El Bloque no existe";
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
