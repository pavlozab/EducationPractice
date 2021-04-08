using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductRest.Entities;
using ProductRest.Repositories.Contracts;

namespace ProductRest.Repositories
{
    public class MongoDbUserRepository : IUserRepository
    {
        private const string DatabaseName = "catalog";
        private const string CollectionName = "users";
        private readonly IMongoCollection<User> _usersCollection;
        private readonly FilterDefinitionBuilder<User> _filterDefinitionBuilder = Builders<User>.Filter;
        
        public MongoDbUserRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(DatabaseName);
            _usersCollection = database.GetCollection<User>(CollectionName); 
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var filter = new BsonDocument();
            return await _usersCollection.Find(filter).ToListAsync();
        }
        
        public async Task<User> GetUserByEmail(string email)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Email, email);
            return await _usersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<User> Get(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Id, id);
            return await _usersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task Create(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task Update(Guid id, User user)
        {
            var filter = _filterDefinitionBuilder.Eq(existingItem => existingItem.Id, user.Id);
            await _usersCollection.ReplaceOneAsync(filter, user);
        }

        public async Task Delete(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Id, id);
            await _usersCollection.DeleteOneAsync(filter);
        }
    }
}