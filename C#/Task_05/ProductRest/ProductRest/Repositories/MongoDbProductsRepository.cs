using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductRest.Entities;
using ProductRest.Models;
using ProductRest.Repositories.Contracts;

namespace ProductRest.Repositories
{
    public class MongoDbProductsRepository : IProductsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "products";
        private readonly IMongoCollection<Product> _productsCollection;
        private readonly FilterDefinitionBuilder<Product> _filterDefinitionBuilder = Builders<Product>.Filter;
        
        public MongoDbProductsRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _productsCollection = database.GetCollection<Product>(collectionName); 
        }
        
        public async Task<Product> GetProductAsync(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Id, id);
            return await _productsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(QueryParametersModel filter)
        {
            var search2 = filter.Search is null 
                ? new BsonDocument()
                : _filterDefinitionBuilder.Where(obj => typeof(Product).GetProperties()
                    .Select(attr => typeof(Product).GetProperty(attr.Name).GetValue(obj, null))
                    .Any(temp => temp.ToString().ToLower().Contains(filter.Search.ToLower()))
                );
            // FIXME Search. Unsupported filter: Any(...) - search2
            
            var search = filter.Search is null 
                ? new BsonDocument()
                : _filterDefinitionBuilder.Where(obj
                    => obj.AddressLine.Contains(filter.Search)
                       || obj.PostalCode.Contains(filter.Search)
                       || obj.Country.Contains(filter.Search)
                       || obj.City.Contains(filter.Search)
                       || obj.FaxNumber.Contains(filter.Search)
                       || obj.PhoneNumber.Contains(filter.Search)
                );
            

            var sort = filter.SortType == "asc"
                ? Builders<Product>.Sort.Ascending(filter.SortBy)
                : Builders<Product>.Sort.Descending(filter.SortBy);

            return await _productsCollection.Find(search)
                .Sort(sort).Skip((filter.Offset) * filter.Limit)
                .Limit(filter.Limit).ToListAsync();
        }

        public async Task CreateProductAsync(Product item)
        {
            await _productsCollection.InsertOneAsync(item);
        }

        public async Task UpdateProductAsync(Product item)
        {
            var filter = _filterDefinitionBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await _productsCollection.ReplaceOneAsync(filter, item);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(item => item.Id, id);
            await _productsCollection.DeleteOneAsync(filter);
        }

        public async Task<long> Count()
        {
            return await _productsCollection.CountDocumentsAsync(new BsonDocument());
        }
    }
}