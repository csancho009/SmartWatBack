using LogicaSmartWat.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSmartWat.Controllers
{
    public class GestionClientes
    {
        public object UnCliente(int Codigo, string BDCia)
        {
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    var R = from C in db.CLIENTES.Where(d => d.CODIGO == Codigo)
                            select new
                            {
                                C.CODIGO,
                                C.NOMBRE,
                                C.DIRECCION,
                                C.TELEFONO,
                                C.CORREO,
                                C.TIPO,
                                C.IDENTIFICACION,
                                C.PRECIO,
                                C.LIMITE,
                                C.FRECUENCIA,
                                C.ESTADO,
                                C.PROVINCIA,
                                C.CANTON,
                                C.DISTRITO,
                                C.BARRIO,
                                C.NombreComercial,
                                C.CELULAR,
                                C.DIAS,
                                C.DE_TASA_IMP,
                                C.NUMDOCEXO,
                                C.INSTIEXO,
                                C.FECHAEXO,
                                C.PROCENTAJEEXO
                            };
                    return new { Codigo = 1, Mensaje = "OK", Objeto = R };
                }
            }
            catch (Exception ex)
            {
                return new { Codigo = -1, Mensaje = ex.Message + " NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException };
            }

        }

        public object ListaClientes(ParametroBusquedaCLiente P)
        {
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(P.BDCia);
                    int NumSaliente = 0;
                    int.TryParse(P.Codigo, out NumSaliente);
                    var R = from C in db.CLIENTES.Where(d => (P.NombreCliente.Length > 0 && d.NOMBRE.Contains(P.NombreCliente)) || (P.Cedula.Length > 0 && d.IDENTIFICACION.Contains(P.Cedula)) || (NumSaliente > 0 && d.CODIGO == NumSaliente))
                            select new
                            {
                                C.CODIGO,
                                C.NOMBRE,
                                C.IDENTIFICACION
                            };
                    return new { Codigo = 1, Mensaje = "OK", Objeto = R };
                }
            }
            catch (Exception ex)
            {
                return new { Codigo = -1, Mensaje = ex.Message + " NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException };
            }

        }

        public Object NuevoCliente(CLIENTES C, string BDCia)
        {
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    db.CLIENTES.Add(C);
                    db.SaveChanges();
                    return new { Codigo = C.CODIGO, Mensaje = "OK" };
                }
            }
            catch (Exception ex)
            {
                return new { Codigo = -1, Mensaje = ex.Message + " NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException };
            }
        }

        public object ParametrosInicialesCarga(string BDCia)
        {
            Respuesta R = new Respuesta();
            ArregloProvCantDist Ubicacion = new ArregloProvCantDist();
            try
            {
                using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    db.Database.Connection.ChangeDatabase(BDCia);
                    CIA E = db.CIA.FirstOrDefault();

                    var P = from Ps in db.PROVINCIAS select new { Ps.CODIGO, Ps.NOMBRE };
                    Ubicacion.Pro = new Respuesta { Codigo = (int)E.PROVINCIA, Mensaje = "OK", Objeto = P };
                    var C = from Cs in db.CANTONES.Where(d => d.PROVINCIA == E.PROVINCIA) select new { Cs.CODIGO, Cs.NOMBRE };
                    Ubicacion.Can = new Respuesta { Codigo = (int)E.CANTON, Mensaje = "OK", Objeto = C };
                    var D = from Ds in db.DISTRITOS.Where(d => d.PROVINCIA == E.PROVINCIA && d.CANTON == E.CANTON) select new { Ds.CODIGO, Ds.NOMBRE };
                    Ubicacion.Dis = new Respuesta { Codigo = (int)E.DISTRITO, Mensaje = "OK", Objeto = D };
                    var Pcs = from Pc in db.LISTA_PRECIOS select new { Pc.CODIGO, Pc.NOMBRE };
                    Ubicacion.ListasPrecios = new Respuesta { Codigo = 0, Mensaje = "OK", Objeto = Pcs };
                    R.Codigo = 0;
                    R.Mensaje = "OK";
                    R.Objeto = Ubicacion;
                    return R;
                }
            }
            catch (Exception ex)
            {
                return new { Codigo = -1, Mensaje = ex.Message + " NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException };
            }
        }

        public object GetCliente(string nombre)
        {
            using (POLTA_PRUEBASEntities db = new POLTA_PRUEBASEntities())
            {
                if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                {
                    db.Database.Connection.Open();
                }
                var listC = db.Sp_BuscarCliente(nombre);
                return listC;
            }
        }
    }

    public class ArregloProvCantDist
    {
        public Respuesta Pro = new Respuesta();
        public Respuesta Can = new Respuesta();
        public Respuesta Dis = new Respuesta();
        public Respuesta ListasPrecios = new Respuesta();
    }
    public class ParametroBusquedaCLiente
    {
        public string Codigo { get; set; }
        public string NombreCliente { get; set; }
        public string Cedula { get; set; }
        public string BDCia { get; set; }
    }

}
