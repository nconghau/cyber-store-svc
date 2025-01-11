namespace DotnetApiPostgres.Api.Models.Common
{
    public class JsonResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public JsonResponse(bool success, T data, string? message=null)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public JsonResponse()
        {
          
        }
    }
}

