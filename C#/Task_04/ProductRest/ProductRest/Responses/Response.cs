using System.Collections.Generic;

namespace ProductRest.Responses
{
    public class Response<T>
    {
        public Response() { }

        public Response(IEnumerable<T> data)
        {
            Data = data;
        }
        public IEnumerable<T> Data { get; set; }
    }
}