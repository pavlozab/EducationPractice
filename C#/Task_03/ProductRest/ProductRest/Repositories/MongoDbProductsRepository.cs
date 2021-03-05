using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductRest.Dtos;

namespace ProductRest.Repositories
{
    public class MongoDbProductsRepository : IProductsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "products";
        private readonly IMongoCollection<ProductDto> productsCollection;
        private readonly FilterDefinitionBuilder<ProductDto> _filterDefinitionBuilder = Builders<ProductDto>.Filter;
        
        public MongoDbProductsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            productsCollection = database.GetCollection<ProductDto>(collectionName); 
        }
        
        public async Task<ProductDto> GetProductAsync(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Id, id);
            return await productsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            return await productsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task CreateProductAsync(ProductDto item)
        {
            await productsCollection.InsertOneAsync(item);
        }

        public async Task UpdateProductAsync(ProductDto item)
        {
            var filter = _filterDefinitionBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await productsCollection.ReplaceOneAsync(filter, item);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Id, id);
            await productsCollection.DeleteOneAsync(filter);
        }
    }
}