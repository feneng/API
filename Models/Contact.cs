using MongoDB.Bson.Serialization.Attributes;
using System;

namespace HSPD_API.Models
{
    
    public class Contact:Bson
    {
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime LastModified { get; set; }
    }
}