using BackECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Service.Interfaces
{
    public interface IPedidoService
    {
        List<Pedido> GetPedidos();
        Pedido GetPedido(string pedidoId);
        List<Pedido> GetPedidosByUser(string id);
        Pedido GetPedidoByUser(string userId, string pedidoId);
        Pedido CreatePedido(Pedido pedido);
        void UpdatePedido(Pedido pedidoNovo, string id);
        void EndPedidoByUser(string userId);
    }
}
