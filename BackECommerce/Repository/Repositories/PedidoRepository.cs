using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using BackECommerce.Service.Interfaces;
using BackECommerce.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Repository.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly IPedidoService _pedidoService;
        //private readonly IVendaRepository _vendaRepository = new VendaRepository();
        private readonly IProdutoRepository _produtoRepository = new ProdutoRepository();
        private readonly IUsuarioRepository _usuarioRepository = new UsuarioRepository();
        private readonly IEnderecoRepository _enderecoRepository = new EnderecoRepository();
        private EmailRepository _emailRepository = new EmailRepository();
        public PedidoRepository()
        {
            _pedidoService = new PedidoService();
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
            return _pedidoService.GetPedidos();
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
                //Venda venda = new Venda();

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

                //venda.BairroCompra = pedido.Bairro;
                //venda.CepCompra = pedido.Cep;
                //venda.CidadeCompra = pedido.Cidade;
                //venda.Complemento = pedido.Complemento;
                //venda.NomeEnderecoCompra = pedido.NomeEndereco;
                //venda.NumeroCompra = pedido.Numero;
                //venda.PedidoIdCompra = pedido.Id;
                //venda.RuaCompra = pedido.Rua;
                //venda.UfCompra = pedido.Uf;

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

                    //criar pedido de venda
                    //venda.DataPedidoRealizadoCompra = DateTime.Now;
                    //venda.IdProdutoCompra = produto.Id;
                    //venda.UserIdVenda = produto.User;
                    //venda.VlFinalCompra = prod.Preco * prod.Quantidade;
                    //venda.VlFreteCompra = prod.Frete;
                    //venda.VlTotalCompra = venda.VlFinalCompra + venda.VlFreteCompra;
                    //_vendaRepository.CriarVenda(venda);
                }

                pedido.VlTotal = pedido.VlFinal + pedido.VlFrete;

                var user = _usuarioRepository.BuscarUsuario(userId);

                string products = "";
                foreach (var prod in pedido.Produtos)
                {
                    products = products + "\n" + prod.NameProduto;
                }
                _emailRepository.EnviarEmail(user.Email, "Pedido confirmado com sucesso!", $"Caro(a) {user.Name}, \n\nseu pedido de número {pedido.Numero} foi processado em nosso sistema.\n\nObrigado por comprar em nossa loja!");

                return _pedidoService.CreatePedido(pedido);
            }
            return null;
        }

        public void DeletarPedidoPorUsuario(string userId)
        {
            _pedidoService.EndPedidoByUser(userId);
        }

        public Pedido PagarPedido(string userId, string pedidoId)
        {

            var pedido = BuscarPedidoPorUsuario(userId, pedidoId);
            if (pedido != null)
            {
                pedido.DataPagamentoConfirmado = DateTime.Now;

                AtualizarPedido(pedido, pedido.Id);
                return pedido;
            }//pedido não encontrado
            return null;
        }

        public Pedido AtualizarStatusPedidoCompra(string userId, string pedidoId, string produtoId, int tipo)
        {
            var pedido = BuscarPedidoPorUsuario(userId, pedidoId);
            if (pedido != null)
            {
                foreach (ProdutosCarrinho item in pedido.Produtos)
                {
                    if (item.IdProduto == produtoId)
                    {
                        if (tipo == 0) //item cancelado
                        {
                            if (item.DataItemSeparacao != null && item.DataItemSeparacao.Year < 2020) //item não saiu para entrega
                            {
                                item.DataItemCancelado = DateTime.Now;
                            }
                        }
                        else if (tipo == 1) //item recebido
                        {
                            if (item.DataItemSeparacao != null && item.DataItemSeparacao >= item.DataNfEmitida) //item enviado e com nf
                            {
                                item.DataItemEntregue = DateTime.Now;
                            }
                        }
                    }
                }
                AtualizarPedido(pedido, pedidoId);
                return pedido;
            }//pedido não encontrado
            return null;
        }

        public Pedido BuscarPedido(string pedidoId)
        {
            if (pedidoId.Length == 24)
            {
                return _pedidoService.GetPedido(pedidoId);
            }
            return null;
        }
    }
}