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
    public class CarrinhoTest
    {
        private readonly CarrinhoRepository _carrinhoRepository;

        private readonly ProdutoRepository _produtoRepository;
        private readonly Produto _produtoTeste;

        private readonly UsuarioRepository _usuarioRepository;
        private readonly Usuario _usuarioTeste;

        private readonly EnderecoRepository _enderecoRepository;

        public CarrinhoTest()
        {
            _carrinhoRepository = new CarrinhoRepository();

            _produtoRepository = new ProdutoRepository();
            _produtoTeste = _produtoRepository.BuscarProduto("5ebf258583ca2c72ec89c760");

            _usuarioRepository = new UsuarioRepository();
            _usuarioTeste = _usuarioRepository.BuscarUsuarioPorEmail("teste@hotmail.com");

            _enderecoRepository = new EnderecoRepository();
        }

        [Test]
        public void CriaCarrinhoSucesso()
        {

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);

            if (carrinhoResult == null)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }
        //Produto inativo
        [Test]
        public void CriaCarrinhoErro()
        {
            Produto prod = _produtoRepository.BuscarProduto("5eda993a1049d145c4a686be");

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, prod.Id);

            if (carrinhoResult != null)
            {
                Assert.Fail();
            }
        }

        //Sem estoque
        [Test]
        public void CriaCarrinhoProdutoSemEstoque()
        {
            Produto prod = _produtoRepository.BuscarProduto("5ebd72407f288a429408e5f9");

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, prod.Id);

            if (carrinhoResult != null)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void AddEnderecoSucesso()
        {
            //Codacy acusou como não utilizado: Carrinho carrinho = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);

            Endereco novo = new Endereco();
            novo.Bairro = "Vila Paiva";
            novo.Cep = "02075040";
            novo.Cidade = "São Paulo";
            novo.Uf = "SP";
            novo.User = _usuarioTeste.Id;
            novo.NomeEndereco = "Teste unitário";
            novo.Numero = 396;
            novo.Rua = "Manuel de Almeida";

            _enderecoRepository.CadastroEndereco(novo);

            //Codacy acusou como não utilizado: Endereco enderecoTest = _enderecoRepository.BuscarEndereco("5e8fa9c6d776493a38eb4cfc");

            var teste = _enderecoRepository.BuscarEnderecoPorNome(novo.NomeEndereco).FirstOrDefault();

            Carrinho carrinhoResult = _carrinhoRepository.AddEndereco(_usuarioTeste.Id, teste.Id);

            if (carrinhoResult.EnderecoId == null)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test]
        public void AddEnderecoErro()
        {
            
            Carrinho carrinho = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);

            Endereco enderecoTest = _enderecoRepository.BuscarEndereco(carrinho.UserId);

            if (enderecoTest != null)
            {
                Carrinho carrinhoResult = _carrinhoRepository.AddEndereco(_usuarioTeste.Id, enderecoTest.Id);

                if (carrinhoResult.EnderecoId != null)
                {
                    Assert.Fail();
                }                
            }
            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test] //Teste 
        public void AumentarQuantProdutoSucesso()
        {
            Produto prod = _produtoRepository.BuscarProduto("5ebd99f50ec51c52381a046d");

            //Codacy acusou como não utilizado: Carrinho carrinhoResult1 = _carrinhoRepository.AddProduto(_usuarioTeste.Id, prod.Id);

            Carrinho carrinhoResult2 = _carrinhoRepository.AlterarQuantProduto(_usuarioTeste.Id, prod.Id, 1);

            if (carrinhoResult2.Produtos[0].Quantidade != 2)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);

        }

        [Test]
        public void AumentarQuantProdutoErro()
        {
            Produto prod = _produtoRepository.BuscarProduto("5ebd99f50ec51c52381a046d");

            Carrinho carrinhoResult1 = _carrinhoRepository.AddProduto(_usuarioTeste.Id, prod.Id);

            Carrinho carrinhoResult2 = _carrinhoRepository.AlterarQuantProduto(_usuarioTeste.Id, prod.Id, 1);

            if (carrinhoResult2.Produtos[0].Quantidade <= 1)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);

        }

        [Test]
        public void AddProdutoSucesso()
        {

            Produto prod = _produtoRepository.BuscarProduto("5ebd99f50ec51c52381a046d");

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, prod.Id);
            if (carrinhoResult.Produtos.Count != 2)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test]
        public void AddProdutoRepedito()
        {

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);
            if (carrinhoResult.Produtos.Count == 2)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test]
        public void AddProdutoInativo()
        {

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);

            Produto prod = _produtoRepository.BuscarProduto("5eda993a1049d145c4a686be");

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, prod.Id);
            if (carrinhoResult != null)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test]
        public void AddProdutoSemEstoque()
        {

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);

            Produto prod = _produtoRepository.BuscarProduto("5ebd72407f288a429408e5f9");

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, prod.Id);
            if (carrinhoResult != null)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test]
        public void AddProdutoProprioUser()
        {

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);

            Produto prod = _produtoRepository.BuscarProduto("5ebf258583ca2c72ec89c760");

            Carrinho carrinhoResult = _carrinhoRepository.AddProduto(_usuarioTeste.Id, prod.Id);
            if (carrinhoResult.Produtos.Count != 1)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
        }

        [Test]
        public void RemoveProdutoSucesso()
        {

            //Codacy acusou como não utilizado: Carrinho carrinhotest = _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);

            Carrinho carrinhoResult = _carrinhoRepository.RemoverProduto(_usuarioTeste.Id, _produtoTeste.Id);

            if (carrinhoResult.Produtos.Count != 0)
            {
                Assert.Fail();
            }

        }

        [Test]
        public void RemoveProdutosSucesso()
        {
            Produto prod = _produtoRepository.BuscarProduto("5ebd99f50ec51c52381a046d");

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);

            //Codacy acusou como não utilizado: Carrinho carrinhotest = _carrinhoRepository.AddProduto(_usuarioTeste.Id, prod.Id);

            Carrinho carrinhoResult = _carrinhoRepository.RemoverProduto(_usuarioTeste.Id, _produtoTeste.Id);

            if (carrinhoResult.Produtos.Count != 1)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverProduto(_usuarioTeste.Id, prod.Id);

        }

        [Test]
        public void RemoveProdutoSemCarrinhoSucesso()
        {
            Produto prod = _produtoRepository.BuscarProduto("5ebd99f50ec51c52381a046d");

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste.Id);

            Carrinho carrinhoResult = _carrinhoRepository.RemoverProduto(_usuarioTeste.Id, prod.Id);

            if (carrinhoResult.Produtos.Count != 1)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverProduto(_usuarioTeste.Id, prod.Id);

        }

        [Test]
        public void DiminuirQuantProdutoSucesso()
        {
            Produto prod = _produtoRepository.BuscarProduto("5ebd99f50ec51c52381a046d");

            Carrinho carrinhoResult1 = _carrinhoRepository.AddProduto(_usuarioTeste.Id, prod.Id);

            Carrinho carrinhoResult2 = _carrinhoRepository.AlterarQuantProduto(_usuarioTeste.Id, prod.Id, 2);

            if (carrinhoResult2.Produtos.Count != 0)
            {
                Assert.Fail();
            }

            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);

        }

        [Test]
        public void DiminuirQuantProdutoSemEstoqueSucesso()
        {
            Produto prod = _produtoRepository.BuscarProduto("5ebd72407f288a429408e5f9");

            Carrinho carrinhoResult1 = _carrinhoRepository.AddProduto(_usuarioTeste.Id, prod.Id);

            Carrinho carrinhoResult2 = _carrinhoRepository.AlterarQuantProduto(_usuarioTeste.Id, prod.Id, 2);

            if (carrinhoResult2 != null)
            {
                Assert.Fail();
            }
            
            _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);

        }
    }
}