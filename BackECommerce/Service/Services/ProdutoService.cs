using BackECommerce.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Service.Services
{
    public class ProdutoService 
    {
        private readonly IMongoCollection<Produto> _produtos;

        public ProdutoService()
        {
            MongoClient client = new MongoClient("mongodb+srv://UserAdmin:projetocalebe@clusterecommerce-onzbe.mongodb.net/test?retryWrites=true&w=majority");
            IMongoDatabase database = client.GetDatabase("ProjetoEcommerce");
            _produtos = database.GetCollection<Produto>("Products");
        }

        public List<Produto> GetProduto()
        {
            return _produtos.Find(produto => true).ToList();
        }

        public Produto GetProdutoById(string id)
        {
            return _produtos.Find<Produto>(produto => produto.Id == id).FirstOrDefault();
        }

        public Produto CreateProduto(Produto produto)
        {
            produto.CreatedAt = DateTime.Now;
            _produtos.InsertOne(produto);
            return produto;
        }
        
        public void UpdateProduto(string id, Produto produtoNovo)
        {
            _produtos.ReplaceOne(produto => produto.Id == id, produtoNovo);
        }

        public List<Produto> GetProdutosByName(string nome)
        {
            return _produtos.Find<Produto>(produto => produto.Name.Contains(nome)).ToList();
        }

        public List<Produto> GetProdutosByUser(string userId)
        {
            return _produtos.Find<Produto>(produto => produto.User == userId).ToList();
        }

        public Produto GetProdutoByUser(string userId, string id)
        {            
            return _produtos.Find<Produto>(produto => produto.User == userId && produto.Id == id).FirstOrDefault();
        }

        public List<Produto> GetProdutosByNameOrderByPrice(string nome)
        {
            var lista = _produtos.Find<Produto>(produto => produto.Name.Contains(nome)).ToList();
            var listaOrdenada = from s in lista orderby s.Price select s;
            return listaOrdenada.ToList();
        }

        public List<Produto> GetProdutosByNameOrderByPriceDesc(string nome)
        {
            var lista = _produtos.Find<Produto>(produto => produto.Name.Contains(nome)).ToList();
            var listaOrdenada = from s in lista orderby s.Price descending select s;
            return listaOrdenada.ToList();
        }

        public List<Produto> GetProdutosByNameAsc(string nome)
        {
            var lista = _produtos.Find<Produto>(produto => produto.Name.Contains(nome)).ToList();
            var listaOrdenada = from s in lista orderby s.Name select s;
            return listaOrdenada.ToList();
        }

        public List<Produto> GetProdutosByNameDesc(string nome)
        {
            var lista = _produtos.Find<Produto>(produto => produto.Name.Contains(nome)).ToList();
            var listaOrdenada = from s in lista orderby s.Name descending select s;
            return listaOrdenada.ToList();
        }
    }
}
