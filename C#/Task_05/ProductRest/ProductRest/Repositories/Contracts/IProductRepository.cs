using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Entities;
using ProductRest.Models;

namespace ProductRest.Repositories.Contracts
{
    public interface IProductsRepository: IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll(QueryParametersModel filter);
        Task Update(Product item);
        Task Delete(Guid id);
        Task<long> Count();
    }
}