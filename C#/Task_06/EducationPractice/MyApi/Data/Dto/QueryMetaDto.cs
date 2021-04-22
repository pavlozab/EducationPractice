using System.Linq;
using Entities;
using Microsoft.Extensions.Options;

namespace Data.Dto
{
    public class QueryMetaDto
    {
        public string SortBy { get; set; }
        public string SortType { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string Search { get; set; }

        public QueryMetaDto()
        {
            Search = null;
            SortBy = "AddressLine";
            SortType = "asc";
            Offset = 0;
            Limit = 10;
        }
        
        public QueryMetaDto(QueryMetaDto filter)
        {
            Search = filter.Search;
            Validate();
        }
        
        public void Validate()
        {
            SortBy = typeof(Address).GetProperties()
                .Select(obj => obj.Name)
                .Contains(SortBy) ? SortBy : "AddressLine";
            
            SortType = SortType == "desc" ? "desc" : "asc";
            Offset = Offset < 0 ? 0 : Offset;
            Limit = Limit > 10 ? 10 : Limit;
        }
    }
}