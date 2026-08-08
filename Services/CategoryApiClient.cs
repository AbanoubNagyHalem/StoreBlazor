using System.Net.Http.Json;
using StoreBlazor.Models;

namespace StoreBlazor.Services;

public class CategoryApiClient
{
  private readonly HttpClient _httpClient;

  public CategoryApiClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<List<CategoryResponse>> GetAllAsync(
      CancellationToken cancellationToken = default)
  {
    List<CategoryResponse>? categories =
        await _httpClient.GetFromJsonAsync<List<CategoryResponse>>(
            "api/categories",
            cancellationToken);

    return categories ?? [];
  }
}