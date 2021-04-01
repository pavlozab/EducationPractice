using System;
using System.Collections.Generic;
using System.Linq;
using ProductRest.Models;

namespace ProductRest.Responses
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public long Count { get; set; }

        public PagedResponse(IEnumerable<T> data, long count)
        {
            Data = data;
            Count = count;
        }
    }
}