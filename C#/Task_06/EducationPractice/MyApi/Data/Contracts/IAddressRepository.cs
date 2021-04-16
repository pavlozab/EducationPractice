using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Data
{
    public interface IAddressRepository : IBaseRepository<Address>
    {
        Task<IEnumerable<Address>> GetAll(); //GetAll(QueryParametersModel filter);
        Task Update(Address item);
        Task<long> Count();
    }
}