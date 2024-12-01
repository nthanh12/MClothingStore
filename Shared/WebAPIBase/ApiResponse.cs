using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.WebAPIBase
{
    public class ApiResponse
    {
        public StatusCode Status { get; set; }
        public object? Data { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public ApiResponse(StatusCode statusCode, object? data, int code, string message)
        {
            Status = statusCode;
            Data = data;
            Code = code;
            Message = message;
        }
        public ApiResponse(object? data)
        {
            Status = StatusCode.Success;
            Data = data;
            Code = 200;
            Message = "Ok";
        }
        public ApiResponse()
        {
            Status = StatusCode.Success;
            Data = null;
            Code = 200;
            Message = "Ok";
        }
    }
    public enum StatusCode
    {
        Success = 1,
        Error = 0
    }

    public class ApiResponse<T> : ApiResponse
    {
        public new T Data { get; set; }
        public ApiResponse(StatusCode statusCode, T data, int code, string message) : base(statusCode, data, code, message)
        {
            Status = statusCode;
            Data = data;
            Code = code;
            Message = message;
        }
        public ApiResponse(T data) : base()
        {
            Data = data;
        }
    }
}
