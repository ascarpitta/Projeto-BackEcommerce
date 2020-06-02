using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Repository.Interfaces
{
    public interface IEmailRepository
    {
        void EnviarEmail(string de, string para, string assunto, string conteudo);
    }
}
