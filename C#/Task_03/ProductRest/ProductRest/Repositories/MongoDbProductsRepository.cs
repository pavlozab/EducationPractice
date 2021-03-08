using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductRest.Dtos;
using ProductRest.Models;

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
        
        public async Task<IEnumerable<ProductDto>> GetProductsAsync(QueryParametersModel filter)
        {
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
            // FIXME Search 
            

            var sort = filter.SortType == "asc"
                ? Builders<ProductDto>.Sort.Ascending(filter.SortBy)
                : Builders<ProductDto>.Sort.Descending(filter.SortBy);

            return await productsCollection.Find(search)
                .Sort(sort).Skip((filter.Offset - 1) * filter.Limit)
                .Limit(filter.Limit).ToListAsync();
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

        public async Task<long> Count()
        {
            return await productsCollection.CountDocumentsAsync(new BsonDocument());
        }
    }
}