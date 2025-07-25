using System.Net;

namespace ChessGame.Common.Exceptions;

public class HttpException : Exception
{
    public HttpException(string message, HttpStatusCode statusCode) 
        : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}
