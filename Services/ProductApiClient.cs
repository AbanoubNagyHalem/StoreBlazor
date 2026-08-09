using System.Net.Http.Json;
using StoreBlazor.Models;

namespace StoreBlazor.Services;

public class ProductApiClient
{
    private readonly IHttpClientFactory
        _httpClientFactory;

    private readonly ApiRequestFactory
        _requestFactory;


    public ProductApiClient(
        IHttpClientFactory httpClientFactory,
        ApiRequestFactory requestFactory)
    {
        _httpClientFactory =
            httpClientFactory;

        _requestFactory =
            requestFactory;
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


    public async Task<ProductResponse?>
        GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient(
                "StoreApi");


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


    public async Task<HttpResponseMessage>
        CreateAsync(
            CreateProductRequest request,
            CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient(
                "StoreApi");


        using HttpRequestMessage httpRequest =
            _requestFactory.Create(
                HttpMethod.Post,
                "api/products",
                JsonContent.Create(request),
                requiresAuthentication: true);


        return await httpClient.SendAsync(
            httpRequest,
            cancellationToken);
    }


    public async Task<HttpResponseMessage>
        UpdateAsync(
            int id,
            UpdateProductRequest request,
            CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient(
                "StoreApi");


        using HttpRequestMessage httpRequest =
            _requestFactory.Create(
                HttpMethod.Put,
                $"api/products/{id}",
                JsonContent.Create(request),
                requiresAuthentication: true);


        return await httpClient.SendAsync(
            httpRequest,
            cancellationToken);
    }


    public async Task<HttpResponseMessage>
        DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient(
                "StoreApi");


        using HttpRequestMessage httpRequest =
            _requestFactory.Create(
                HttpMethod.Delete,
                $"api/products/{id}",
                requiresAuthentication: true);


        return await httpClient.SendAsync(
            httpRequest,
            cancellationToken);
    }
}