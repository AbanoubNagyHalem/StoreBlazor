using System.Net.Http.Headers;
using System.Net.Http.Json;
using StoreBlazor.Models;

namespace StoreBlazor.Services;

public class ProductApiClient
{
    private readonly IHttpClientFactory
        _httpClientFactory;

    private readonly AuthStateService
        _authState;


    public ProductApiClient(
        IHttpClientFactory httpClientFactory,
        AuthStateService authState)
    {
        _httpClientFactory =
            httpClientFactory;

        _authState =
            authState;
    }


    public async Task<
        PagedResponse<ProductResponse>?>
        GetAllAsync(
            ProductQueryParameters parameters,
            CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient(
                "StoreApi");


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
            .GetFromJsonAsync<
                PagedResponse<ProductResponse>>(
                url,
                cancellationToken);
    }


    public async Task<HttpResponseMessage>
        CreateAsync(
            CreateProductRequest request,
            CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient(
                "StoreApi");


        using HttpRequestMessage httpRequest =
            new(
                HttpMethod.Post,
                "api/products");


        httpRequest.Content =
            JsonContent.Create(request);


        Console.WriteLine(
            $"Create Product Token Exists: {!string.IsNullOrWhiteSpace(_authState.Token)}");

        Console.WriteLine(
            $"Create Product Role: {_authState.Role}");


        if (!string.IsNullOrWhiteSpace(
                _authState.Token))
        {
            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _authState.Token);
        }


        return await httpClient.SendAsync(
            httpRequest,
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

        if (!string.IsNullOrWhiteSpace(
                _authState.Token))
        {
            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _authState.Token);
        }

        return await httpClient.SendAsync(
            httpRequest,
            cancellationToken);
    }
}