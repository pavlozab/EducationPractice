using System;
using System.Collections.Generic;
using System.Linq;
using ProductRest.Dtos;
using ProductRest.Models;

namespace ProductRest.Responses
{
    public class PagedResponse<T>
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public long Count { get; set; }
        public string SortBy { get; set; }
        public string SortType { get; set; }
        public string Search { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PagedResponse(IEnumerable<T> data, QueryParametersModel parameters, long count)
        {
            Offset = parameters.Offset;
            Limit = parameters.Limit;
            SortBy = parameters.SortBy;
            SortType = parameters.SortType;
            Search = parameters.Search;
            Data = data;
            Count = count;
        }
    }
}