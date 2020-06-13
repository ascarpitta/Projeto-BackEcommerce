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
            _produtoTeste = _produtoRepository.BuscarProduto("5ebcb414e06d5752dc873d54");
        }
//Cadastrar Produto
        [Test] //produto com todas as informações
        public void CadastroProdutoTest()
        {
            bool ok = false;
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
                List<Produto> lista = _produtoRepository.BuscarProdutosPorUsuario(prod.User);
                if (lista == null)
                {
                    Assert.Fail();
                }
                else
                {
                    foreach (Produto item in lista)
                    {
                        Produto encontrado = _produtoRepository.BuscarProdutoPorUsuario(prod.User, item.Id);

                        if (encontrado != null)
                        {
                            ok = true;
                        }
                    }

                    if (!ok)
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

        [Test] //produto sem quantidade
        public void CadastroProdutoTest_SemQtde()
        {
            bool ok = false;
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
                List<Produto> lista = _produtoRepository.BuscarProdutosPorUsuario(prod.User);
                if (lista == null)
                {
                    Assert.Fail();
                }
                else
                {
                    foreach (Produto item in lista)
                    {
                        Produto encontrado = _produtoRepository.BuscarProdutoPorUsuario(prod.User, item.Id);

                        if (encontrado != null)
                        {
                            ok = true;
                        }
                    }

                    if (!ok)
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

        [Test] //produto sem preco
        public void CadastroProdutoTest_SemPreco()
        {
            bool ok = false;
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
                List<Produto> lista = _produtoRepository.BuscarProdutosPorUsuario(prod.User);
                if (lista == null)
                {
                    Assert.Fail();
                }
                else
                {
                    foreach (Produto item in lista)
                    {
                        Produto encontrado = _produtoRepository.BuscarProdutoPorUsuario(prod.User, item.Id);

                        if (encontrado != null)
                        {
                            ok = true;
                        }
                    }

                    if (!ok)
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

        [Test] //produto sem frete
        public void CadastroProdutoTest_SemFrete()
        {
            bool ok = false;
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
                List<Produto> lista = _produtoRepository.BuscarProdutosPorUsuario(prod.User);
                if (lista == null)
                {
                    Assert.Fail();
                } 
                else
                {
                    foreach (Produto item in lista)
                    {
                        Produto encontrado = _produtoRepository.BuscarProdutoPorUsuario(prod.User, item.Id);

                        if (encontrado != null)
                        {
                            ok = true;
                        }
                    }

                    if (!ok)
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
            try
            {
                var teste = _produtoRepository.AtivaProduto(_produtoTeste.User, _produtoTeste.Id);
                if (teste == null || !teste.Ativo)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //inativar produto
        public void InativaProdutoTest()
        {
            try
            {
                var teste = _produtoRepository.InativaProduto(_produtoTeste.User, _produtoTeste.Id);
                if (teste == null || teste.Ativo)
                {
                    Assert.Fail();
                }

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test] //ativar produto de outro usuário
        public void AtivaProdutoTest_Outro()
        {
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
            try
            {
                if (_produtoTeste.Ativo)
                {
                    var prod = _produtoRepository.BuscarProduto(_produtoTeste.Id);
                    var teste = _produtoRepository.InativaProduto(_produtoTeste.Id, _produtoTeste.Id);
                    if (teste != null || prod.Ativo == _produtoTeste.Ativo)
                    {
                        Assert.Fail();
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
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
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