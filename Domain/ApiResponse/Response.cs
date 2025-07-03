using System.Net;

namespace Domain.ApiResponse;

public class Response<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public string Massege { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public Response(string massege, HttpStatusCode statusCode)
    {
        IsSuccess = false;
        Data = default!;
        Massege = massege;
        StatusCode = statusCode;
    }

    public Response(T data, string massege)
    {
        IsSuccess = true;
        Data = data;
        Massege = massege;
        StatusCode = HttpStatusCode.OK;
    }
}