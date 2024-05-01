using LogicaSmartWat.Datos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
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
                    db.Database.Connection.Close();
                    return new { Codigo = 1, Mensaje = "OK", Objeto = R.ToList() };
                }
            }
            catch (Exception ex)
            {
                return new { Codigo = -1, Mensaje = ex.Message + " UnCliente NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException };
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
                    var R = from C in db.CLIENTES.Where(d => ( (P.NombreCliente.Length > 0 && d.NOMBRE.Contains(P.NombreCliente)) || (P.Cedula.Length > 0 && d.IDENTIFICACION.Contains(P.Cedula)) || (NumSaliente > 0 && d.CODIGO == NumSaliente)))
                            select new
                            {
                                C.CODIGO,
                                C.NOMBRE,
                                C.IDENTIFICACION
                            };
                    db.Database.Connection.Close();
                    return new { Codigo = 1, Mensaje = "OK", Objeto = R.ToList() };
                }
            }
            catch (Exception ex)
            {
                return new { Codigo = -1, Mensaje = ex.Message + " ListaClientes NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException };
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
                    db.Database.Connection.Close();
                    return new { Codigo = C.CODIGO, Mensaje = "OK" };
                }
            }
            catch (DbEntityValidationException ex)
            {
                return new { Codigo = -1, Mensaje = ex.Message + " NuevoCliente NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.EntityValidationErrors.ToString() };
            }
        }

        public Object ActualizaCliente(CLIENTES C, string BDCia)
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
                    CLIENTES CL = db.CLIENTES.Where(d => d.CODIGO == C.CODIGO).FirstOrDefault();
                    db.Entry(CL).State = System.Data.Entity.EntityState.Modified;
                    CL.NOMBRE = C.NOMBRE;
                    CL.DIRECCION = C.DIRECCION;
                    CL.TELEFONO = C.TELEFONO;
                    CL.CORREO = C.CORREO;
                    CL.TIPO = C.TIPO;
                    CL.IDENTIFICACION = C.IDENTIFICACION;
                    CL.PRECIO = C.PRECIO;
                    CL.LIMITE = C.LIMITE;
                    CL.FRECUENCIA = C.FRECUENCIA;
                    CL.ESTADO = C.ESTADO;
                    CL.PROVINCIA = C.PROVINCIA;
                    CL.CANTON = C.CANTON;
                    CL.DISTRITO = C.DISTRITO;
                    CL.BARRIO = C.BARRIO;
                    CL.NombreComercial = C.NombreComercial;
                    CL.CELULAR = C.CELULAR;
                    CL.DIAS = C.DIAS;
                    CL.DE_TASA_IMP = C.DE_TASA_IMP;
                    CL.NUMDOCEXO = C.NUMDOCEXO;
                    CL.INSTIEXO = C.INSTIEXO;
                    CL.FECHAEXO = C.FECHAEXO;
                    CL.PROCENTAJEEXO = C.PROCENTAJEEXO;
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return new { Codigo = C.CODIGO, Mensaje = "OK" };
                }
            }
            catch (Exception ex)
            {
                return new { Codigo = -1, Mensaje = ex.Message + " ActualizaCliente NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException.ToString() };
            }
        }
        public Object ConsultaCedula(string Dato, string BDCia)
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
                    CLIENTES Cl = db.CLIENTES.Where(d => d.IDENTIFICACION == Dato).FirstOrDefault();
                    db.Database.Connection.Close();
                    if (Cl != null)
                    {
                        return new { Codigo = 0, Mensaje = "Cliente Ya Existe", Objeto = Cl.NOMBRE };
                    }
                    else
                    {
                        HttpClient http = new HttpClient();                        
                        string URL = "https://api.hacienda.go.cr/fe/ae?identificacion=" + Dato;
                        HttpResponseMessage response = http.GetAsync(URL).Result;
                        string res = response.Content.ReadAsStringAsync().Result;
                        try
                        {
                            Root Hac = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(res);
                            if (Hac.nombre!= null)
                            {
                                return new { Codigo = 0, Mensaje = "OK", Objeto = Hac.nombre };
                            }
                            else
                            {
                                return new { Codigo = -4, Mensaje = "Cédula incorrecta. Omita guiones o espacios", Objeto = "" };
                            }
                        }
                        catch (Exception)
                        {
                           
                            return new { Codigo = -3, Mensaje = "Cédula incorrecta. Omita guiones o espacios", Objeto = ""};
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                return new { Codigo = -1, Mensaje = ex.Message + " ConsultaCedula NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException };
            }
        }

        public Object ListaGeneralClientes( string BDCia)
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
                    var Lista = from Cs in  db.CLIENTES.OrderBy(O=>O.NOMBRE) select new { value= Cs.CODIGO, label=Cs.NOMBRE };
                    db.Database.Connection.Close();

                    return Lista.ToList();
                }
            }
            catch (Exception ex)
            {
                return new { value = -1, label = ex.Message + " ConsultaCedula NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException };
            }
        }

        public Respuesta ParametrosInicialesCarga(string BDCia)
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
                    Ubicacion.Pro = new Respuesta { Codigo = (int)E.PROVINCIA, Mensaje = "OK", Objeto = P.ToList() };
                    var C = from Cs in db.CANTONES.Where(d => d.PROVINCIA == E.PROVINCIA) select new { Cs.CODIGO, Cs.NOMBRE };
                    Ubicacion.Can = new Respuesta { Codigo = (int)E.CANTON, Mensaje = "OK", Objeto = C.ToList() };
                    var D = from Ds in db.DISTRITOS.Where(d => d.PROVINCIA == E.PROVINCIA && d.CANTON == E.CANTON) select new { Ds.CODIGO, Ds.NOMBRE };
                    Ubicacion.Dis = new Respuesta { Codigo = (int)E.DISTRITO, Mensaje = "OK", Objeto = D.ToList() };
                    var Pcs = from Pc in db.LISTA_PRECIOS select new { Pc.CODIGO, Pc.NOMBRE };
                    Ubicacion.ListasPrecios = new Respuesta { Codigo = 0, Mensaje = "OK", Objeto = Pcs.ToList() };
                    R.Codigo = 0;
                    R.Mensaje = "OK";
                    R.Objeto = Ubicacion;
                    db.Database.Connection.Close();
                    return R;
                }
            }
            catch (Exception ex)
            {
                return new Respuesta { Codigo = -1, Mensaje = ex.Message + " ParametrosInicialesCarga NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException };
            }
        }

        public object Cantones(string BDCia, int Provincia)
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
                    var P = from Ps in db.CANTONES.Where(d => d.PROVINCIA == Provincia).OrderBy(O => O.NOMBRE) select new { Ps.CODIGO, Ps.NOMBRE };
                    R.Codigo = 0;
                    R.Mensaje = "OK";
                    R.Objeto = P.ToList();
                    db.Database.Connection.Close();
                    return R;
                }
            }
            catch (Exception ex)
            {

                return new Respuesta { Codigo = -1, Mensaje = ex.Message + " NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException };
            }
        }

        public object Distritos(string BDCia, int Provincia, int Canton)
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
                    var P = from Ps in db.DISTRITOS.Where(d => d.PROVINCIA == Provincia && d.CANTON==Canton).OrderBy(O => O.NOMBRE) select new { Ps.CODIGO, Ps.NOMBRE };
                    R.Codigo = 0;
                    R.Mensaje = "OK";
                    R.Objeto = P.ToList();
                    db.Database.Connection.Close();
                    return R;
                }
            }
            catch (Exception ex)
            {

                return new Respuesta { Codigo = -1, Mensaje = ex.Message + " NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7), Objeto = ex.InnerException };
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

    public class Regimen
    {
        public int codigo { get; set; }
        public string descripcion { get; set; }
    }

    public class Root
    {
        public string nombre { get; set; }
        public string tipoIdentificacion { get; set; }
        public Regimen regimen { get; set; }
        public Situacion situacion { get; set; }
        public List<object> actividades { get; set; }
    }

    public class Situacion    {
        public string moroso { get; set; }
        public string omiso { get; set; }
        public string estado { get; set; }
        public string administracionTributaria { get; set; }
        public string mensaje { get; set; }
    }

    public class RootError
    {
        public int code { get; set; }
        public string status { get; set; }
    }

}
