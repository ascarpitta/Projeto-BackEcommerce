using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using BackECommerce.Repository.Interfaces;

namespace BackECommerce.Repository.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        public EmailRepository() { }

        public void EnviarEmail(string para, string assunto, string conteudo, string de = "suporte@ecommerce.com.br", string caminhoAnexo = null)
        {
            try
            {
                var client = new SmtpClient("smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("c50fa06de5aace", "019fdd06cb1b43"),
                    EnableSsl = true
                };
                MailMessage mailMessage = new MailMessage(de, para, assunto, conteudo);
                mailMessage.Attachments.Add(new Attachment(caminhoAnexo));
                client.Send(de, para, assunto, conteudo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar email: " + ex.Message);
            }
        }
    }
}
