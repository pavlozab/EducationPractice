using System;
using System.Collections.Generic;
using System.Linq;
using ProductRest.Dtos;

namespace ProductRest.Responses
{
    public class PagedResponse<T> : Response<T>
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public long Count { get; set; }

        public PagedResponse(IEnumerable<T> data, int offset, int limit, long count)
        {
            Offset = offset;
            Limit = limit;
            Data = data;
            Count = count;
            Message = null;
            Succeeded = true;
            Errors = null;
        }
    }
}