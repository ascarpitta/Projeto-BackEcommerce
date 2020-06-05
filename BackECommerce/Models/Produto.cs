using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BackECommerce.Models
{
    public class Produto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("description")]
        [BsonRequired]
        public string Description { get; set; }

        [BsonElement("price")]
        [BsonRequired]
        public double Price { get; set; }

        [BsonElement("frete")]
        public double Frete { get; set; }

        [BsonElement("quantity")]
        [BsonRequired]
        public int Quantity { get; set; }

        [BsonElement("category")]
        [BsonRequired]
        public string Category { get; set; }

        [BsonElement("marca")]
        public string Marca { get; set; }

        [BsonElement("user")]
        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string User { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("__v")]
        [BsonRepresentation(BsonType.Int32)]
        public Int32 version { get; set; }

        [BsonElement("ativo")]
        [BsonRepresentation(BsonType.Boolean)]
        public bool Ativo { get; set; }
    }
}
