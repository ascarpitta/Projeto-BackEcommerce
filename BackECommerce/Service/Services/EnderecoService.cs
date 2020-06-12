using BackECommerce.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Service.Services
{
    public class EnderecoService
    {
        private readonly IMongoCollection<Endereco> _enderecos;

        public EnderecoService()
        {
            MongoClient client = new MongoClient("mongodb+srv://UserAdmin:projetocalebe@clusterecommerce-onzbe.mongodb.net/test?retryWrites=true&w=majority");
            IMongoDatabase database = client.GetDatabase("ProjetoEcommerce");
            _enderecos = database.GetCollection<Endereco>("Address");
        }

        public Endereco CreateEndereco(Endereco endereco)
        {
            endereco.CreatedAt = DateTime.Now;
            _enderecos.InsertOne(endereco);
            return endereco;
        }

        public List<Endereco> GetEndereco()
        {
            return _enderecos.Find(endereco => true).ToList();
        }

        public Endereco GetEnderecoById(string id)
        {
            return _enderecos.Find<Endereco>(endereco => endereco.Id == id).FirstOrDefault();
        }

        public List<Endereco> GetEnderecoByName(string nome)
        {
            return _enderecos.Find<Endereco>(endereco => endereco.NomeEndereco == nome).ToList();
        }

        public List<Endereco> GetEnderecoByUser(string userId)
        {
            return _enderecos.Find<Endereco>(endereco => endereco.User == userId).ToList();
        }

        public void RemoveEndereco(string enderecoId)
        {
            _enderecos.DeleteOne<Endereco>(endereco => endereco.Id == enderecoId);
        }

        public void UpdateEndereco(string id, Endereco enderecoNovo)
        {
            _enderecos.ReplaceOne(endereco => endereco.Id == id, enderecoNovo);
        }
    }
}
