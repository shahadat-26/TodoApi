using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using TodoApi.CustomException;
namespace TodoApi.CustomMiddleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ItemFoundOrNot
    {
        private readonly RequestDelegate _next;
       
        public ItemFoundOrNot(RequestDelegate next)
        {
            
            _next = next;
           
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(ItemNotFoundException im)
            {
                var data = new
                {
                    Message = im.Message,
                    StackTrace = im.StackTrace
                };

                var json = JsonSerializer.Serialize(data);



                httpContext.Response.StatusCode = 404;
                httpContext.Response.Headers.Add("Exception",json);
                await httpContext.Response.WriteAsync("Wrote The Middleware Successfully");
                
            }

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ItemFoundOrNotExtensions
    {
        public static IApplicationBuilder UseItemFoundOrNot(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ItemFoundOrNot>();
        }
    }
}
