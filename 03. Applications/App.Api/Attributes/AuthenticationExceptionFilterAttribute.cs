using App.Api.ApiResponse;
using App.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace App.Api.Attributes
{
    public class AuthenticationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public int StatusCode { get; set; }

        public override void OnException(ExceptionContext context)
        {

            var message = GetTheMostInternalExceptionMessage(context.Exception);

            var apiError = new ApiError(message);
            var apiResult = ApiResult.From(apiError);

            context.Result = new JsonResult(apiResult);
            context.HttpContext.Response.StatusCode =
                StatusCode != default ?
                StatusCode :
                (int)StatusCodeFinder.Find(context.Exception);

            base.OnException(context);
        }

        private string GetTheMostInternalExceptionMessage(Exception exception)
        {
            if (exception.InnerException is null)
            {
                return exception.Message;
            }

            return GetTheMostInternalExceptionMessage(exception.InnerException);
        }
    }
}
