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

        public ApiResponse(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Ok(T data, string message = "Operação realizada com sucesso.")
            => new(true, message, data);

        public static ApiResponse<T> Fail(string message)
            => new(false, message);
    }
}