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
    class PedidoTests
    {
        private readonly CarrinhoRepository _carrinhoRepository;

        private readonly ProdutoRepository _produtoRepository;
        private readonly Produto _produtoTeste1;
        private readonly Produto _produtoTeste2;

        private readonly UsuarioRepository _usuarioRepository;
        private readonly Usuario _usuarioTeste;

        private readonly EnderecoRepository _enderecoRepository;
        private readonly Endereco _enderecoTeste;

        private readonly PedidoRepository _pedidoRepository;

        public PedidoTests()
        {
            _carrinhoRepository = new CarrinhoRepository();

            _produtoRepository = new ProdutoRepository();
            _produtoTeste1 = _produtoRepository.BuscarProduto("5eee43682677f2362887051c");
            _produtoTeste2 = _produtoRepository.BuscarProduto("5eee5b117373bcd310902b22");

            _usuarioRepository = new UsuarioRepository();
            _usuarioTeste = _usuarioRepository.BuscarUsuario("5eed8d19f7cf570004d6f68f");

            _enderecoRepository = new EnderecoRepository();
            _enderecoTeste = _enderecoRepository.BuscarEndereco("5ee8d82a14128a00042a1b0a");

            _pedidoRepository = new PedidoRepository();
        }

        [Test]
        public void BuscaPedidosSucesso()
        {
            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            _carrinhoRepository.FinalizarCarrinho(_usuarioTeste.Id);

            List<Pedido> listPedido = _pedidoRepository.BuscarPedidos();
            if (listPedido == null)
            {
                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
                Assert.Fail();
            }
            else
            {
                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _carrinhoRepository.RemoverCarrinhoPorUsuario(_usuarioTeste.Id);
            }

        }

        [Test]
        public void BuscaPedidoSucesso()
        {
            Endereco novo = new Endereco();
            novo.Bairro = "Vila Paiva";
            novo.Cep = "02075040";
            novo.Cidade = "São Paulo";
            novo.Uf = "SP";
            novo.User = _usuarioTeste.Id;
            novo.NomeEndereco = "AlteraEnderecoSucesso";
            novo.Numero = 396;
            novo.Rua = "Manuel de Almeida";

            if (novo != null)
            {
                _enderecoRepository.CadastroEndereco(novo);
            }

            var enderecoResult = _enderecoRepository.BuscarEnderecoPorNome(novo.NomeEndereco).FirstOrDefault();

            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            _carrinhoRepository.AddEndereco(_usuarioTeste.Id, enderecoResult.Id);

            Pedido pedidoResult = _carrinhoRepository.FinalizarCarrinho(_usuarioTeste.Id);

            Pedido pedidoBuscado = _pedidoRepository.BuscarPedido(pedidoResult.Id);

            if (pedidoBuscado == null)
            {

                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
                Assert.Fail();
            }
            else
            {

                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
            }

        }

        [Test]
        public void BuscaPedidoNaoExistente()
        {
            Endereco novo = new Endereco();
            novo.Bairro = "Vila Paiva";
            novo.Cep = "02075040";
            novo.Cidade = "São Paulo";
            novo.Uf = "SP";
            novo.User = _usuarioTeste.Id;
            novo.NomeEndereco = "AlteraEnderecoSucesso";
            novo.Numero = 396;
            novo.Rua = "Manuel de Almeida";

            if (novo != null)
            {
                _enderecoRepository.CadastroEndereco(novo);
            }

            var enderecoResult = _enderecoRepository.BuscarEnderecoPorNome(novo.NomeEndereco).FirstOrDefault();

            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            _carrinhoRepository.AddEndereco(_usuarioTeste.Id, enderecoResult.Id);

            Pedido pedidoResult = _carrinhoRepository.FinalizarCarrinho(_usuarioTeste.Id);
            _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
            Pedido pedidoBuscado = _pedidoRepository.BuscarPedido(pedidoResult.Id);

            if (pedidoBuscado != null)
            {

                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
                Assert.Fail();
            }
            else
            {

                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
            }
        }

        [Test]
        public void BuscarPedidosPorUsuario()
        {
            Endereco novo = new Endereco();
            novo.Bairro = "Vila Paiva";
            novo.Cep = "02075040";
            novo.Cidade = "São Paulo";
            novo.Uf = "SP";
            novo.User = _usuarioTeste.Id;
            novo.NomeEndereco = "AlteraEnderecoSucesso";
            novo.Numero = 396;
            novo.Rua = "Manuel de Almeida";

            if (novo != null)
            {
                _enderecoRepository.CadastroEndereco(novo);
            }

            var enderecoResult = _enderecoRepository.BuscarEnderecoPorNome(novo.NomeEndereco).FirstOrDefault();

            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            _carrinhoRepository.AddEndereco(_usuarioTeste.Id, enderecoResult.Id);

            Pedido pedidoResult = _carrinhoRepository.FinalizarCarrinho(_usuarioTeste.Id);

            List<Pedido> listpedidoBuscado = _pedidoRepository.BuscarPedidosPorUsuario(_usuarioTeste.Id);

            if (listpedidoBuscado == null)
            {
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
                Assert.Fail();
            }
            else
            {

                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
            }
        }

        [Test]
        public void BuscaPedidoPorUserNaoExistente()
        {
            Endereco novo = new Endereco();
            novo.Bairro = "Vila Paiva";
            novo.Cep = "02075040";
            novo.Cidade = "São Paulo";
            novo.Uf = "SP";
            novo.User = _usuarioTeste.Id;
            novo.NomeEndereco = "AlteraEnderecoSucesso";
            novo.Numero = 396;
            novo.Rua = "Manuel de Almeida";

            if (novo != null)
            {
                _enderecoRepository.CadastroEndereco(novo);
            }

            var enderecoResult = _enderecoRepository.BuscarEnderecoPorNome(novo.NomeEndereco).FirstOrDefault();

            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            _carrinhoRepository.AddEndereco(_usuarioTeste.Id, enderecoResult.Id);

            _carrinhoRepository.FinalizarCarrinho(_usuarioTeste.Id);
            _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);

            List<Pedido> listpedidoBuscado = _pedidoRepository.BuscarPedidosPorUsuario(_usuarioTeste.Id);

            if (listpedidoBuscado.Count > 0)
            {

                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
                Assert.Fail();
            }
            else
            {

                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
            }

        }

        [Test]
        public void BuscaPedidoPorUsuario()
        {
            Endereco novo = new Endereco();
            novo.Bairro = "Vila Paiva";
            novo.Cep = "02075040";
            novo.Cidade = "São Paulo";
            novo.Uf = "SP";
            novo.User = _usuarioTeste.Id;
            novo.NomeEndereco = "AlteraEnderecoSucesso";
            novo.Numero = 396;
            novo.Rua = "Manuel de Almeida";

            if (novo != null)
            {
                _enderecoRepository.CadastroEndereco(novo);
            }

            var enderecoResult = _enderecoRepository.BuscarEnderecoPorNome(novo.NomeEndereco).FirstOrDefault();

            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            _carrinhoRepository.AddEndereco(_usuarioTeste.Id, enderecoResult.Id);

            Pedido pedidoResult = _carrinhoRepository.FinalizarCarrinho(_usuarioTeste.Id);

            Pedido pedidoBuscado = _pedidoRepository.BuscarPedidoPorUsuario(_usuarioTeste.Id, pedidoResult.Id);

            if (pedidoBuscado == null)
            {
                
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
                Assert.Fail();
            }
            else
            {
                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
            }
        }

        [Test]
        public void BuscaPedidoPorUsuarioInexistente()
        {
            Endereco novo = new Endereco();
            novo.Bairro = "Vila Paiva";
            novo.Cep = "02075040";
            novo.Cidade = "São Paulo";
            novo.Uf = "SP";
            novo.User = _usuarioTeste.Id;
            novo.NomeEndereco = "AlteraEnderecoSucesso";
            novo.Numero = 396;
            novo.Rua = "Manuel de Almeida";

            if (novo != null)
            {
                _enderecoRepository.CadastroEndereco(novo);
            }

            var enderecoResult = _enderecoRepository.BuscarEnderecoPorNome(novo.NomeEndereco).FirstOrDefault();

            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            _carrinhoRepository.AddEndereco(_usuarioTeste.Id, enderecoResult.Id);

            Pedido pedidoResult = _carrinhoRepository.FinalizarCarrinho(_usuarioTeste.Id);
            _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
            Pedido pedidoBuscado = _pedidoRepository.BuscarPedidoPorUsuario(_usuarioTeste.Id, pedidoResult.Id);

            if (pedidoBuscado != null)
            {
                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
                Assert.Fail();
            }
            else
            {
               _enderecoRepository.RemoverEndereco(enderecoResult.Id);
            }
        }

        [Test]
        public void BuscaPedidoCompraEmAndamento()
        {
            Endereco novo = new Endereco();
            novo.Bairro = "Vila Paiva";
            novo.Cep = "02075040";
            novo.Cidade = "São Paulo";
            novo.Uf = "SP";
            novo.User = _usuarioTeste.Id;
            novo.NomeEndereco = "AlteraEnderecoSucesso";
            novo.Numero = 396;
            novo.Rua = "Manuel de Almeida";

            if (novo != null)
            {
                _enderecoRepository.CadastroEndereco(novo);
            }

            var enderecoResult = _enderecoRepository.BuscarEnderecoPorNome(novo.NomeEndereco).FirstOrDefault();

            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            _carrinhoRepository.AddEndereco(_usuarioTeste.Id, enderecoResult.Id);

            _carrinhoRepository.FinalizarCarrinho(_usuarioTeste.Id);

            List<Pedido> listpedidoBuscado = _pedidoRepository.PedidosCompraEmAndamento(_usuarioTeste.Id);

            if (listpedidoBuscado.Count == 0)
            {
                
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
                Assert.Fail();
            }
            else
            {
                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
            }
        }

        //Usuario Inexistente
        [Test]
        public void BuscaPedidoCompraEmAndamentoInexistente()
        {

            try
            {
                List<Pedido> listpedidoBuscado = _pedidoRepository.PedidosCompraEmAndamento("5eed8d19f7cf570004d6f68");

                if (listpedidoBuscado != null)
                {
                    Assert.Fail();
                }

            }
            catch(System.Exception e)
            {
                Assert.Fail();
            }
        }


        [Test]
        public void PagarPedido()
        {
            Endereco novo = new Endereco();
            novo.Bairro = "Vila Paiva";
            novo.Cep = "02075040";
            novo.Cidade = "São Paulo";
            novo.Uf = "SP";
            novo.User = _usuarioTeste.Id;
            novo.NomeEndereco = "AlteraEnderecoSucesso";
            novo.Numero = 396;
            novo.Rua = "Manuel de Almeida";

            if (novo != null)
            {
                _enderecoRepository.CadastroEndereco(novo);
            }

            var enderecoResult = _enderecoRepository.BuscarEnderecoPorNome(novo.NomeEndereco).FirstOrDefault();

            _produtoTeste1.Quantity = 1;
            _produtoTeste1.Ativo = true;
            _produtoRepository.AtualizarProduto(_produtoTeste1.User, _produtoTeste1.Id, _produtoTeste1);

            _carrinhoRepository.AddProduto(_usuarioTeste.Id, _produtoTeste1.Id);

            _carrinhoRepository.AddEndereco(_usuarioTeste.Id, enderecoResult.Id);

            Pedido pedidoResult = _carrinhoRepository.FinalizarCarrinho(_usuarioTeste.Id);

            Pedido pedidopago = _pedidoRepository.PagarPedido(_usuarioTeste.Id, pedidoResult.Id);
            if(pedidopago == null)
            {
                _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                _enderecoRepository.RemoverEndereco(enderecoResult.Id);
                Assert.Fail();
            }
            else
            {
                if (pedidopago.DataPagamentoConfirmado == null)
                {
                    _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                    _enderecoRepository.RemoverEndereco(enderecoResult.Id);
                    Assert.Fail();
                }
                else
                {
                    _pedidoRepository.DeletarPedidoPorUsuario(_usuarioTeste.Id);
                    _enderecoRepository.RemoverEndereco(enderecoResult.Id);
                }
            }

        }
    }
}
