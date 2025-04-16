namespace Core.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> Ok(T data) => new() { Data = data };
        public static ApiResponse<T> Fail(string error) => new() { Success = false, Errors = new List<string> { error } };
        public static ApiResponse<T> Fail(List<string> errors) => new() { Success = false, Errors = errors };
    }
}
