using BackECommerce.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackECommerce.Service.Services
{
    public class PedidoService
    {
        private readonly IMongoCollection<Pedido> _pedido;
        public PedidoService()
        {
            MongoClient client = new MongoClient("mongodb+srv://UserAdmin:projetocalebe@clusterecommerce-onzbe.mongodb.net/test?retryWrites=true&w=majority");
            IMongoDatabase database = client.GetDatabase("ProjetoEcommerce");
            _pedido = database.GetCollection<Pedido>("Orders");
        }

        public Pedido CreatePedido(Pedido pedido)
        {
            pedido.DataPedidoRealizado = DateTime.Now;
            _pedido.InsertOne(pedido);
            return pedido;
        }

        public void EndPedidoByUser(string userId)
        {
            _pedido.DeleteOne<Pedido>(pedido => pedido.UserId == userId);
        }

        public List<Pedido> GetPedidos()
        {
            return _pedido.Find(pedido => true).ToList();
        }

        public List<Pedido> GetPedidosByUser(string id)
        {
            return _pedido.Find<Pedido>(pedido => pedido.Id == id).ToList();
        }

        public Pedido GetPedidoByUser(string userId, string pedidoId)
        {
            return _pedido.Find<Pedido>(pedido => pedido.UserId == userId && pedido.Id == pedidoId).FirstOrDefault();
        }

        public void UpdatePedido(Pedido pedidoNovo, string id)
        {
            _pedido.ReplaceOne(pedido => pedido.UserId == id, pedidoNovo);
        }

        public Pedido GetPedido(string pedidoId)
        {
            return _pedido.Find<Pedido>(p => p.Id == pedidoId).FirstOrDefault();
        }

        public List<Pedido> GetPedidosAndamentoByUser(string id)
        {
            return _pedido.Find<Pedido>(pedido => pedido.Id == id && !pedido.StatusFinalizado).ToList();
        }
    }
}
