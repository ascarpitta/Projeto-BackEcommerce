using BackECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Service.Interfaces
{
    public interface IEnderecoService
    {
        List<Endereco> GetEndereco();
        Endereco GetEnderecoById(string id);
        List<Endereco> GetEnderecoByName(string nome);
        List<Endereco> GetEnderecoByUser(string userId);
        Endereco CreateEndereco(Endereco endereco);
        void UpdateEndereco(string id, Endereco enderecoNovo);
        void RemoveEndereco(string enderecoId);
    }
}
