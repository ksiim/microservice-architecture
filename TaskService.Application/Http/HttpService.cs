using System.Text;
using System.Text.Json;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace TaskService.Application.Http;

public interface IHttpService
{
    Task<T> GetAsync<T>(string url, Dictionary<string, string>? headers = null);
    Task<T> PostAsync<T>(string url, object data, Dictionary<string, string>? headers = null);
    Task<T> PutAsync<T>(string url, object data, Dictionary<string, string>? headers = null);
    Task<bool> DeleteAsync(string url, Dictionary<string, string>? headers = null);
}

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<HttpService> _logger;

    public HttpService(HttpClient httpClient, ILogger<HttpService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<T> GetAsync<T>(string url, Dictionary<string, string>? headers = null)
    {
        var request = CreateHttpRequest(HttpMethod.Get, url, headers);
        return await SendRequestAsync<T>(request);
    }

    public async Task<T> PostAsync<T>(string url, object data, Dictionary<string, string>? headers = null)
    {
        var request = CreateHttpRequest(HttpMethod.Post, url, headers);
        request.Content = CreateJsonContent(data);
        return await SendRequestAsync<T>(request);
    }

    public async Task<T> PutAsync<T>(string url, object data, Dictionary<string, string>? headers = null)
    {
        var request = CreateHttpRequest(HttpMethod.Put, url, headers);
        request.Content = CreateJsonContent(data);
        return await SendRequestAsync<T>(request);
    }

    public async Task<bool> DeleteAsync(string url, Dictionary<string, string>? headers = null)
    {
        var request = CreateHttpRequest(HttpMethod.Delete, url, headers);
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    private HttpRequestMessage CreateHttpRequest(HttpMethod method, string url, Dictionary<string, string>? headers)
    {
        var request = new HttpRequestMessage(method, url);
        
        request.Headers.Add("X-Trace-Id", TraceId.Current);
        
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        return request;
    }

    private StringContent CreateJsonContent(object data)
    {
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private async Task<T> SendRequestAsync<T>(HttpRequestMessage request)
    {
        _logger?.LogInformation("Sending HTTP {Method} {Url} TraceId={TraceId}", request.Method, request.RequestUri, TraceId.Current);
        var response = await _httpClient.SendAsync(request);
        _logger?.LogInformation("Received HTTP {StatusCode} for {Url} TraceId={TraceId}", response.StatusCode, request.RequestUri, TraceId.Current);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}. Response body: {errorBody}");
        }

        var content = await response.Content.ReadAsStringAsync();
        
        if (string.IsNullOrEmpty(content))
        {
            return default(T)!;
        }

        return JsonSerializer.Deserialize<T>(content, _jsonOptions) ?? 
               throw new InvalidOperationException("Failed to deserialize response");
    }
}