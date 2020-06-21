using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Models
{
    public class Endereco
    {
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("user")]
		[BsonRequired]
		[BsonRepresentation(BsonType.ObjectId)]
		public string User { get; set; }

		[BsonElement("NomeEndereco")]
		[BsonRequired]
		public string NomeEndereco { get; set; }

		[BsonElement("Uf")]
		[BsonRequired]
		public string Uf { get; set; }

		[BsonElement("Cidade")]
		[BsonRequired]
		public string Cidade { get; set; }

		[BsonElement("Cep")]
		[BsonRequired]
		public string Cep { get; set; }

		[BsonElement("Bairro")]
		[BsonRequired]
		public string Bairro { get; set; }

		[BsonElement("Rua")]
		[BsonRequired]
		public string Rua { get; set; }

		[BsonElement("Numero")]
		public int Numero { get; set; }

		[BsonElement("Complemento")]
		public string Complemento { get; set; }

		[BsonElement("createdAt")]
		public DateTime CreatedAt { get; set; }

		[BsonElement("__v")]
		[BsonRepresentation(BsonType.Int32)]
		public Int32 version { get; set; }
	}

	public class EnderecoViaCep
	{
		public string Cep { get; set; }
		public string Bairro { get; set; }
		public string Cidade { get; set; }
		public string Rua { get; set; }
		public string Uf { get; set; }
	}
}
