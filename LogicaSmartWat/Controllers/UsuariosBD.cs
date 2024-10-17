using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using LogicaSmartWat.Datos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LogicaSmartWat.Controllers
{
    public class UsuariosBD
    {
        public Respuesta ListaUsuarios ( string Usuario, string Nombre, string BDCia)
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
                    var Consulta = (from Uss in db.USUARIOS.Where(d => (d.USUARIO.Contains(Usuario) || d.NOMBRE.Contains(Nombre))) select new { Uss.CODIGO, Uss.NOMBRE, Uss.USUARIO }).ToList();
                    return new Respuesta { Codigo = 0, Mensaje = "OK", Objeto = Consulta };
                }
                catch (Exception ex)
                {

                    return new Respuesta { Codigo = -1, Mensaje = "Alerta en ListaUsuarios NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) + " " + ex.Message };
                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }
        }

        public Respuesta RepositorioUsuario(int Codigo, string BDCia)
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
                    var Consulta = (from Uss in db.USUARIOS.Where(d => d.CODIGO==Codigo) select new { 
                        Uss.CODIGO, 
                        Uss.NOMBRE, 
                        Uss.CEDULA,
                        Uss.TELEFONO, Uss.DIRECCION, Uss.USUARIO, CLAVE="",
                        Uss.PERMISO, Uss.ESTADO, Uss.VENDEDOR,Uss.BODEGA}).ToList();
                    return new Respuesta { Codigo = 0, Mensaje = "OK", Objeto = Consulta };
                }
                catch (Exception ex)
                {

                    return new Respuesta { Codigo = -1, Mensaje = "Alerta en RepositorioUsuario NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) + " " + ex.Message };
                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }
        }

        
        public async Task<Respuesta> GuardarUsuario(USUARIOS U, string BDCia, int MiIdUsuario)
        {
            //if (String.IsNullOrEmpty(U.DIRECCION))
            //{
            //    return new Respuesta { Codigo = -1, Mensaje = "Serie de usuario no fue guardada" };
            //}
            using (POLTAEntities db = new POLTAEntities())
            {
                if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                {
                    db.Database.Connection.Open();
                }
                db.Database.Connection.ChangeDatabase(BDCia);
                var MiUsuario = db.USUARIOS.Where(d => d.CODIGO == MiIdUsuario).FirstOrDefault();
                try
                {
                    if (U.CODIGO == 0)
                    {
                        var UsuarioExistente = db.USUARIOS.Where(d => d.USUARIO == U.USUARIO).FirstOrDefault();
                        if (UsuarioExistente!= null)
                        {
                            return new Respuesta { Codigo = -7, Mensaje = "Usuario ya se tiene asignado para "+ UsuarioExistente.NOMBRE };
                        }
                        string RespGoogle;
                        if (FirebaseAuth.DefaultInstance == null)
                        {
                            FirebaseApp.Create(new AppOptions()
                            {
                                Credential = GoogleCredential.FromFile("C:/FireBase/polta-51fff-firebase-adminsdk-63j0j-4b77570fb4.json"),
                            });
                        }
                        var auth = FirebaseAuth.DefaultInstance;
                        try
                        {
                            // Create the user with email and password
                            UserRecordArgs args = new UserRecordArgs()
                            {
                                Email = U.CEDULA,
                                Password = U.CLAVE
                            };
                            UserRecord userRecord = await  auth.CreateUserAsync(args).ConfigureAwait(false);
                            RespGoogle= userRecord.Uid;
                        }
                        catch (FirebaseAuthException ex)
                        {
                            return new Respuesta { Codigo = -6, Mensaje = ex.Message };
                        }

                        //var RespGoogle = await UsuarioNuevoFireBase(U.CEDULA, U.CLAVE);

                        if (RespGoogle.Contains("Respuesta Proveedor"))
                        {
                            return new Respuesta { Codigo = -6, Mensaje = RespGoogle };
                        }
                        else
                        {
                            U.DIRECCION = RespGoogle;
                        }

                        System.Data.Entity.Core.Objects.ObjectParameter P = new System.Data.Entity.Core.Objects.ObjectParameter("new_identity",1);
                        var Ds = db.NuevoUsuario(U.NOMBRE, U.CEDULA, U.TELEFONO, U.DIRECCION,
                            U.USUARIO, U.CLAVE, U.PERMISO, U.ESTADO, U.VENDEDOR, U.BODEGA,P);
                        int CodNuevo = 0;
                        foreach( var D in Ds)
                        {
                            CodNuevo = D.Value;
                        }
                        db.AplicarPermisoUsuarioRol(CodNuevo, U.PERMISO);
                        return new Respuesta { Codigo = CodNuevo, Mensaje = "OK", Objeto= RespGoogle };
                    }
                    else
                    {
                        db.Entry(U).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        db.AplicarPermisoUsuarioRol(U.CODIGO, U.PERMISO);
                        return new Respuesta { Codigo = U.CODIGO, Mensaje = "OK", Objeto = MiUsuario.DIRECCION };
                    }
                    
                }
                catch (Exception ex)
                {
                    return new Respuesta { Codigo = -1, Mensaje = "Alerta en GuardarUsuario NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) + " " + ex.Message };
                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }
        }

        
        static async Task<string> UsuarioNuevoFireBase (string email, string password)
        {
            try
            {
                // Create the user with email and password
                UserRecordArgs args = new UserRecordArgs()
                {
                    Email = email,
                    Password = password
                };
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
                return userRecord.Uid;
            }
            catch (FirebaseAuthException ex)
            {
                return "Respuesta Proveedor Autenticación "+ ex.Message;
            }
        }
        
        public Respuesta ListaRoles(string BDCia)
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
                    var Consulta = (from Cnt in db.ROLES select new { Cnt.CODIGO, Cnt.NOMBRE}).ToList();
                    return new Respuesta { Codigo = 0, Mensaje = "OK", Objeto = Consulta };
                }
                catch (Exception ex)
                {

                    return new Respuesta { Codigo = -1, Mensaje = "Alerta en ListaRoles NL " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) + " " + ex.Message };
                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }
        }
    }
}
