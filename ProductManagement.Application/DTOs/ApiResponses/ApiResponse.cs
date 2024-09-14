using System.Text.Json.Serialization;

namespace ProductManagement.Core.DTOs.ApiResponses;

public class ApiResponse<T> where T : class
{
    /// <summary>
    ///     Data is the object that will be returned to the client.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    ///     StatusCode is the status code that will be returned to the client.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    ///     Error is the error message that will be returned to the client.
    /// </summary>
    public ErrorDto? Error { get; set; }

    /// <summary>
    ///     Check the status code to see if the request was successful during development. NOT RETURNED TO THE CLIENT.
    /// </summary>
    [JsonIgnore]
    public bool IsSuccessful => StatusCode >= 200 && StatusCode < 300;

    /// <summary>
    ///     Creates an instance of ApiResponse with the given data and status code.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public static ApiResponse<T> Success(T data, int statusCode)
    {
        return new ApiResponse<T>() { Data = data, StatusCode = statusCode };
    }

    /// <summary>
    ///     Creates an instance of ApiResponse with the given status code. USE WHEN THERE IS NO DATA TO RETURN.
    /// </summary>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public static ApiResponse<T> Success(int statusCode)
    {
        return new ApiResponse<T>() { Data = null, StatusCode = statusCode };
    }

    /// <summary>
    ///     Creates an instance of ApiResponse with the given error message and status code.
    /// </summary>
    /// <param name="errorDto"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public static ApiResponse<T> Fail(ErrorDto errorDto, int statusCode)
    {
        return new ApiResponse<T>() { Error = errorDto, StatusCode = statusCode };
    }

    /// <summary>
    ///     Creates an instance of ApiResponse with the given error message and status code.
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <param name="statusCode"></param>
    /// <param name="isShow"></param>
    /// <returns></returns>
    public static ApiResponse<T> Fail(string errorMessage, int statusCode, bool isShow = true)
    {
        ErrorDto errorDto = new(errorMessage, isShow);
        return new ApiResponse<T> { Error = errorDto, StatusCode = statusCode };
    }
}