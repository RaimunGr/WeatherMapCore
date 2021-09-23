namespace App.Api.ApiResponse
{
    public sealed class ApiResult
    {
        private ApiResult()
        { }

        public object Result { get; private set; }

        public static ApiResult From(object obj)
        {
            return new ApiResult
            {
                Result = obj
            };
        }
    }
}
