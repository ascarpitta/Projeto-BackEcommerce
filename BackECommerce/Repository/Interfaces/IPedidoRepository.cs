using BackECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Repository.Interfaces
{
    public interface IPedidoRepository
    {
        List<Pedido> BuscarPedidos();
        Pedido BuscarPedido(string pedidoId);
        List<Pedido> BuscarPedidosPorUsuario(string userId);
        Pedido BuscarPedidoPorUsuario(string userId, string pedidoId);
        Pedido CriarPedido(string userId, Carrinho carrinho);
        Pedido PagarPedido(string userId, string pedidoId);
        Pedido AtualizarStatusPedidoCompra(string userId, string pedidoId, string produtoId, int tipo);
        void AtualizarPedido(Pedido pedidoNovo, string id);
        void DeletarPedidoPorUsuario(string userId);
        List<Pedido> PedidosCompraEmAndamento(string userId);

        //Venda
        List<Venda> BuscarVendas();
        List<Venda> BuscarVendasPorUsuario(string id);
        Venda BuscarVendaPorUsuario(string userId, string vendaId);
        Venda BuscarVendaPorUsuarioPorPedido(string userId, string pedidoCompraId);
        Venda CriarVenda(Venda venda);
        Venda AtualizarStatusPedido(string userId, string vendaId, int tipo);
        List<Venda> PedidosVendaEmAndamento(string userId);
        void GerarReciboProduto(string userId, string pedidoId, string produtoId);
    }
}
