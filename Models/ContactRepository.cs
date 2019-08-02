using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Linq;

namespace HSPD_API.Models
{
    public class ContactRepository : IContactRepository
    {
        private readonly IMongoDatabase _database = null;
        private readonly IMongoCollection<Contact> _contacts = null;

        public ContactRepository(string connection)
        {
            if (string.IsNullOrEmpty(connection))
            {
                connection = "mongodb://localhost:27017";
            }

            MongoClient client = new MongoClient(connection);
            _database = client.GetDatabase("Contacts");
            _contacts = _database.GetCollection<Contact>("contacts");

            _contacts.DeleteMany(e => e != null);
            for (int i = 0; i < 5; i++)
            {
                Contact contact = new Contact
                {
                    Email = string.Format("test{0}@example.com", i),
                    Name = string.Format("test{0}", i),
                    Phone = string.Format("{0}{0}{0} {0}{0}{0} {0}{0}{0}{0}", i)
                };
                AddContact(contact);
            }
        }
        public Contact AddContact(Contact item)
        {
            item.Id = ObjectId.GenerateNewId().ToString();
            item.LastModified = DateTime.UtcNow;
            _contacts.InsertOne(item);
            return item;
        }

        public IEnumerable GetAllContacts()
        {
            yield return _contacts.Find(Builders<Contact>.Filter.Empty);
        }

        public Contact GetContact(string id)
        {
            return _contacts.Find(Builders<Contact>.Filter.Eq("_id", id)).FirstOrDefault();
        }

        public bool RemoveContact(string id)
        {
            DeleteResult result = _contacts.DeleteOne(Builders<Contact>.Filter.Eq("_id", id));
            return result.DeletedCount == 1;
        }

        public bool UpdateContact(string id, Contact item)
        {
            UpdateResult result = _contacts.UpdateMany(Builders<Contact>.Filter.Eq("_id", id),
                Builders<Contact>.Update
                .Set("Email", item.Email)
                .Set("LastModified", DateTime.UtcNow)
                .Set("Name", item.Name)
                .Set("Phone", item.Phone));
            return result.IsAcknowledged;
        }
    }
}