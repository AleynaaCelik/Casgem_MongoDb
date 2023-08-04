using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Estate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("price")]
        public int Price { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("room")]
        public int Room { get; set; }

        [BsonElement("buildYear")]
        public string BuildYear { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("userName")]
        public string UserName { get; set; }
    }
}
