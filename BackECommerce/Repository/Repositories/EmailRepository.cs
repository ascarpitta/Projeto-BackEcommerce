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
            var smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;

            smtp.Credentials = new NetworkCredential("projetos.mack2020@gmail.com", "mackenzista2020");
            MailMessage mailMessage = new MailMessage("projetos.mack2020@gmail.com", para, assunto, conteudo);
            
            if (caminhoAnexo != null)
            {
                mailMessage.Attachments.Add(new Attachment(caminhoAnexo));
            }
            smtp.Send(mailMessage);
            smtp.Dispose();
            mailMessage.Dispose();
        }
    }
}
