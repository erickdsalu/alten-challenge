using System;
using System.Net;

namespace Extensions.Exceptions
{
    public class CustomNotificationException : Exception
    {
        public CustomNotificationException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
    }
}
