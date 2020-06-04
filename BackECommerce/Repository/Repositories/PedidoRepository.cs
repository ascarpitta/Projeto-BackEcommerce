using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using BackECommerce.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Repository.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly IPedidoService _pedidoService;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private EmailRepository _emailRepository;
        public PedidoRepository(IPedidoService pedidoService, IProdutoRepository produtoRepository, IUsuarioRepository usuarioRepository, IEnderecoRepository enderecoRepository)
        {
            _pedidoService = pedidoService;
            _produtoRepository = produtoRepository;
            _usuarioRepository = usuarioRepository;
            _enderecoRepository = enderecoRepository;
            _emailRepository = new EmailRepository();
        }

        public void AtualizarPedido(Pedido pedidoNovo, string id)
        {
            _pedidoService.UpdatePedido(pedidoNovo, id);
        }

        public Pedido BuscarPedidoPorUsuario(string userId, string pedidoId)
        {
            return _pedidoService.GetPedidoByUser(userId, pedidoId);
        }

        public List<Pedido> BuscarPedidos()
        {
            return _pedidoService.GetPedido();
        }

        public List<Pedido> BuscarPedidosPorUsuario(string userId)
        {
            return _pedidoService.GetPedidosByUser(userId);
        }

        public Pedido CriarPedido(string userId, Carrinho carrinho)
        {
            if (carrinho != null)
            {
                var endereco = _enderecoRepository.BuscarEndereco(carrinho.EnderecoId);

                Pedido pedido = new Pedido();

                pedido.UserId = userId;
                pedido.NomeEndereco = endereco.NomeEndereco;
                pedido.Uf = endereco.Uf;
                pedido.Cidade = endereco.Cidade;
                pedido.Cep = endereco.Cep;
                pedido.Bairro = endereco.Bairro;
                pedido.Rua = endereco.Rua;
                pedido.Numero = endereco.Numero;
                pedido.Complemento = endereco.Complemento;
                pedido.Produtos = carrinho.Produtos;
                pedido.DataPedidoRealizado = DateTime.Now;

                //Verificar se todos os produtos estão ativos e com estoque
                foreach (ProdutosCarrinho prod in carrinho.Produtos)
                {
                    var produto = _produtoRepository.BuscarProduto(prod.IdProduto);

                    if (produto.Quantity <= prod.Quantidade || !produto.Ativo)
                    {
                        return null;
                    }
                }

                //Somar preços e atualizar estoque dos produtos
                foreach (ProdutosCarrinho prod in carrinho.Produtos)
                {
                    var produto = _produtoRepository.BuscarProduto(prod.IdProduto);

                    pedido.VlFinal += (prod.Preco * prod.Quantidade);
                    pedido.VlFrete += prod.Frete;

                    //Atualizar estoque
                    produto.Quantity -= prod.Quantidade;
                    _produtoRepository.AtualizarProduto(prod.IdUserVenda, prod.IdProduto, produto);
                }

                pedido.VlTotal = pedido.VlFinal + pedido.VlFrete;

                var user = _usuarioRepository.BuscarUsuario(userId);

                string products = "";
                foreach (var prod in pedido.Produtos)
                {
                    products = products + "\n" + prod.NameProduto;
                }
                _emailRepository.EnviarEmail(user.Email, "Pedido realizado com sucesso!", $"Caro(a) {user.Name}, seu pedido foi processado em nosso sistema. \nProdutos: \n{products}");

                return _pedidoService.CreatePedido(pedido);
            }
            return null;
        }

        public void DeletarPedidoPorUsuario(string userId)
        {
            _pedidoService.EndPedidoByUser(userId);
        }
    }
}