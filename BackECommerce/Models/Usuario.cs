using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace BackECommerce.Models
{
	public class Usuario
    {
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("name")]
		[BsonRequired]
		public string Name { get; set; }

		[BsonElement("cpf")]
		[BsonRepresentation(BsonType.Int64)]
		[BsonRequired]
		public long Cpf { get; set; }

		[BsonElement("email")]
		[BsonRequired]
		public string Email { get; set; }

		[BsonElement("password")]
		[BsonRequired]
		public string Password { get; set; }

		[BsonElement("createdAt")]
		public DateTime CreatedAt { get; set; }

		[BsonElement("__v")]
		[BsonRepresentation(BsonType.Int32)]
		public Int32 version { get; set; }

		[BsonElement("wishList")]
		public List<ListaDesejo> ListaDesejos { get; set; }

		[BsonElement("ativo")]
		[BsonRepresentation(BsonType.Boolean)]
		public bool Ativo { get; set; }
	}

	public class ProdutoVenda
	{
		[BsonElement("IdProdutoVenda")]
		public string IdProdutoVenda { get; set; }
		public ProdutoVenda(string id)
		{
			this.IdProdutoVenda = id;
		}
	}

	public class ListaDesejo
	{
		[BsonElement("IdProduto")]
		public string IdProduto { get; set; }

		[BsonElement("NameProduto")]
		public string NameProduto { get; set; }

		public ListaDesejo(string id, string nome)
		{
			this.IdProduto = id;
			this.NameProduto = nome;
		}
	}
}
