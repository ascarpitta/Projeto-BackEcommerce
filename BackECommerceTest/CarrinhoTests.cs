using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackECommerce.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using BackECommerce.Models;
using System.Linq;

namespace BackECommerceTest
{
    [TestClass]
    public class CarrinhoTest
    {
        private readonly CarrinhoRepository _carrinhoRepository;

        private readonly ProdutoRepository _produtoRepository;
        private readonly Produto _produtoTeste1;
        private readonly Produto _produtoTeste2;

        private readonly UsuarioRepository _usuarioRepository;
        private readonly Usuario _usuarioTeste;

        private readonly EnderecoRepository _enderecoRepository;
        private readonly Endereco _enderecoTeste;

        public CarrinhoTest()
        {
            _carrinhoRepository = new CarrinhoRepository();

            _produtoRepository = new ProdutoRepository();
            _produtoTeste1 = _produtoRepository.BuscarProduto("5eee43682677f2362887051c");
            _produtoTeste2 = _produtoRepository.BuscarProduto("5eee5b117373bcd310902b22");

            _usuarioRepository = new UsuarioRepository();
            _usuarioTeste = _usuarioRepository.BuscarUsuario("5eed8d19f7cf570004d6f68f");

            _enderecoRepository = new EnderecoRepository();
            _enderecoTeste = _enderecoRepository.BuscarEndereco("5ee8d82a14128a00042a1b0a");
            
        }

        [Test]
        public void CriaCarrinhoSucesso()
        {
            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            if (carrinhoResult == null)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }
        //Produto inativo
        [Test]
        public void CriaCarrinhoErro()
        {
            _produtoTeste1.Ativo = false;
            _produtoTeste1.Quantity = 1;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            if (carrinhoResult != null)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }
        }

        //Sem estoque
        [Test]
        public void CriaCarrinhoProdutoSemEstoque()
        {
            _produtoTeste1.Quantity = 0;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            if (carrinhoResult != null)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }
        }

        [Test]
        public void AddEnderecoSucesso()
        {
            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);
                        
            Carrinho carrinhoResult = _carrinhoRepository.AddEndereco(_usuarioTeste.Id, _enderecoTeste.Id);

            if (carrinhoResult.EnderecoId == null)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test]
        public void AddEnderecoErro()
        {
            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            var carrinhoResult = _carrinhoRepository.AddEndereco(_usuarioTeste.Id, _enderecoTeste.User);

            if (carrinhoResult != null)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);      

        }

        [Test]
        public void AumentarQuantProdutoSucesso()
        {
            _produtoTeste2.Quantity = 5;
            _produtoTeste2.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste2.User, _produtoTeste2.Id, _produtoTeste2);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste2.Id);

            Carrinho carrinhoResult2 = _carrinhoRepository.AlterarQuantProduto(_usuarioTeste.Id, _produtoTeste2.Id, 1);

            if (carrinhoResult2.Produtos[0].Quantidade != 2)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);

        }

        [Test]
        public void AumentarQuantProdutoErro()
        {
            _produtoTeste2.Quantity = 1;
            _produtoTeste2.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste2.User, _produtoTeste2.Id, _produtoTeste2);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste2.Id);

            Carrinho carrinhoResult2 = _carrinhoRepository.AlterarQuantProduto(_usuarioTeste.Id, _produtoTeste2.Id, 1);

            if (carrinhoResult2 != null)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);

        }

        [Test]
        public void AddProdutoSucesso()
        {
            _produtoTeste2.Quantity = 1;
            _produtoTeste2.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste2.User, _produtoTeste2.Id, _produtoTeste2);

            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste2.Id);
            if (carrinhoResult.Produtos.Count != 2)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test]
        public void AddProdutoRepedito()
        {
            _produtoTeste1.Quantity = 5;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);
            if (carrinhoResult.Produtos.Count == 2)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test]
        public void AddProdutoInativo()
        {
            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = false;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);
            if (carrinhoResult != null)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test]
        public void AddProdutoSemEstoque()
        {
            _produtoTeste1.Quantity = 0;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);
            if (carrinhoResult != null)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }
            else
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
            }            
        }

        [Test]
        public void AddProdutoProprioUser()
        {
            Produto prod = new Produto();
            prod.User = _usuarioTeste.Id;
            prod.Ativo = true;
            prod.Description = "Teste unitário";
            prod.Frete = 10;
            prod.Marca = "Teste unitário";
            prod.Name = "Teste unitário - mesmo user";
            prod.Price = 50;
            prod.Quantity = 2;

            _produtoRepository.CadastroProduto(prod);
            Produto produtoNovo = _produtoRepository.BuscarProdutoPorUsuario(prod.User, prod.Id);
            if (produtoNovo == null)
            {
                Assert.Fail();
            }

            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);            

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, produtoNovo.Id);
            if (carrinhoResult != null)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                _produtoRepository.RemoverProdutoPorId(produtoNovo.Id);
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
            _produtoRepository.RemoverProdutoPorId(produtoNovo.Id);
        }

        [Test]
        public void RemoveProdutoSucesso()
        {

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            Carrinho carrinhoResult = _carrinhoRepository.RemoverProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            if (carrinhoResult.Produtos.Count != 0)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }
            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test]
        public void RemoveProdutosSucesso()
        {
            _produtoTeste1.Quantity = 2;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            Carrinho carrinhoResult = _carrinhoRepository.RemoverProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            if (carrinhoResult.Produtos.Count > 0)
            {
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);

        }

        [Test]
        public void RemoveProdutoSemCarrinhoSucesso()
        {
            _produtoTeste1.Quantity = 2;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            Carrinho carrinhoResult = _carrinhoRepository.RemoverProduto(_usuarioTeste.Id, _produtoTeste2.Id);

            if (carrinhoResult != null)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);

        }

        [Test]
        public void DiminuirQuantProdutoSucesso()
        {
            _produtoTeste1.Quantity = 2;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            Carrinho carrinhoResult2 = _carrinhoRepository.AlterarQuantProduto(_usuarioTeste.Id, _produtoTeste1.Id, 2);

            if (carrinhoResult2.Produtos.Count != 0)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);

        }

        [Test]
        public void DiminuirQuantProdutoSemEstoqueSucesso()
        {
            _produtoTeste1.Quantity = 2;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            _produtoTeste1.Quantity = 0;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            Carrinho carrinhoResult2 = _carrinhoRepository.AlterarQuantProduto(_usuarioTeste.Id, _produtoTeste1.Id, 2);

            if (carrinhoResult2 != null)
            {
                Assert.Fail();
            }
            
            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);

        }
    }
}