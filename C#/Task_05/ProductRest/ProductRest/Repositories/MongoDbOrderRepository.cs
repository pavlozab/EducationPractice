using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProductRest.Entities;
using ProductRest.Repositories.Contracts;

namespace ProductRest.Repositories
{
    public class MongoDbOrderRepository: IOrderRepository
    {
        private const string DatabaseName = "catalog";
        private const string CollectionName = "orders";
        private readonly IMongoCollection<Order> _ordersCollection;
        private readonly FilterDefinitionBuilder<Order> _filterDefinitionBuilder = Builders<Order>.Filter;
        
        public MongoDbOrderRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(DatabaseName);
            _ordersCollection = database.GetCollection<Order>(CollectionName); 
        }
        public async Task<IEnumerable<Order>> GetAll(Guid userId)
        {
            var filter = _filterDefinitionBuilder.Where(obj =>
                obj.UserId == userId);
            return await _ordersCollection.Find(filter).ToListAsync();
        }

        public async Task<Order> Get(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(obj => obj.Id, id);
            return await _ordersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task Create(Order order)
        {
            await _ordersCollection.InsertOneAsync(order);
        }
    }
}