using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using LogicaSmartWat.Datos;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace SmartWatBack.Models
{
    public class CorreoElectronico
    {
        //@"\\server\c$\Aplicaciones\client_secret_782852003339-2e3ic79oh138hgq1krqatoidfn7agt7j.apps.googleusercontent.com.json
        public Respuesta EnviarCorreo(string Destinatario, string ConCopia, string Mensaje, string Asunto, string Adjunto)
        {
            GoogleCredential credential = null;
            try
            {
                // Cargar las credenciales de la cuenta de servicio
                 credential = GoogleCredential.FromFile(@"c:\aplicaciones\correossmartpos-bbdaf3f42832.json")
     .CreateScoped(GmailService.Scope.GmailSend)
     .CreateWithUser("alertas@pcservicecr.com"); // Delegación si está permitido

                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "correossmartpos"
                });

                // Crear el mensaje de correo
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Notificación Documento Electrónico", "alertas@pcservicecr.com"));

                foreach (var correo in Destinatario.Replace(";", ",").Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(correo))
                    {
                        emailMessage.To.Add(new MailboxAddress("", correo.Trim()));
                    }
                }

                foreach (var correo in ConCopia.Replace(";", ",").Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(correo))
                    {
                        emailMessage.Cc.Add(new MailboxAddress("", correo.Trim()));
                    }
                }

                emailMessage.Subject = Asunto;
                emailMessage.Body = new TextPart("html")
                {
                    Text = Mensaje,
                    ContentTransferEncoding = ContentEncoding.QuotedPrintable
                };
                emailMessage.Body.ContentType.Charset = "utf-8";

                var multipart = new Multipart("mixed");
                multipart.Add(emailMessage.Body);

                if (!string.IsNullOrEmpty(Adjunto) && File.Exists(Adjunto))
                {
                    var attachment = new MimePart("application/octet-stream")
                    {
                        Content = new MimeContent(File.OpenRead(Adjunto)),
                        ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(Adjunto)
                    };
                    multipart.Add(attachment);
                }

                emailMessage.Body = multipart;

                var message = new Google.Apis.Gmail.v1.Data.Message
                {
                    Raw = Convert.ToBase64String(Encoding.UTF8.GetBytes(emailMessage.ToString()))
                        .Replace("+", "-")
                        .Replace("/", "_")
                        .Replace("=", "")
                };

                service.Users.Messages.Send(message, "alertas@pcservicecr.com").Execute();
                service.Dispose();
                emailMessage.Dispose();

                return new Respuesta { Codigo = 0, Mensaje = "Correo Enviado" };
            }
            catch (Exception ex)
            {
                return new Respuesta { Codigo = -1, Mensaje = ex.Message + " NL-EnviarCorreo " + ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) };
            }
            finally
            {
                if (credential != null)
                {
                    credential = null;
                }
            }
        }
    }
}