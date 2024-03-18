using LogicaSmartWat.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat.Controllers
{
    public class Lecturas
    {
        public Respuesta IngresaLectura(LECTURAS L, string BDCia)
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
                    L.FECHA = DateTime.Now;
                    db.LECTURAS.Add(L);
                    db.SaveChanges();

                    R.Codigo = L.ID_LEC;
                    R.Mensaje = "Ok";                    
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta IngresaLectura " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);

            }

            return R;
        }

        public Respuesta ActualizaLectura(int NumLectura, int Consumo, int usuario, string BDCia)
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

                    LECTURAS L = db.LECTURAS.Where(d => d.ID_LEC == NumLectura).FirstOrDefault();
                    if (L.ESTADO=="PEN" || L.ESTADO == "COT")
                    {
                        db.Entry(L).State = System.Data.Entity.EntityState.Modified;
                        L.LECTURA = Consumo;
                        L.ID_USU = usuario;
                        db.SaveChanges();
                        if (L.ESTADO == "COT")
                        {
                            db.RECTIFICA_LECTURA_MEDIDOR(usuario, L.ID_LEC);
                        }
                        R.Codigo = L.ID_LEC;
                        R.Mensaje = "Ok";
                    }
                    else
                    {
                        R.Codigo = -2;
                        R.Mensaje = "Esa lectura no puede cambiarse";
                    }
                    
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta ActualizaLectura " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta TablaLecturas(int mes, int annio, string BDCia)
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

                    var Repos = db.TablaLecturas(mes, annio);
                    R.Codigo = 0;
                    R.Mensaje = "Ok";
                    R.Objeto = Repos.ToList();
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta TablaLecturas " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }
        public Respuesta GenerarCobro(int Usuario, string BDCia)
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

                    var Repos = db.GENERA_FACTURACION(Usuario);
                    R.Codigo = 0;
                    R.Mensaje = "Ok";
                    R.Objeto = Repos.ToList();
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta GenerarCobro " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }
    }
}
