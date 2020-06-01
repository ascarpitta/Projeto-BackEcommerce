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
        Carrinho AumentarQuantProduto(string userId, string produtoId);
        Carrinho DiminuirQuantProduto(string userId, string produtoId);
        Carrinho RemoverProduto(string userId, string produtoId);
        void RemoverCarrinhoPorUsuario(string userId);
        Pedido FinalizarCarrinho(string userId);
    }
}

