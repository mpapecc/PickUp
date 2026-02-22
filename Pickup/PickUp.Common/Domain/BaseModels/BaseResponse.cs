namespace PickUp.Common.Domain.BaseModels
{
    public interface IBaseResponse
    {
        public bool IsSuccessful { get; set; }
    }

    public class BaseResponse<T> : IBaseResponse
    {
        public BaseResponse()
        {
        }
        public BaseResponse(bool isSuccessful, T data)
        {
            IsSuccessful = isSuccessful;
            Data = data;
        }

        public BaseResponse(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }

        public BaseResponse(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }

        public BaseResponse(bool isSuccessful, string message, T data)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            Data = data;
        }

        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public  static BaseResponse<T> CreateSuccess(T data) => new BaseResponse<T>(true, data);
        public  static BaseResponse<T> CreateSuccess(T data, string message) => new BaseResponse<T>(true, message, data);
        public static BaseResponse<T> CreateFail(T data) => new BaseResponse<T>(false, data);
        public static BaseResponse<T> CreateFail(T data, string message) => new BaseResponse<T>(false, message, data);
        public static BaseResponse<T> CreateFail(string message) => new BaseResponse<T>(false, message);
    }

    public class BaseResponse : BaseResponse<string>
    {
        public BaseResponse(bool isSuccessful, string data) : base(isSuccessful, data)
        {
        }

        public new static BaseResponse CreateSuccess(string data) => new BaseResponse(true, data);
        public new static BaseResponse CreateFail(string data) => new BaseResponse(false, data);
    }
}
