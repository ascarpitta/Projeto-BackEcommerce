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
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ProdutoService _produtoService;
        public ProdutoRepository()
        {
            _produtoService = new ProdutoService();
        }

        public Produto BuscarProduto(string id)
        {
            return _produtoService.GetProdutoById(id);
        }

        public List<Produto> BuscarProdutosPorUsuario(string userId)
        {
            return _produtoService.GetProdutosByUser(userId);
        }

        public List<Produto> BuscarProdutos()
        {
            //ProdutoService 
            return _produtoService.GetProduto();
        }

        public void CadastroProduto(Produto produto)
        {
            produto.Ativo = true;
            _produtoService.CreateProduto(produto);
        }

        public Produto BuscarProdutoPorUsuario(string userId, string id)
        {
            return _produtoService.GetProdutoByUser(userId, id);
        }

        public List<Produto> BuscarProdutosPorNome(string nome)
        {
            return _produtoService.GetProdutosByName(nome);
        }

        public List<Produto> BuscarProdutoPorNomeOrdemMaiorPreco(string nome)
        {
            return _produtoService.GetProdutosByNameOrderByPriceDesc(nome);
        }

        public List<Produto> BuscarProdutoPorNomeOrdemMenorPreco(string nome)
        {
            return _produtoService.GetProdutosByNameOrderByPrice(nome);
        }

        public List<Produto> BuscarProdutoPorNomeOrdemAbc(string nome)
        {
            return _produtoService.GetProdutosByNameAsc(nome);
        }

        public List<Produto> BuscarProdutoPorNomeOrdemZyx(string nome)
        {
            return _produtoService.GetProdutosByNameDesc(nome);
        }

        public Produto AtualizarProduto(string userId, string produtoId, Produto produtoNovo)
        {
            var prod = BuscarProduto(produtoId);
            if (prod != null)
            {
                if (prod.User == userId)
                {
                    _produtoService.UpdateProduto(produtoId, produtoNovo);
                    return BuscarProduto(produtoId);
                }
            }
            return null;
        }

        public Produto AtivaProduto(string userId, string produtoId)
        {
            var prod = BuscarProduto(produtoId);
            if (prod != null)
            {
                if (prod.User == userId)
                {
                    prod.Ativo = true;
                    return AtualizarProduto(userId, produtoId, prod);
                }
            }
            return null;
        }

        public Produto InativaProduto(string userId, string produtoId)
        {
            var prod = BuscarProduto(produtoId);
            if (prod != null)
            {
                if (prod.User == userId)
                {
                    prod.Ativo = false;
                    return AtualizarProduto(userId, produtoId, prod);
                }
            }
            return null;
        }
    }
}
