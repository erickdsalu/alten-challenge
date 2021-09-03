using Extensions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Web.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(CustomNotificationException))
            {
                var statusCode = ((CustomNotificationException)context.Exception).StatusCode;
                context.Result = new ContentResult
                {
                    StatusCode = (int)statusCode,
                    Content = JsonConvert.SerializeObject(new { statusCode = statusCode.ToString(), message = context.Exception.Message }),
                    ContentType = "application/json"
                };
            }
        }
    }
}
