using System.Linq;
using Entities;

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
            SortBy = typeof(Address).GetProperties()
                .Select(obj => obj.Name)
                .Contains(filter.SortBy) ? filter.SortBy : "AddressLine";
            
            SortType = filter.SortType == "desc" ? "desc" : "asc";
            Offset = filter.Offset < 0 ? 0 : filter.Offset;
            Limit = filter.Limit > 10 ? 10 : filter.Limit;
        }
    }
}