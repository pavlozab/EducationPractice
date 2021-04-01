using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductRest.Dto;
using ProductRest.Entities;
using ProductRest.Repositories.Contracts;

namespace ProductRest.Repositories
{
    public class MongoDbUserRepository : IUserRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "users";
        private readonly IMongoCollection<User> _usersCollection;
        private readonly FilterDefinitionBuilder<User> _filterDefinitionBuilder = Builders<User>.Filter;
        
        public MongoDbUserRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _usersCollection = database.GetCollection<User>(collectionName); 
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var filter = new BsonDocument();
            return await _usersCollection.Find(filter).ToListAsync();
        }
        
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Email, email);
            return await _usersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Id, id);
            return await _usersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task UpdateUser(Guid id, User user)
        {
            var filter = _filterDefinitionBuilder.Eq(existingItem => existingItem.Id, user.Id);
            await _usersCollection.ReplaceOneAsync(filter, user);
        }

        public async Task DeleteUser(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Id, id);
            await _usersCollection.DeleteOneAsync(filter);
        }
        
    }
}