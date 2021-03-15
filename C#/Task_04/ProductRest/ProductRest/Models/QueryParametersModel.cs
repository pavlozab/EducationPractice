using System;
using System.Linq;
using ProductRest.Dtos;

namespace ProductRest.Models
{
    public class QueryParametersModel
    {
        public string SortBy { get; set; }
        public string SortType { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string Search { get; set; }

        public QueryParametersModel()
        {
            Search = null;
            SortBy = "AddressLine";
            SortType = "asc";
            Offset = 0;
            Limit = 10;
        }
        
        public QueryParametersModel(QueryParametersModel filter)
        {
            Search = filter.Search;
            SortBy = typeof(ProductDto).GetProperties()
                .Select(obj => obj.Name)
                .Contains(filter.SortBy) ? filter.SortBy : "AddressLine";
            
            SortType = filter.SortType == "desc" ? "desc" : "asc";
            Offset = filter.Offset < 0 ? 0 : filter.Offset;
            Limit = filter.Limit > 10 ? 10 : filter.Limit;
        }
    }
}