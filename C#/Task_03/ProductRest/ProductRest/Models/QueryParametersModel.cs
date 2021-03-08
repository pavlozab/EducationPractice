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
            Offset = 1;
            Limit = 10;
        }
        
        public QueryParametersModel(string sortby, string sorttype, int offset, int limit, string search)
        {
            Search = search;
            SortBy = typeof(ProductDto).GetProperties()
                .Select(obj => obj.Name)
                .Contains(sortby) ? sortby : "AddressLine";
            
            SortType = sorttype == "desc" ? "desc" : "asc";
            Offset = offset < 1 ? 1 : offset;
            Limit = limit > 10 ? 10 : limit;
        }
        
        public QueryParametersModel(QueryParametersModel filter)
        {
            Search = filter.Search;
            SortBy = typeof(ProductDto).GetProperties()
                .Select(obj => obj.Name)
                .Contains(filter.SortBy) ? filter.SortBy : "AddressLine";
            
            SortType = filter.SortType == "desc" ? "desc" : "asc";
            Offset = filter.Offset < 1 ? 1 : filter.Offset;
            Limit = filter.Limit > 10 ? 10 : filter.Limit;
        }
    }
}