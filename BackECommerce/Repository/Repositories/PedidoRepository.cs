using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using BackECommerce.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Repository.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidoService _pedidoService = new PedidoService();
        private readonly VendaService _vendaService = new VendaService();
        private readonly IProdutoRepository _produtoRepository = new ProdutoRepository();
        private readonly IUsuarioRepository _usuarioRepository = new UsuarioRepository();
        private readonly IEnderecoRepository _enderecoRepository = new EnderecoRepository();
        private readonly EmailRepository _emailRepository = new EmailRepository();
        
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
                Venda venda = new Venda();
                ProdutosCarrinho produtoCarrinho = new ProdutosCarrinho();

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
                pedido.StatusFinalizado = false;

                venda.BairroCompra = pedido.Bairro;
                venda.CepCompra = pedido.Cep;
                venda.CidadeCompra = pedido.Cidade;
                venda.Complemento = pedido.Complemento;
                venda.NomeEnderecoCompra = pedido.NomeEndereco;
                venda.NumeroCompra = pedido.Numero;
                venda.PedidoIdCompra = pedido.Id;
                venda.RuaCompra = pedido.Rua;
                venda.UfCompra = pedido.Uf;

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

                    //Adicionar produto no pedido
                    produtoCarrinho.Preco = prod.Preco * prod.Quantidade;
                    produtoCarrinho.Quantidade = prod.Quantidade;
                    produtoCarrinho.NameProduto = prod.NameProduto;
                    produtoCarrinho.IdUserVenda = prod.IdUserVenda;
                    produtoCarrinho.IdProduto = prod.IdProduto;
                    produtoCarrinho.Frete = prod.Frete;
                    produtoCarrinho.StatusCancelado = false;
                    produtoCarrinho.StatusEmTransporte = false;
                    produtoCarrinho.StatusEntregue = false;
                    pedido.Produtos.Add(produtoCarrinho);

                    //criar pedido de venda
                    venda.DataPedidoRealizadoCompra = DateTime.Now;
                    venda.StatusCancelado = false;
                    venda.StatusEmTransporte = false;
                    venda.StatusFinalizado = false;
                    venda.IdProdutoCompra = produto.Id;
                    venda.UserIdVenda = produto.User;
                    venda.VlFinalCompra = prod.Preco * prod.Quantidade;
                    venda.VlFreteCompra = prod.Frete;
                    venda.VlTotalCompra = venda.VlFinalCompra + venda.VlFreteCompra;
                    CriarVenda(venda);
                }

                pedido.VlTotal = pedido.VlFinal + pedido.VlFrete;

                var user = _usuarioRepository.BuscarUsuario(userId);

                string products = "";
                foreach (var prod in pedido.Produtos)
                {
                    products = products + "\n" + prod.NameProduto;
                }
                _emailRepository.EnviarEmail(user.Email, "Pedido confirmado com sucesso!", $"Caro(a) {user.Name}, \n\nseu pedido de número {pedido.Numero} foi processado em nosso sistema.\n\nObrigado por comprar em nossa loja!");
                //arrumar numero do email, (esse numero é o do endereco)
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

                //gerar recibo compra

                foreach(ProdutosCarrinho itens in pedido.Produtos)
                {
                    var venda = BuscarVendaPorUsuario(itens.IdUserVenda, pedido.Id);
                    venda.DataPagamentoConfirmadoCompra = DateTime.Now;
                    _vendaService.UpdateSale(venda, venda.Id);

                    //gerar recibo venda
                }
                return pedido;
            }//pedido não encontrado
            return null;
        }

        public Pedido AtualizarStatusPedidoCompra(string userId, string pedidoId, string produtoId, int tipo)
        {
            bool finalizado = true;
            var pedido = BuscarPedidoPorUsuario(userId, pedidoId);
            if (pedido != null)
            {
                if (!pedido.StatusFinalizado) { //se o pedido ainda não foi finalizado
                    foreach (ProdutosCarrinho item in pedido.Produtos)
                    {
                        if (item.IdProduto == produtoId)
                        {
                            var pedidoVenda = BuscarVendaPorUsuario(item.IdUserVenda, pedido.Id);
                            finalizado = false;

                            if (tipo == 0) //item cancelado
                            {
                                if (!item.StatusEntregue && !item.StatusEmTransporte && !item.StatusCancelado)//item não saiu para entrega, não foi entregue e nem cancelado
                                {
                                    item.DataItemCancelado = DateTime.Now;
                                    item.StatusCancelado = true;
                                    pedidoVenda.DataCancelamentoCompra = DateTime.Now;
                                    pedidoVenda.StatusCancelado = true;

                                    _vendaService.UpdateSale(pedidoVenda, pedidoVenda.Id);
                                }
                            }
                            else if (tipo == 1) //item recebido
                            {
                                if (!item.StatusCancelado && !item.StatusEmTransporte && !item.StatusEntregue) //item enviado e com nf
                                {
                                    item.DataItemEntregue = DateTime.Now;
                                    item.StatusEntregue = true;
                                    pedidoVenda.DataPedidoFinalizado = DateTime.Now;
                                    pedidoVenda.StatusFinalizado = true;
                                    _vendaService.UpdateSale(pedidoVenda, pedidoVenda.Id);
                                }
                            }                            
                        }
                    }
                }

                if (finalizado)
                {
                    pedido.DataPedidoFinalizado = DateTime.Now;
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

        public List<Pedido> PedidosCompraEmAndamento(string userId)
        {
            if (userId.Length == 24)
            {
                if (_usuarioRepository.BuscarUsuario(userId) != null)
                {
                    return _pedidoService.GetPedidosAndamentoByUser(userId);
                }
            }
            return null;
        }


        //VENDA
        public Venda BuscarVendaPorUsuario(string userId, string vendaId)
        {
            if (userId.Length == 24 && vendaId.Length == 24)
            {
                var usuario = _usuarioRepository.BuscarUsuario(userId);
                if (usuario != null)
                {
                    return _vendaService.GetSaleByUser(usuario.Id, vendaId);
                }
            }
            return null;
        }

        public List<Venda> BuscarVendas()
        {
            return _vendaService.GetSales();
        }

        public List<Venda> BuscarVendasPorUsuario(string id)
        {
            if (id.Length == 24)
            {
                var usuario = _usuarioRepository.BuscarUsuario(id);
                if (usuario != null)
                {
                    return _vendaService.GetSalesByUser(usuario.Id);
                }
            }
            return null;
        }

        public Venda CriarVenda(Venda venda)
        {
            return _vendaService.CreateSale(venda);
        }

        public Venda AtualizarStatusPedido(string userId, string vendaId, int tipo) //0 cancelado, 1 em transporte
        {
            if (userId.Length == 24 && vendaId.Length == 24)
            {
                var venda = BuscarVendaPorUsuario(userId, vendaId);
                if (venda != null)
                {
                    var pedido = BuscarPedido(venda.PedidoIdCompra);
                    if (pedido != null && !pedido.StatusFinalizado)
                    {
                        foreach (ProdutosCarrinho item in pedido.Produtos)
                        {
                            if (item.IdUserVenda == userId && venda.IdProdutoCompra == item.IdProduto)
                            {
                                if (tipo == 1 && !item.StatusEntregue && !item.StatusEmTransporte && !item.StatusCancelado)
                                {
                                    item.DataItemEmTransporte = DateTime.Now;
                                    item.StatusEmTransporte = true;
                                    venda.DataEmTransporteCompra = DateTime.Now;
                                    venda.StatusEmTransporte = true;

                                    _vendaService.UpdateSale(venda, venda.Id);
                                    AtualizarPedido(pedido, pedido.Id);
                                }
                                else if (tipo == 0 && !item.StatusEntregue && !item.StatusEmTransporte && !item.StatusCancelado)
                                {
                                    item.DataItemCancelado = DateTime.Now;
                                    item.StatusCancelado = true;
                                    venda.DataCancelamentoCompra = DateTime.Now;
                                    venda.StatusCancelado = true;

                                    _vendaService.UpdateSale(venda, venda.Id);
                                    AtualizarPedido(pedido, pedido.Id);
                                }
                                return venda;
                            }
                        }
                    }
                }
            }
            return null;
        }        

        public List<Venda> PedidosVendaEmAndamento(string userId)
        {
            if (userId.Length == 24)
            {
                if (_usuarioRepository.BuscarUsuario(userId) != null)
                {
                    return _vendaService.GetSalesAndamentoByUser(userId);
                }
            }
            return null;
        }
    }
}