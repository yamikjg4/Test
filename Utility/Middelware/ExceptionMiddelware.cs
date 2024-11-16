using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Utility.Middelware
{
    public class ExceptionMiddelware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddelware(RequestDelegate next) { _next = next; }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = new { Status = 500, ErrorMessage = ex.Message,IsError=true };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
