using BackECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Repository.Interfaces
{
    public interface ICarrinhoRepository
    {
        List<Carrinho> BuscarCarrinhos();
        Carrinho BuscarCarrinhoPorUsuario(string id);
        Carrinho CriarCarrinho(string produtoId);
        Carrinho AddProduto(string userId, string produtoId);
        Carrinho AddEndereco(string userId, string enderecoId);
        Carrinho AlterarQuantProduto(string userId, string produtoId, int tipo);
        Carrinho RemoverProduto(string userId, string produtoId);
        void RemoverCarrinhoPorUsuario(string userId);
        Pedido FinalizarCarrinho(string userId);
    }
}

