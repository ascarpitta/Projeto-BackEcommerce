using BackECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Service.Interfaces
{
    public interface IProdutoService
    {
        List<Produto> GetProduto();
        Produto GetProdutoById(string id);
        List<Produto> GetProdutosByName(string nome);
        List<Produto> GetProdutosByUser(string userId);
        Produto GetProdutoByUser(string userId, string id);
        Produto CreateProduto(Produto produto);
        void UpdateProduto(string id, Produto produtoNovo);
        List<Produto> GetProdutosByNameOrderByPrice(string nome);
        List<Produto> GetProdutosByNameOrderByPriceDesc(string nome);
        List<Produto> GetProdutosByNameAsc(string nome);
        List<Produto> GetProdutosByNameDesc(string nome);
    }
}
