using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace SocialNetworkApp1.Services
{
    public class DatabaseService
    {
        private readonly IMongoDatabase _database;

        public DatabaseService()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("socialNetwork");
        }

        public IMongoCollection<BsonDocument> Users => _database.GetCollection<BsonDocument>("users");
        public IMongoCollection<BsonDocument> Posts => _database.GetCollection<BsonDocument>("posts");

        public BsonDocument Login(string email, string password)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("email", email) & Builders<BsonDocument>.Filter.Eq("password", password);
            return Users.Find(filter).FirstOrDefault();
        }

        public List<BsonDocument> GetNewsFeed()
        {
            var sort = Builders<BsonDocument>.Sort.Descending("createdAt");
            return Posts.Find(new BsonDocument()).Sort(sort).ToList();
        }

        public void AddFriend(string userId, string friendId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            var update = Builders<BsonDocument>.Update.AddToSet("friends", friendId);
            Users.UpdateOne(filter, update);
        }

        public void AddComment(string postId, string userId, string content)
        {
            var postFilter = Builders<BsonDocument>.Filter.Eq("postId", postId);
            var comment = new BsonDocument
            {
                { "postId", postId },
                { "userId", userId },
                { "content", content },
                { "createdAt", DateTime.UtcNow }
            };
            Posts.UpdateOne(postFilter, Builders<BsonDocument>.Update.Push("comments", comment));
        }

        public void AddLike(string postId, string userId)
        {
            var postFilter = Builders<BsonDocument>.Filter.Eq("postId", postId);
            var update = Builders<BsonDocument>.Update.Inc("reactions.like", 1);
            Posts.UpdateOne(postFilter, update);
        }
    }
}
