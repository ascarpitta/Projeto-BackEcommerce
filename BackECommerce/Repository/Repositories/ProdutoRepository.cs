using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using BackECommerce.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Repository.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ProdutoService _produtoService = new ProdutoService();
        private readonly CarrinhoService _carrinhoService = new CarrinhoService();
        
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
                    if (produtoNovo.Quantity == 0)
                    {
                        foreach (string carrinhoId in prod.Carrinhos) //Remover produto dos carrinhos
                        {
                            var car = _carrinhoService.GetCarrinhoById(carrinhoId);
                            car.Produtos.RemoveAll(c => c.IdProduto == produtoId);
                            _carrinhoService.UpdateCarrinho(car, car.UserId);
                        }
                        produtoNovo.Carrinhos = null;
                    }
                    _produtoService.UpdateProduto(produtoId, produtoNovo);
                    return BuscarProduto(produtoId);
                }
            }
            return null;
        }

        public Produto AtualizarDadosProduto(string produtoId, Produto produtoNovo)
        {
            var prod = BuscarProduto(produtoId);
            if (prod != null)
            {
                _produtoService.UpdateProdutoById(produtoId, produtoNovo);
                return BuscarProduto(produtoId);
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

                    foreach (string carrinhoId in prod.Carrinhos)
                    {
                        var car = _carrinhoService.GetCarrinhoById(carrinhoId);
                        car.Produtos.RemoveAll(c => c.IdProduto == produtoId);
                        _carrinhoService.UpdateCarrinho(car, car.UserId);
                    }
                    prod.Carrinhos = null;
                    return AtualizarProduto(userId, produtoId, prod);
                }
            }
            return null;
        }

        public List<Produto> Filtro(string categoria)
        {
            return _produtoService.GetProdutosByCategory(categoria);
        }
    }
}
