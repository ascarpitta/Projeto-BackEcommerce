using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackECommerce.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using BackECommerce.Models;

namespace ProdutoTeste.Tests
{
    [TestClass]
    public class ProdutoTests
    {
        private readonly ProdutoRepository _produtoRepository;
        private readonly Produto _produtoTeste;
        public ProdutoTests()
        {
            _produtoRepository = new ProdutoRepository();
            _produtoTeste = _produtoRepository.BuscarProduto("5eee43682677f2362887051c");
        }
//Cadastrar Produto
        [Test] //produto com todas as informações
        public void CadastroProdutoTest()
        {
            Produto prod = new Produto();
            prod.User = _produtoTeste.User;
            prod.Ativo = true;
            prod.Description = "Teste unitário";
            prod.Frete = 10;
            prod.Marca = "Teste unitário";
            prod.Name = "Teste unitário";
            prod.Price = 50;
            prod.Quantity = 2;

            _produtoRepository.CadastroProduto(prod);
            Produto proutoNovo = _produtoRepository.BuscarProdutoPorUsuario(prod.User, prod.Id);
            if (proutoNovo == null)
            {
                Assert.Fail();
            }
            else
            {
                _produtoRepository.RemoverProdutoPorId(proutoNovo.Id);
            }                
           
        }

