using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Models
{
    public class Pedido
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("userId")]
		[BsonRequired]
		[BsonRepresentation(BsonType.ObjectId)]
		public string UserId { get; set; }

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

		[BsonElement("produtos")]
		public List<ProdutosCarrinho> Produtos { get; set; }

		[BsonElement("vlFinal")]
		public double VlFinal { get; set; }

		[BsonElement("vlFrete")]
		public double VlFrete { get; set; }

		[BsonElement("vlTotal")]
		public double VlTotal { get; set; }

		[BsonElement("dataPedidoRealizado")]
		public DateTime DataPedidoRealizado { get; set; }

		[BsonElement("dataPagamento")]
		public DateTime DataPagamentoConfirmado { get; set; }

		[BsonElement("__v")]
		[BsonRepresentation(BsonType.Int32)]
		public Int32 version { get; set; }
	}
}
