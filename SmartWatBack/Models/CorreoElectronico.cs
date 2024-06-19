using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SmartWatBack.Models
{
    public class CorreoElectronico
    {
        public bool EnviarCorreo(string Destinatario, string ConCopia, string Mensaje, string Asunto, string  Adjunto)
        {
            System.Net.Mail.MailMessage MMessage = new System.Net.Mail.MailMessage();
            SmtpClient SClient = new SmtpClient();

            try
            {

                MMessage.To.Add(Destinatario);
                if (!string.IsNullOrEmpty(ConCopia)) { 
                MMessage.CC.Add(ConCopia);
                }
                MMessage.From = new MailAddress("alertas@pcservicecr.com", "Asada Polta", System.Text.Encoding.UTF8);
                MMessage.Subject = Asunto;
                MMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                MMessage.Body = Mensaje;
                MMessage.BodyEncoding = System.Text.Encoding.UTF8;
                MMessage.IsBodyHtml = true;    // Formato texto plano

                if (!string.IsNullOrEmpty(Adjunto))
                {
                   
                        MMessage.Attachments.Add(new Attachment(Adjunto));
                }



                SClient.Credentials = new System.Net.NetworkCredential("alertas@pcservicecr.com", "Cambio123");
                SClient.Host = "smtp.gmail.com"; // Servidor SMTP de Gmail
                SClient.Port = 587; // Puerto del SMTP de Gmail
                SClient.EnableSsl = true;   // Habilita el SSL, necesio en Gmail
                                            // Capturamos los errores en el envio
                SClient.Send(MMessage);
                SClient.Dispose();
                MMessage.Dispose();
                SClient = null;
                MMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Daily user sending quota exceeded") | ex.Message.Contains("Error al enviar correo."))
                {
                    try
                    {
                        MMessage.From = new MailAddress("compras@pcservicecr.com", "Asada Polta", System.Text.Encoding.UTF8);
                        SClient.Credentials = new System.Net.NetworkCredential("compras@pcservicecr.com", "Joker1983");
                        SClient.Send(MMessage);
                        SClient.Dispose();
                        MMessage.Dispose();
                        SClient = null;
                        MMessage = null;
                        return true;
                    }
                    catch (Exception exf)
                    {
                        if (exf.Message.Contains("Daily user sending quota exceeded") | ex.Message.Contains("Error al enviar correo."))
                        {
                            MMessage.From = new MailAddress("info@pcservicecr.com", "Asada Polta", System.Text.Encoding.UTF8);
                            SClient.Credentials = new System.Net.NetworkCredential("info@pcservicecr.com", "Joker1983");
                            SClient.Send(MMessage);
                            SClient.Dispose();
                            MMessage.Dispose();
                            SClient = null;
                            MMessage = null;
                            return true;
                        }
                        return false;
                    }
                }
                else
                    return false;
            }
        }
    }
}