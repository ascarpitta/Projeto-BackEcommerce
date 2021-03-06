﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace BackECommerce.Models
{
    public class Carrinho
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("enderecoId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string EnderecoId { get; set; }

        [BsonElement("produtos")]
        public List<ProdutosCarrinho> Produtos { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("__v")]
        [BsonRepresentation(BsonType.Int32)]
        public Int32 version { get; set; }
    }

    public class ProdutosCarrinho 
    {
        [BsonElement("IdProduto")]
        public string IdProduto { get; set; }

        [BsonElement("NameProduto")]
        public string NameProduto { get; set; }

        [BsonElement("quantidade")]
        public int Quantidade { get; set; }

        [BsonElement("preco")]
        public double Preco { get; set; }

        [BsonElement("url_imagem")]
        public string url_imagem { get; set; }

        [BsonElement("frete")]
        public double Frete { get; set; }

        [BsonElement("idUservenda")]
        public string IdUserVenda { get; set; }

        [BsonElement("dataNfEmitida")]
        public DateTime DataNfEmitida { get; set; }

        [BsonElement("dataItemEmTransporte")]
        public DateTime DataItemEmTransporte { get; set; }

        [BsonElement("dataItemEntregue")]
        public DateTime DataItemEntregue { get; set; }

        [BsonElement("dataItemCancelado")]
        public DateTime DataItemCancelado { get; set; }

        [BsonElement("statusEmTransporte")]
        public bool StatusEmTransporte { get; set; }

        [BsonElement("statusEntregue")]
        public bool StatusEntregue { get; set; }

        [BsonElement("statusCancelado")]
        public bool StatusCancelado { get; set; }
    }
}
