using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LondonApi.Filters
{
    public class JsonExceptionFilter : IExceptionFilter
    {

        private readonly IHostingEnvironment _env;
        public JsonExceptionFilter(IHostingEnvironment env)
        {
            // injecting IHostingEnvironment to the constructor 
            _env = env;

        }
        public void OnException(ExceptionContext contex)
        {
            //Called when an exception occurs. We want to serialize the exception and send it back to user
            var error = new Models.ApiError();

            if (_env.IsDevelopment())
            {
                error.Message = contex.Exception.Message;
                error.Detail = contex.Exception.StackTrace;
            }
            else
            {
                error.Message = "A server error occured";
                error.Detail = contex.Exception.Message;
            }

            // ObjectResult Implemenets IActionResult
            contex.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };
                    }
    }
}
