using System.Net.Http.Json;
using StoreBlazor.Models;

namespace StoreBlazor.Services;

public class CategoryApiClient
{
    private readonly IHttpClientFactory
        _httpClientFactory;

    private readonly ApiRequestFactory
        _requestFactory;


    public CategoryApiClient(
        IHttpClientFactory httpClientFactory,
        ApiRequestFactory requestFactory)
    {
        _httpClientFactory =
            httpClientFactory;

        _requestFactory =
            requestFactory;
    }


    public async Task<List<CategoryResponse>>
        GetAllAsync(
            CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient(
                "StoreApi");


        List<CategoryResponse>? categories =
            await httpClient
                .GetFromJsonAsync<
                    List<CategoryResponse>>(
                    "api/categories",
                    cancellationToken);


        return categories ?? [];
    }


    public async Task<HttpResponseMessage>
        CreateAsync(
            CreateCategoryRequest request,
            CancellationToken cancellationToken = default)
    {
        HttpClient httpClient =
            _httpClientFactory.CreateClient(
                "StoreApi");


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
}