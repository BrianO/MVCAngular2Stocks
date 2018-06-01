using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialStocks2.Models
{
    public class mongoCloud
    {
        private const string CONNECTION_STRING_NAME = "mongoCloud";
        private const string DATABASE_NAME = "test";
        private const string CUSTOMERS_COLLECTION_NAME = "customers";


        // This is ok... Normally, they would be put into
        // an IoC container.
        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;

        static mongoCloud()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[CONNECTION_STRING_NAME].ConnectionString;
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(DATABASE_NAME);
        }

        public IMongoClient Client
        {
            get { return _client; }
        }

        public IMongoCollection<BCustomer> Customers
        {
            get { return _database.GetCollection<BCustomer>(CUSTOMERS_COLLECTION_NAME); }
        }

        //public IMongoCollection<User> Users
        //{
        //    get { return _database.GetCollection<User>(CUSTOMERS_COLLECTION_NAME); }
        //}
    }


    public class BCustomer
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string NAME { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }

    }

}