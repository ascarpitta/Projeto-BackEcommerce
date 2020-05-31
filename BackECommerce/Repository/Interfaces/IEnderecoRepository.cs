using BackECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Repository.Interfaces
{
    public interface IEnderecoRepository
    {
        void CadastroEndereco(Endereco endereco);
        List<Endereco> BuscarEnderecos();
        Endereco BuscarEndereco(string id);
        List<Endereco> BuscarEnderecoPorNome(string nome);
        List<Endereco> BuscarEnderecoPorUsuario(string userId);
        EnderecoViaCep BuscarEnderecoPorCep(string cep);
        Endereco AtualizarEndereco(string userId, string idEndereco, Endereco enderecoNovo);
        void RemoverEndereco(string idEndereco);
    }
}
