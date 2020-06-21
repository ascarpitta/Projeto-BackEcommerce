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
        public void EnviarEmail(string para, string assunto, string conteudo, string caminhoAnexo = null)
        {
            string de = "suporte@ecommerce.com.br";
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("scarpittaa@gmail.com", "23071309!As"),
                EnableSsl = true
            };
            MailMessage mailMessage = new MailMessage(de, para, assunto, conteudo);
            if (caminhoAnexo != null)
            {
                mailMessage.Attachments.Add(new Attachment(caminhoAnexo));
            }
            client.Send(mailMessage);
        }
    }
}
