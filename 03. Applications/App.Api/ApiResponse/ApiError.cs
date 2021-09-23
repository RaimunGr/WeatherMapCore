namespace App.Api.ApiResponse
{
    public sealed class ApiError
    {
        public ApiError(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
