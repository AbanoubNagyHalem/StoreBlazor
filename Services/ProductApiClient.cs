using System.Net.Http.Json;
using StoreBlazor.Models;

namespace StoreBlazor.Services;

public class ProductApiClient
{
  private readonly HttpClient _httpClient;

  public ProductApiClient(
      HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<PagedResponse<ProductResponse>?> GetAllAsync(
      ProductQueryParameters parameters,
      CancellationToken cancellationToken = default)
  {
    string url =
        $"api/products" +
        $"?search={Uri.EscapeDataString(parameters.Search ?? "")}" +
        $"&page={parameters.Page}" +
        $"&pageSize={parameters.PageSize}" +
        $"&sortBy={parameters.SortBy}" +
        $"&sortDirection={parameters.SortDirection}";

    if (parameters.CategoryId.HasValue)
    {
      url += $"&categoryId={parameters.CategoryId.Value}";
    }

    return await _httpClient
        .GetFromJsonAsync<PagedResponse<ProductResponse>>(
            url,
            cancellationToken);
  }

  public async Task<HttpResponseMessage> CreateAsync(
      CreateProductRequest request,
      CancellationToken cancellationToken = default)
  {
    return await _httpClient.PostAsJsonAsync(
        "api/products",
        request,
        cancellationToken);
  }
}