using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProductRest.Data.Contracts;
using ProductRest.Entities;

namespace ProductRest.Data.Repositories
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
        
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Email, email);
            return await _usersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        // public async Task DeleteUserAsync(Guid id)
        // {
        //     var filter = _filterDefinitionBuilder.Eq(item => item.Id, id);
        //     await _usersCollection.DeleteOneAsync(filter);
        // }
        //
        // public async Task UpdateUserAsync(Guid id, User user)
        // {
        //     var filter = _filterDefinitionBuilder.Eq(existingItem => existingItem.Id, user.Id);
        //     await _usersCollection.ReplaceOneAsync(filter, user);
        // }
    }
}