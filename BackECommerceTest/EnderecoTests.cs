using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackECommerce.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using BackECommerce.Models;
using System.Linq;

namespace EnderecoTeste.Tests
{
    [TestClass]
    public class EnderecoTests
    {
        private readonly EnderecoRepository _enderecoRepository;
        private readonly Endereco _enderecoTeste;
        public EnderecoTests()
        {
            _enderecoRepository = new EnderecoRepository();
            _enderecoTeste = _enderecoRepository.BuscarEndereco("5e8fa9c6d776493a38eb4cfc");
        }
//Cadastrar endereço
        [Test]
        public void CadastroEnderecoTest()
        {
            Endereco novo = new Endereco();
            novo.Bairro = "Vila Paiva";
            novo.Cep = "02075040";
            novo.Cidade = "São Paulo";
            novo.Uf = "SP";
            novo.User = _enderecoTeste.User;
            novo.NomeEndereco = "Teste unitário";
            novo.Numero = 396;
            novo.Rua = "Manuel de Almeida";
            
            try
            {
                if (novo != null)
                {
                    _enderecoRepository.CadastroEndereco(novo);
                }
                var teste = _enderecoRepository.BuscarEnderecoPorNome(novo.NomeEndereco).FirstOrDefault();

                if (teste == null)
                {
                    Assert.Fail();
                }

                _enderecoRepository.RemoverEndereco(teste.Id);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

//Alterar endereço
        [Test] //alterar endereco cadastrado no usuário
        public void AtualizarEnderecoTest()
        {
            try
            {
                var alterar = _enderecoRepository.BuscarEndereco(_enderecoTeste.Id);
                alterar.Rua = "Nova Rua";
                var alterado = _enderecoRepository.AtualizarEndereco(_enderecoTeste.User, _enderecoTeste.Id, alterar);

                if (alterado == null)
                {
                    Assert.Fail();
                }

                if (alterar.Rua != alterado.Rua)
                {
                    Assert.Fail();
                }
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //alterar endereco inexistente no usuário
        public void AtualizarEnderecoTest_EnderecoInexistente()
        {
            try
            {
                var alterar = _enderecoRepository.BuscarEndereco(_enderecoTeste.Id);
                alterar.Rua = "Nova Rua";
                var alterado = _enderecoRepository.AtualizarEndereco(_enderecoTeste.User, _enderecoTeste.User, alterar);

                if (alterado != null)
                {
                    Assert.Fail();
                }
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //alterar endereco de usuário diferente
        public void AtualizarEnderecoTest_UserDiferente()
        {
            try
            {
                var alterar = _enderecoRepository.BuscarEndereco(_enderecoTeste.Id);
                alterar.Rua = "Nova Rua";
                var alterado = _enderecoRepository.AtualizarEndereco(_enderecoTeste.Id, _enderecoTeste.Id, alterar);

                if (alterado != null)
                {
                    Assert.Fail();
                }
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

//Buscar Endereco
        [Test] //buscar endereco existente
        public void BuscarEnderecoTest()
        {
            try
            {
                if (_enderecoRepository.BuscarEndereco(_enderecoTeste.Id) == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //buscar endereco inexistente
        public void BuscarEnderecoTest_EnderecoInexistente()
        {
            try
            {
                if (_enderecoRepository.BuscarEndereco(_enderecoTeste.User) != null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //buscar cep na api do correio
        public void BuscarEnderecoPorCepTest()
        {
            try
            {
                var teste = _enderecoRepository.BuscarEnderecoPorCep("02075040");
                if (teste == null)
                {
                    Assert.Fail();
                }
                var rua = " Rua Manuel de Almeida";
                if (teste.Rua != rua)
                {
                    Assert.Fail();
                }
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //buscar endereco existente
        public void BuscarEnderecoPorNomeTest()
        {
            try
            {
                if (_enderecoRepository.BuscarEnderecoPorNome(_enderecoTeste.NomeEndereco) == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //buscar endereco inexistente
        public void BuscarEnderecoPorNomeTest_EnderecoInexistente()
        {
            try
            {
                var teste = _enderecoRepository.BuscarEnderecoPorNome(_enderecoTeste.Id);
                if (teste.Count > 0)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //buscar enderecos do usuário
        public void BuscarEnderecoPorUsuarioTest()
        {
            try
            {
                if (_enderecoRepository.BuscarEnderecoPorUsuario(_enderecoTeste.User) == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //buscar enderecos do usuário inexistente
        public void BuscarEnderecoPorUsuarioTest_UserInexistente()
        {
            try
            {
                var teste = _enderecoRepository.BuscarEnderecoPorUsuario(_enderecoTeste.Id);
                if (teste.Count > 0)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //buscar todos os endereços
        public void BuscarEnderecosTest()
        {
            try
            {
                if (_enderecoRepository.BuscarEnderecos() == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

    }
}