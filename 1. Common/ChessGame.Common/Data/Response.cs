using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace ChessGame.Common.Data;

public class Response
{
    public HttpStatusCode StatusCode { get; set; } = 0;
    public string? ErrorMessage { get; set; }

    public Response()
    {
        ErrorMessage = null;
    }
    public Response(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public virtual bool ResponseIsValid()
    {
        return ErrorMessage == null || ErrorMessage == string.Empty;
    }
}

public class Response<TData> : Response
{
    public TData? Data { get; set; }
    public new string? ErrorMessage
    {
        get => base.ErrorMessage;
        set => base.ErrorMessage = value;
    }

    public Response()
    {
        Data = default;
        ErrorMessage = null;
    }

    public Response(TData? data)
    {
        Data = data;
        ErrorMessage = null;
    }

    public Response(string errorMessage)
    {
        Data = default;
        ErrorMessage = errorMessage;
    }


    [MemberNotNullWhen(true, nameof(Data))]
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public override bool ResponseIsValid()
    {
        if (Data == null)
        {
            if (ErrorMessage == null)
                ErrorMessage = ChessGameCommonRes.Response_UnspecifiedError;

            return false;
        }

        return true;
    }
}
