using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public int? Take { get; set; }
        public int? Offset { get; set; }
        public int? Total { get; set; }

        public ApiResponse(bool success, string message, T? data = default, int? take = null, int? offset = null, int? total = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Take = take;
            Offset = offset;
            Total = total;
        }

        public static ApiResponse<T> Ok(T? data, string message = "Operation successfully completed.", int? take = null, int? offset = null, int? total = null)
            => new(true, message, data, take, offset, total);

        public static ApiResponse<T> Fail(string message)
            => new(false, message);
    }
}