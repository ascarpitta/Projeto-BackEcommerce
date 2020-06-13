using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using BackECommerce.Service.Services;

namespace BackECommerce.Repository.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly EnderecoService _enderecoService = new EnderecoService();
        private readonly ICarrinhoRepository _carrinhoRepository = new CarrinhoRepository();
        
        public Endereco AtualizarEndereco(string userId, string idEndereco, Endereco enderecoNovo)
        {
            var end = BuscarEndereco(idEndereco);
            if (end == null)
            {
                return null;
            }
            else
            {
                if (userId == end.User)
                {
                    _enderecoService.UpdateEndereco(idEndereco, enderecoNovo);
                    return BuscarEndereco(idEndereco);
                }
            }
            return null;
            
        }

        public Endereco BuscarEndereco(string id)
        {
            return _enderecoService.GetEnderecoById(id);
        }

        public EnderecoViaCep BuscarEnderecoPorCep(string cep)
        {
            EnderecoViaCep enderecoViaCep = new EnderecoViaCep();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + cep + "/json/");
            request.AllowAutoRedirect = false;
            HttpWebResponse ChecaServidor = (HttpWebResponse)request.GetResponse();

            if (ChecaServidor.StatusCode != HttpStatusCode.OK)
            {
                return enderecoViaCep;
            }

            enderecoViaCep.Cep = cep;

            using (Stream webStream = ChecaServidor.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        response = Regex.Replace(response, "[{},]", string.Empty);
                        response = response.Replace("\"", "");

                        String[] substrings = response.Split('\n');

                        int cont = 0;
                        foreach (var substring in substrings)
                        {
                            string[] valor = substring.Split(":".ToCharArray());
                            if (cont == 2)
                            {
                                enderecoViaCep.Rua = valor[1];
                            }
                            if (cont == 3) { 
                                enderecoViaCep.Complemento = valor[1]; 
                            }
                            if (cont == 4)
                            {
                                enderecoViaCep.Bairro = valor[1];
                            }
                            if (cont == 5)
                            {
                                enderecoViaCep.Cidade = valor[1];
                            }
                            if (cont == 6)
                            {
                                enderecoViaCep.Uf = valor[1];
                            }

                            cont++;
                        }
                    }
                }
            }
            return enderecoViaCep;
        }

        public List<Endereco> BuscarEnderecoPorNome(string nome)
        {
            return _enderecoService.GetEnderecoByName(nome);
        }

        public List<Endereco> BuscarEnderecoPorUsuario(string userId)
        {
            return _enderecoService.GetEnderecoByUser(userId);
        }

        public List<Endereco> BuscarEnderecos()
        {
            return _enderecoService.GetEndereco();
        }

        public void CadastroEndereco(Endereco endereco)
        {
            _enderecoService.CreateEndereco(endereco);
        }

        public void RemoverEndereco(string idEndereco)
        {
            if (idEndereco.Length == 24)
            {
                _enderecoService.RemoveEndereco(idEndereco);

                List<Carrinho> carrinhos = _carrinhoRepository.BuscarCarrinhos();
                foreach (Carrinho carrinho in carrinhos)
                {
                    if (carrinho.EnderecoId == idEndereco)
                    {
                        _carrinhoRepository.AddEndereco(carrinho.UserId, null);
                    }
                }
            }
            
        }
    }
}
