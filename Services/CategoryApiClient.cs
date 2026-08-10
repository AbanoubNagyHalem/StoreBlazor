using System.Net;
using System.Net.Http.Json;
using StoreBlazor.Models;

namespace StoreBlazor.Services;

public class CategoryApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApiRequestFactory _requestFactory;

    public CategoryApiClient(
        IHttpClientFactory httpClientFactory,
        ApiRequestFactory requestFactory)
    {
        _httpClientFactory = httpClientFactory;
        _requestFactory = requestFactory;
    }

    public async Task<List<CategoryResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient("StoreApi");

        List<CategoryResponse>? categories =
            await httpClient.GetFromJsonAsync<List<CategoryResponse>>(
                "api/categories",
                cancellationToken);

        return categories ?? [];
    }

    public async Task<CategoryResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient("StoreApi");

        HttpResponseMessage response =
            await httpClient.GetAsync(
                $"api/categories/{id}",
                cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<CategoryResponse>(
                cancellationToken);
    }

    public async Task<HttpResponseMessage> CreateAsync(
        CreateCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient("StoreApi");

        using HttpRequestMessage httpRequest =
            _requestFactory.Create(
                HttpMethod.Post,
                "api/categories",
                JsonContent.Create(request),
                requiresAuthentication: true);

        return await httpClient.SendAsync(
            httpRequest,
            cancellationToken);
    }

    public async Task<HttpResponseMessage> UpdateAsync(
        int id,
        UpdateCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient("StoreApi");

        using HttpRequestMessage httpRequest =
            _requestFactory.Create(
                HttpMethod.Put,
                $"api/categories/{id}",
                JsonContent.Create(request),
                requiresAuthentication: true);

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
            _requestFactory.Create(
                HttpMethod.Delete,
                $"api/categories/{id}",
                requiresAuthentication: true);

        return await httpClient.SendAsync(
            httpRequest,
            cancellationToken);
    }
}