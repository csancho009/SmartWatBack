using LogicaSmartWat.Datos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat
{
    public class TarifaController
    {
        public Respuesta ObtenerTarifas(string BaseDeDatos)
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
                    var tarifasT = from T in db.TARIFAS select new { T.ID_TAR, T.NOMBRE, T.RESIDENCIAL, T.COMERCIAL };
                    R.Codigo = 75;
                    R.Mensaje = "Ok";
                    R.Objeto = tarifasT.ToList();
                    db.Database.Connection.Close();
                }
            } catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta ObtenerPrecioTarifa(string BaseDeDatos, string CodigoTarifa)
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
                    var tarifasT = from T in db.TARIFAS.Where(d=>d.ID_TAR== CodigoTarifa) select new { T.ID_TAR, T.NOMBRE, T.RESIDENCIAL, T.COMERCIAL };
                    R.Codigo = 0;
                    R.Mensaje = "Ok";
                    R.Objeto = tarifasT.ToList();
                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public Respuesta ActualizarTarifas(List<TARIFAS> tarifas, string BaseDeDatos)
        {
            Respuesta R = new Respuesta();
            List<object> respuestas  = new List<object>();
           try
            {
                foreach (var tarifa in tarifas)
                {
                    using (POLTAEntities db = new POLTAEntities())
                    {
                        if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                        {
                            db.Database.Connection.Open();
                        }
                        db.Database.Connection.ChangeDatabase(BaseDeDatos);
                        var tarifaResultado = db.TARIFAS.Find(tarifa.ID_TAR);

                        if (tarifaResultado != null)
                        {
                            respuestas.Add(ValidarPrecios(tarifa, tarifaResultado));
                            db.SaveChanges();
                        }
                        else
                        {
                            var respuesta = new
                            {
                                ID_TAR = tarifa.ID_TAR,
                                Estado = "Tarifa no existe"
                            };
                            respuestas.Add(respuesta);
                        }
                        db.Database.Connection.Close();
                    }
                }
                R.Codigo = 1;
                R.Objeto = respuestas;
            } catch (Exception ex)
            {
                R.Codigo = -1;
                R.Mensaje = "Alerta " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            }
            return R;
        }

        public object ValidarPrecios(TARIFAS tarifa, TARIFAS tarifaResultado)
        {
            var R = new
            {
                ID_TAR = tarifa.ID_TAR,
                Estado = "Tarifas negativas"
            };

            if (tarifa.COMERCIAL > 0 && tarifa.RESIDENCIAL > 0)
            {
                tarifaResultado.ID_TAR = tarifa.ID_TAR;
                tarifaResultado.RESIDENCIAL = tarifa.RESIDENCIAL;
                tarifaResultado.COMERCIAL = tarifa.COMERCIAL;

                R = new
                {
                    ID_TAR = tarifa.ID_TAR,
                    Estado = "Ok"
                };
            };
            return R;
        }

    }
    }

