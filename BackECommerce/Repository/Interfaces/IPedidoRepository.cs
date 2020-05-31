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
        List<Pedido> BuscarPedidosPorUsuario(string userId);
        Pedido BuscarPedidoPorUsuario(string userId, string pedidoId);
        Pedido CriarPedido(string userId, Carrinho carrinho);
        void AtualizarPedido(Pedido pedidoNovo, string id);
        void DeletarPedidoPorUsuario(string userId);
    }
}
