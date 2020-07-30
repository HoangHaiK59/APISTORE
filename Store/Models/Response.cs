using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Store.Models
{
    public class Response<T>: BaseResponse
    {
        public T data { get; set; }
    }

    public class BaseResponse
    {
        public string message { get; set; }
        public bool success { get; set; }
        public string error { get; set; }
    }
    public class BaseResponseWithToken
    {
        public string message { get; set; }
        public bool success { get; set; }
        public string error { get; set; }
        public string token { get; set; }
    }

    public class ResponseWrapper
    {
        private readonly RequestDelegate _next;

        public ResponseWrapper(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var currentBody = context.Response.Body;
            using(var memoStream = new MemoryStream())
            {
                context.Response.Body = memoStream;
                await _next(context);
                context.Response.Body = currentBody;
                memoStream.Seek(0, SeekOrigin.Begin);
                var readtoEnd = new StreamReader(memoStream).ReadToEnd();
                var objResult = JsonConvert.DeserializeObject(readtoEnd);
                var result = CommonApiResponse.Create((HttpStatusCode)context.Response.StatusCode, objResult, null);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
            }
        }
    }

    public static class ResponseWrapperExtensions
    {
        public static IApplicationBuilder UseResponseWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseWrapper>();
        }
    }

    public class CommonApiResponse
    {
        public static CommonApiResponse Create(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            return new CommonApiResponse(statusCode, result, errorMessage);
        }

        public string Version => "v1";

        public int StatusCode { get; set; }
        public string RequestId { get; }

        public string ErrorMessage { get; set; }

        public object Result { get; set; }

        protected CommonApiResponse(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            RequestId = Guid.NewGuid().ToString();
            StatusCode = (int)statusCode;
            Result = result;
            ErrorMessage = errorMessage;
        }
    }
}
