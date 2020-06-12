using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Models
{
    public class Venda
    {
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("userIdVenda")]
		[BsonRequired]
		[BsonRepresentation(BsonType.ObjectId)]
		public string UserIdVenda { get; set; }

		[BsonElement("pedidoIdCompra")]
		[BsonRequired]
		[BsonRepresentation(BsonType.ObjectId)]
		public string PedidoIdCompra { get; set; }

		[BsonElement("NomeEnderecoCompra")]
		[BsonRequired]
		public string NomeEnderecoCompra { get; set; }

		[BsonElement("UfCompra")]
		[BsonRequired]
		public string UfCompra { get; set; }

		[BsonElement("CidadeCompra")]
		[BsonRequired]
		public string CidadeCompra { get; set; }

		[BsonElement("CepCompra")]
		[BsonRequired]
		public string CepCompra { get; set; }

		[BsonElement("BairroCompra")]
		[BsonRequired]
		public string BairroCompra { get; set; }

		[BsonElement("RuaCompra")]
		[BsonRequired]
		public string RuaCompra { get; set; }

		[BsonElement("NumeroCompra")]
		public int NumeroCompra { get; set; }

		[BsonElement("ComplementoCompra")]
		public string Complemento { get; set; }

		[BsonElement("IdProdutoCompra")]
		public string IdProdutoCompra { get; set; }

		[BsonElement("vlFinalCompra")]
		public double VlFinalCompra { get; set; }

		[BsonElement("vlFreteCompra")]
		public double VlFreteCompra { get; set; }

		[BsonElement("vlTotalCompra")]
		public double VlTotalCompra { get; set; }

		[BsonElement("dataPedidoRealizadoCompra")]
		public DateTime DataPedidoRealizadoCompra { get; set; }

		[BsonElement("dataPagamentoCompra")]
		public DateTime DataPagamentoConfirmadoCompra { get; set; }

		[BsonElement("dataEmTransporteCompra")]
		public DateTime DataEmTransporteCompra { get; set; }

		[BsonElement("dataCancelamentoCompra")]
		public DateTime DataCancelamentoCompra { get; set; }

		[BsonElement("__v")]
		[BsonRepresentation(BsonType.Int32)]
		public Int32 version { get; set; }
	}
}
