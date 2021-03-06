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

        public QueryParametersModel()
        {
            SortBy = "AddressLine";
            SortType = "asc";
            Offset = 1;
            Limit = 10;
        }
        
        public QueryParametersModel(string sortby, string sorttype, int offset, int limit)
        {
            SortBy = typeof(ProductDto).GetProperties()
                .Select(obj => obj.Name)
                .Contains(sortby) ? sortby : "AddressLine";
            
            SortType = sorttype == "desc" ? "desc" : "asc";
            Offset = offset < 1 ? 1 : offset;
            Limit = limit > 10 ? 10 : limit;
        }
    }
}