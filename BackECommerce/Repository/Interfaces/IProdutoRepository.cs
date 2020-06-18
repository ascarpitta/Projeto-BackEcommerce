using BackECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Repository.Interfaces
{
    public interface IProdutoRepository
    {
        void CadastroProduto(Produto produto); //No plano de teste
        List<Produto> BuscarProdutos(); //No plano de teste
        Produto BuscarProduto(string id); //No plano de teste
        List<Produto> BuscarProdutosPorUsuario(string userId); //No plano de teste
        Produto BuscarProdutoPorUsuario(string userId, string id); //No plano de teste
        List<Produto> BuscarProdutosPorNome(string nome); //No plano de teste
        List<Produto> BuscarProdutoPorNomeOrdemMaiorPreco(string nome); //No plano de teste
        List<Produto> BuscarProdutoPorNomeOrdemMenorPreco(string nome); //No plano de teste
        List<Produto> BuscarProdutoPorNomeOrdemAbc(string nome); //No plano de teste
        List<Produto> BuscarProdutoPorNomeOrdemZyx(string nome); //No plano de teste
        Produto AtualizarProduto(string userId, string produtoid, Produto produtoNovo); //No plano de teste
        Produto AtivaProduto(string userId, string produtoId); //No plano de teste
        Produto InativaProduto(string userId, string produtoId); //No plano de teste
        List<Produto> Filtro(string categoria);
    }
}
