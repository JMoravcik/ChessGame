using ChessGame.Common.Data;
using ChessGame.Common.Exceptions;
using ChessGame.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ChessGame.Common.Bases;

public abstract class Linker
{
    private readonly LinkerContext _linkerContext;
    private readonly ILogger _logger;
    private string _serverBaseUrl;
    private readonly string _controllerBasePath;
    private readonly int _retryCount;
    public Linker(LinkerContext linkerContext, IConfiguration configuration, ILogger logger, string configurationSection, string controllerBasePath, bool baseUrlNeeded = false)
    {
        _linkerContext = linkerContext;
        _logger = logger;
        _serverBaseUrl = configuration.GetSection($"{configurationSection}:BaseUrl").Get<string>() ?? string.Empty;

        if (string.IsNullOrEmpty(_serverBaseUrl) && baseUrlNeeded)
            throw new Exception($"Base URL for {configurationSection} is not configured!");

        _retryCount = configuration.GetSection($"{configurationSection}:RetryCount")?.Get<int?>() ?? 5;
        _serverBaseUrl = _serverBaseUrl.StandartizeUrl();
        _controllerBasePath = controllerBasePath.StandartizeUrl();
    }

    public void SetServerBaseUrl(string newServerBaseUrl)
    {
        _serverBaseUrl = newServerBaseUrl.StandartizeUrl();
    }

    private async Task<HttpResponseMessage> SendAsync(string relativeUrl, HttpMethod httpMethod, HttpContent? content = null)
    {
        using var httpClient = new HttpClient();

        if (await _linkerContext.GetDeviceTokenAsync() is string deviceToken)
            httpClient.DefaultRequestHeaders.SetDeviceToken(deviceToken);

        if (await _linkerContext.GetAuthTokenAsync() is string authToken)
            httpClient.DefaultRequestHeaders.SetAuthToken(authToken);

        HttpResponseMessage? response = null;
        Exception? lastException = null;

        for (int i = 0; i < _retryCount; i++)
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Content = content,
                    RequestUri = new Uri($"{_serverBaseUrl}/{_controllerBasePath}/{relativeUrl}"),
                    Method = httpMethod,
                };

                response = await httpClient.SendAsync(httpRequestMessage);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error while sending request to {relativeUrl}. Attempt {i + 1} of {_retryCount}.", ex);
                lastException = ex;
                await Task.Delay(3000);
            }
        }

        _logger.LogInformation($"Request '{relativeUrl}' response code: {response?.StatusCode}");
        return response ?? throw new Exception("Communication couldn't be established!", lastException);
    }

    private async Task<TResponse> SendRequestAsync<TResponse>(string relativeUrl, HttpMethod httpMethod, HttpContent? content = null)
                        where TResponse : Response, new()
    {

        HttpResponseMessage response = await SendAsync(relativeUrl, httpMethod, content);
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = new TResponse
            {
                StatusCode = response.StatusCode,
                ErrorMessage = response.ReasonPhrase
            };
            return errorResponse;
        }
        var responseContent = (await response.Content.ReadFromJsonAsync<TResponse>())!;
        return responseContent;
    }

    protected Task<Response> PostAsync<TRequest>(string relativeUrl, TRequest request)
        => SendRequestAsync<Response>(relativeUrl, HttpMethod.Post, JsonContent.Create(request));

    protected Task<Response<TResponse>> PostAsync<TResponse, TRequest>(string relativeUrl, TRequest request)
        => SendRequestAsync<Response<TResponse>>(relativeUrl, HttpMethod.Post, JsonContent.Create(request));

    protected Task<Response<TResponse>> GetAsync<TResponse>(string relativeUrl)
        => SendRequestAsync<Response<TResponse>>(relativeUrl, HttpMethod.Get);

    protected Task<Response> GetAsync(string relativeUrl)
        => SendRequestAsync<Response>(relativeUrl, HttpMethod.Get);

    protected async Task<Stream> DownloadAsync(string relativeUrl)
    {
        var response = await SendAsync(relativeUrl, HttpMethod.Get);
        return await response.Content.ReadAsStreamAsync();
    }

    protected Task<Response> UploadAsync(string relativeUrl, Stream stream)
    {
        return SendRequestAsync<Response>(relativeUrl, HttpMethod.Post, new StreamContent(stream));
    }
}
