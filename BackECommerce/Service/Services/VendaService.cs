using BackECommerce.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Service.Services
{
    public class VendaService
    {
        public static DateTime Today { get; }

        private readonly IMongoCollection<Venda> _venda;
        public VendaService()
        {
            MongoClient client = new MongoClient("mongodb+srv://UserAdmin:projetocalebe@clusterecommerce-onzbe.mongodb.net/test?retryWrites=true&w=majority");
            IMongoDatabase database = client.GetDatabase("ProjetoEcommerce");
            _venda = database.GetCollection<Venda>("Sales");
        }

        public void UpdateSale(Venda vendaAtualizada, string id)
        {
            _venda.ReplaceOne(v => v.UserIdVenda == id, vendaAtualizada);
        }

        public Venda CreateSale(Venda venda)
        {
            _venda.InsertOne(venda);
            return venda;
        }

        public void EndSaleByUser(string userId)
        {
            _venda.DeleteOne<Venda>(v => v.UserIdVenda == userId);
        }

        public List<Venda> GetSales()
        {
            return _venda.Find(v => true).ToList();
        }

        public List<Venda> GetSalesByUser(string id)
        {
            return _venda.Find<Venda>(v => v.UserIdVenda == id).ToList();
        }

        public Venda GetSaleByUser(string userId, string vendaId)
        {
            return _venda.Find<Venda>(v => v.UserIdVenda == userId && v.Id == vendaId).FirstOrDefault();
        }

        public Venda GetSale(string vendaId)
        {
            return _venda.Find<Venda>(v => v.Id == vendaId).FirstOrDefault();
        }

        public List<Venda> GetSalesAndamentoByUser(string userId)
        {
            return null;
            //return _venda.Find<Venda>(v => v.);
        }
    }
}
