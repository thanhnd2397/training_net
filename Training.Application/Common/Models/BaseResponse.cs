namespace Training.Application.Common.Models;

public class BaseResponse<T>
{
    public string Code { get; set; } = "SUCCESS";
    public string Message { get; set; } = string.Empty;
    public DateTime Time { get; set; } = DateTime.UtcNow;
    public T? Data { get; set; }

    public static BaseResponse<T> Success(T data, string message = "")
    {
        return new BaseResponse<T>
        {
            Code = "SUCCESS",
            Message = message,
            Data = data
        };
    }

    public static BaseResponse<T> Fail(string code, string message)
    {
        return new BaseResponse<T>
        {
            Code = code,
            Message = message
        };
    }
}