using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.Api.ApiResponse
{
    public static partial class ApiActionResult
    {
        public static IActionResult ToActionResult(this object obj, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var apiResult = ApiResult.From(obj);
            var result = new ObjectResult(apiResult)
            {
                StatusCode = (int)statusCode
            };

            return result;
        }
    }
}
