using LogicaSmartWat.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat.Controllers
{
    public  class BodegasBD
    {

        public Respuesta ListaBodegas( string BDCia)
        {
            using (POLTAEntities db = new POLTAEntities())
            {
                if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                {
                    db.Database.Connection.Open();
                }
                db.Database.Connection.ChangeDatabase(BDCia);
                try
                {
                    var Consulta = (from Consul in db.BODEGAS select new { Consul.CODIGO, Consul.NOMBRE}).ToList();
                    return new Respuesta { Codigo = 0, Mensaje = "OK", Objeto = Consulta };
                }
                catch (Exception ex)
                {

                    return new Respuesta { Codigo = -1, Mensaje = "Alerta en ListaBodegass NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) + " " + ex.Message };
                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }

        }

    }
}
