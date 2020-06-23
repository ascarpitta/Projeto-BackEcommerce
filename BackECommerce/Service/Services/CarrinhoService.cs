using BackECommerce.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackECommerce.Service.Services
{
    public class CarrinhoService
    {
        public static DateTime Today { get; }

        private readonly IMongoCollection<Carrinho> _carrinho;
        public CarrinhoService()
        {
            MongoClient client = new MongoClient("mongodb+srv://UserAdmin:projetocalebe@clusterecommerce-onzbe.mongodb.net/test?retryWrites=true&w=majority");
            IMongoDatabase database = client.GetDatabase("ProjetoEcommerce");
            _carrinho = database.GetCollection<Carrinho>("Carrinho");
        }

        public void UpdateCarrinho(Carrinho carrinhoNovo, string id)
        {
            _carrinho.ReplaceOne(carrinho => carrinho.UserId == id, carrinhoNovo);
        }

        public Carrinho CreateCarrinho(Carrinho carrinho)
        {
            carrinho.CreatedAt = DateTime.Now;
            _carrinho.InsertOne(carrinho);
            return carrinho;
        }

        public void EndCarrinhoByUser(string userId)
        {
            _carrinho.DeleteOne<Carrinho>(carrinho => carrinho.UserId == userId);
        }

        public void EndCarrinhos()
        {
            _carrinho.DeleteMany<Carrinho>(c => c.CreatedAt.AddDays(3) <= (DateTime.Today.AddDays(-3)));
        }

        public List<Carrinho> GetCarrinho()
        {
            return _carrinho.Find(carrinho => true).ToList();
        }

        public Carrinho GetCarrinhoByUser(string id)
        {
            return _carrinho.Find<Carrinho>(carrinho => carrinho.UserId == id).FirstOrDefault();
        }

        public Carrinho GetCarrinhoById(string id)
        {
            return _carrinho.Find<Carrinho>(carrinho => carrinho.Id == id).FirstOrDefault();
        }
    }
}
