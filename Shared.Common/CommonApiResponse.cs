namespace Shared.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = [];

        public static ApiResponse<T> Ok(
            T? data,
            string message = "Success",
            int statusCode = 200) =>
            new() { StatusCode = statusCode, Message = message, Data = data };

        public static ApiResponse<T> Fail(
            string message,
            int statusCode = 400,
            List<string>? errors = null) =>
            new() { StatusCode = statusCode, Message = message, Errors = errors ?? [] };
    }
}
