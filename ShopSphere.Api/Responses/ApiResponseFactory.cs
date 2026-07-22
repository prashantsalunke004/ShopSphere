namespace ShopSphere.API.Responses
{
    public class ApiResponseFactory
    {
        public static ApiResponse<T> Success<T>(T data, string message)
        {
            return new ApiResponse<T>(true, message, data);
        }

        public static ApiResponse<object> Success(string message)
        {
            return new ApiResponse<object>(true, message, null);
        }

        public static ApiResponse<object> Fail(string message)
        {
            return new ApiResponse<object>(false, message, null);
        }

        public static ApiResponse<T> Fail<T>(string message)
        {
            return new ApiResponse<T>(false, message, default);
        }
    }
}