        [Test] //produto sem quantidade
        public void CadastroProdutoTest_SemQtde()
        {
            Produto prod = new Produto();
            prod.User = _produtoTeste.User;
            prod.Ativo = true;
            prod.Description = "Teste unitário";
            prod.Frete = 10;
            prod.Marca = "Teste unitário";
            prod.Name = "Teste unitário";
            prod.Price = 50;

            try
            {
                _produtoRepository.CadastroProduto(prod);
                Produto proutoNovo = _produtoRepository.BuscarProdutoPorUsuario(prod.User, prod.Id);
                if (proutoNovo == null)
                {
                    Assert.Fail();
                }
                else
                {
                    _produtoRepository.RemoverProdutoPorId(proutoNovo.Id);
                }
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto sem preco
        public void CadastroProdutoTest_SemPreco()
        {
            Produto prod = new Produto();
            prod.User = _produtoTeste.User;
            prod.Ativo = true;
            prod.Description = "Teste unitário";
            prod.Frete = 10;
            prod.Marca = "Teste unitário";
            prod.Name = "Teste unitário";
            prod.Quantity = 2;

            try
            {
                _produtoRepository.CadastroProduto(prod);
                Produto proutoNovo = _produtoRepository.BuscarProdutoPorUsuario(prod.User, prod.Id);
                if (proutoNovo == null)
                {
                    Assert.Fail();
                }
                else
                {
                    _produtoRepository.RemoverProdutoPorId(proutoNovo.Id);
                }
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto sem frete
        public void CadastroProdutoTest_SemFrete()
        {
            Produto prod = new Produto();
            prod.User = _produtoTeste.User;
            prod.Ativo = true;
            prod.Description = "Teste unitário";
            prod.Frete = 10;
            prod.Marca = "Teste unitário";
            prod.Name = "Teste unitário";
            prod.Price = 50;
            prod.Quantity = 2;

            try
            {
                _produtoRepository.CadastroProduto(prod);
                Produto proutoNovo = _produtoRepository.BuscarProdutoPorUsuario(prod.User, prod.Id);
                if (proutoNovo == null)
                {
                    Assert.Fail();
                }
                else
                {
                    _produtoRepository.RemoverProdutoPorId(proutoNovo.Id);
                }
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

//Alteração de Produto
        [Test] //alterarações diversas
        public void AtualizarProdutoTest()
        {
            try
            {
                var prod = _produtoRepository.BuscarProduto(_produtoTeste.Id);
                prod.Name = "Produto atualizado";

                var att = _produtoRepository.AtualizarProduto(prod.User, prod.Id, prod);

                if (att == null)
                {
                    Assert.Fail();
                }
                else
                {
                    if (prod.Name != att.Name)
                    {
                        Assert.Fail();
                    }
                }                
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //alterar estoque
        public void AtualizarProdutoTest_AlterarEstoque()
        {
            try
            {
                var prod = _produtoRepository.BuscarProduto(_produtoTeste.Id);
                prod.Quantity += 1;

                var att = _produtoRepository.AtualizarProduto(prod.User, prod.Id, prod);

                if (att == null)
                {
                    Assert.Fail();
                }
                else
                {
                    if (prod.Quantity != att.Quantity)
                    {
                        Assert.Fail();
                    }
                }                
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //ativar produto
        public void AtivaProdutoTest()
        {
            _produtoTeste.Ativo = false;
            _produtoTeste.Quantity = 1;
            _produtoRepository.AtualizarProduto(_produtoTeste.User, _produtoTeste.Id, _produtoTeste);
            var teste = _produtoRepository.AtivaProduto(_produtoTeste.User, _produtoTeste.Id);
            if (teste == null || !teste.Ativo)
            {
                Assert.Fail();
            }
        }

        [Test] //inativar produto
        public void InativaProdutoTest()
        {
            _produtoTeste.Ativo = true;
            _produtoTeste.Quantity = 1;
            _produtoRepository.AtualizarProduto(_produtoTeste.User, _produtoTeste.Id, _produtoTeste);

            if (_produtoTeste.Ativo)
            {
                var teste = _produtoRepository.InativaProduto(_produtoTeste.User, _produtoTeste.Id);
                if (teste == null || teste.Ativo)
                {
                    Assert.Fail();
                }
            }        
        }

        [Test] //ativar produto de outro usuário
        public void AtivaProdutoTest_Outro()
        {
            _produtoTeste.Ativo = true;
            _produtoTeste.Quantity = 1;
            _produtoRepository.AtualizarProduto(_produtoTeste.User, _produtoTeste.Id, _produtoTeste);

            try
            {
                if (_produtoTeste.Ativo)
                {
                    var teste = _produtoRepository.AtivaProduto(_produtoTeste.Id, _produtoTeste.Id);
                    if (teste != null)
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    var prod = _produtoRepository.BuscarProduto(_produtoTeste.Id);
                    var teste = _produtoRepository.AtivaProduto(_produtoTeste.Id, _produtoTeste.Id);
                    if (teste != null || prod.Ativo == _produtoTeste.Ativo)
                    {
                        Assert.Fail();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //inativar produto de outro usuário
        public void InativaProdutoTest_Outro()
        {
            _produtoTeste.Ativo = true;
            _produtoTeste.Quantity = 1;
            _produtoRepository.AtualizarProduto(_produtoTeste.User, _produtoTeste.Id, _produtoTeste);

            if (_produtoTeste.Ativo)
            {
                var teste = _produtoRepository.InativaProduto(_produtoTeste.Id, _produtoTeste.Id);
                if (teste != null)
                {
                    if (teste.Ativo != _produtoTeste.Ativo)
                    {
                        Assert.Fail();
                    }
                }
            }
            else
            {
                var teste = _produtoRepository.InativaProduto(_produtoTeste.Id, _produtoTeste.Id);
                if (teste != null)
                {
                    Assert.Fail();
                }
            }            
        }

//Buscar Produto
        [Test]
        public void BuscarProdutosTest()
        {
            try
            {
                if (_produtoRepository.BuscarProdutos() == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto existente
        public void BuscarProdutoTest()
        {
            try
            {
                if (_produtoRepository.BuscarProduto(_produtoTeste.Id) == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            
        }

        [Test] //produto inexistente
        public void BuscarProdutoTest_ProdInexistente()
        {
            try
            {
                if (_produtoRepository.BuscarProduto(_produtoTeste.User) != null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        [Test] 
        public void BuscarProdutosPorUsuarioTest()
        {
            try
            {
                if (_produtoRepository.BuscarProdutosPorUsuario(_produtoTeste.Id) == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        
        [Test] //buscar por usuário inexistente
        public void BuscarProdutosPorUsuarioTest_UserInexistente()
        {
            try
            {
                if (_produtoRepository.BuscarProdutosPorUsuario(_produtoTeste.Id) == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //buscar por usuário existente
        public void BuscarProdutoPorUsuarioTest()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutoPorUsuario(_produtoTeste.User, _produtoTeste.Id);     
                
                if (prod == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //buscar por usuário inexistente com produto cadastrado
        public void BuscarProdutoPorUsuarioTest_UserInexistente_ProdutoExistente()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutoPorUsuario(_produtoTeste.Id, _produtoTeste.Id);

                if (prod != null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //buscar por usuário existente com produto inexistente
        public void BuscarProdutoPorUsuarioTest_UserExistente_ProdutoInexistente()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutoPorUsuario(_produtoTeste.User, _produtoTeste.User);

                if (prod != null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //buscar por usuário inexistente e produto inexistente
        public void BuscarProdutoPorUsuarioTest_UserInexistente_ProdutoInexistente()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutoPorUsuario(_produtoTeste.Id, _produtoTeste.User);

                if (prod != null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto existente
        public void BuscarProdutosPorNomeTest()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutosPorNome(_produtoTeste.Name);

                if (prod == null)
                {
                    Assert.Fail();
                }
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto inexistente
        public void BuscarProdutosPorNomeTest_ProdutoInexistente()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutosPorNome(_produtoTeste.Id);

                if (prod.Count > 0)
                {
                    Assert.Fail();
                }
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto existente
        public void BuscarProdutoPorNomeOrdemMaiorPrecoTest()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutosPorNome(_produtoTeste.Name);

                if (prod == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto inexistente
        public void BuscarProdutoPorNomeOrdemMaiorPrecoTest_ProdutoInexistente()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutosPorNome(_produtoTeste.Id);

                if (prod.Count > 0)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto existente
        public void BuscarProdutoPorNomeOrdemMenorPrecoTest()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutosPorNome(_produtoTeste.Name);

                if (prod == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto inexistente
        public void BuscarProdutoPorNomeOrdemMenorPrecoTest_ProdutoInexistente()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutosPorNome(_produtoTeste.Id);

                if (prod.Count > 0)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto existente
        public void BuscarProdutoPorNomeOrdemAbcTest()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutosPorNome(_produtoTeste.Name);

                if (prod == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto inexistente
        public void BuscarProdutoPorNomeOrdemAbcTest_ProdutoInexistente()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutosPorNome(_produtoTeste.Id);

                if (prod.Count > 0)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto existente
        public void BuscarProdutoPorNomeOrdemZyxTest()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutosPorNome(_produtoTeste.Name);

                if (prod == null)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //produto inexistente
        public void BuscarProdutoPorNomeOrdemZyxTest_ProdutoInexistente()
        {
            try
            {
                var prod = _produtoRepository.BuscarProdutosPorNome(_produtoTeste.Id);

                if (prod.Count > 0)
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