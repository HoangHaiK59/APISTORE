using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool status { get; set; }
    }
    public class BaseResponseWithToken
    {
        public string message { get; set; }
        public bool status { get; set; }

        public string token { get; set; }
    }

}
