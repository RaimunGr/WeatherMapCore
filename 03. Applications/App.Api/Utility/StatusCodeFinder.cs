using System;
using System.Net;

namespace App.Api.Utility
{
    public static class StatusCodeFinder
    {
        public static HttpStatusCode Find(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                AccessViolationException => HttpStatusCode.Forbidden,
                InvalidOperationException when exception.Message.Contains("no elements") => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError,
            };
        }
    }
}
