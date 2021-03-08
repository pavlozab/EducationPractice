using System.Collections.Generic;

namespace ProductRest.Responses
{
    public class Response<T>
    {
        public Response() { }

        public Response(IEnumerable<T> data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public IEnumerable<T> Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}