using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ApiResponse
    {
        public ApiResponse() { }


        public HttpStatusCode StatusCode { get; set; }
        public object Result { get; set; }
        public bool IsError { get; set; }=false;
        public int Count { get; set; }
        public int PageNumber { get; set; }
    }
}
