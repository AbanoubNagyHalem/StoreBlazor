using System.Net.Http.Headers;
using System.Net.Http.Json;
using StoreBlazor.Models;

namespace StoreBlazor.Services;

public class ProductApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthStateService _authState;

    public ProductApiClient(
        IHttpClientFactory httpClientFactory,
        AuthStateService authState)
    {
        _httpClientFactory = httpClientFactory;
        _authState = authState;
    }

    public async Task<PagedResponse<ProductResponse>?> GetAllAsync(
        ProductQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient("StoreApi");

        string url =
            $"api/products" +
            $"?search={Uri.EscapeDataString(parameters.Search ?? "")}" +
            $"&page={parameters.Page}" +
            $"&pageSize={parameters.PageSize}" +
            $"&sortBy={parameters.SortBy ?? ""}" +
            $"&sortDirection={parameters.SortDirection}";

        if (parameters.CategoryId.HasValue)
        {
            url +=
                $"&categoryId={parameters.CategoryId.Value}";
        }

        return await httpClient
            .GetFromJsonAsync<PagedResponse<ProductResponse>>(
                url,
                cancellationToken);
    }

    public async Task<ProductResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient("StoreApi");

        HttpResponseMessage response =
            await httpClient.GetAsync(
                $"api/products/{id}",
                cancellationToken);

        if (response.StatusCode ==
            System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<ProductResponse>(
                cancellationToken);
    }

    public async Task<HttpResponseMessage> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient("StoreApi");

        using HttpRequestMessage httpRequest =
            new(
                HttpMethod.Post,
                "api/products");

        httpRequest.Content =
            JsonContent.Create(request);

        AddAuthorizationHeader(httpRequest);

        return await httpClient.SendAsync(
            httpRequest,
            cancellationToken);
    }

    public async Task<HttpResponseMessage> UpdateAsync(
        int id,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient("StoreApi");

        using HttpRequestMessage httpRequest =
            new(
                HttpMethod.Put,
                $"api/products/{id}");

        httpRequest.Content =
            JsonContent.Create(request);

        AddAuthorizationHeader(httpRequest);

        return await httpClient.SendAsync(
            httpRequest,
            cancellationToken);
    }

    public async Task<HttpResponseMessage> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient("StoreApi");

        using HttpRequestMessage httpRequest =
            new(
                HttpMethod.Delete,
                $"api/products/{id}");

        AddAuthorizationHeader(httpRequest);

        return await httpClient.SendAsync(
            httpRequest,
            cancellationToken);
    }

    private void AddAuthorizationHeader(
        HttpRequestMessage request)
    {
        if (string.IsNullOrWhiteSpace(
                _authState.Token))
        {
            return;
        }

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                _authState.Token);
    }
}