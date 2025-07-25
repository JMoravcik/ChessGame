using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace ChessGame.Backend.Common.Extensions;

public static class HttpContextExtensions
{
    public static bool HasAttribute<TAttribute>(this HttpContext context, [NotNullWhen(true)] out TAttribute? attribute) where TAttribute : Attribute
    {
        attribute = null;
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        var endpoint = context.GetEndpoint();
        if (endpoint == null)
            return false;

        attribute = endpoint.Metadata.GetMetadata<TAttribute>();
        return attribute != null;
    }
}
