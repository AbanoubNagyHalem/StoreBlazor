using System.Net.Http.Json;
using StoreBlazor.Models;

namespace StoreBlazor.Services;

public class CategoryApiClient
{
  private readonly IHttpClientFactory
      _httpClientFactory;


  public CategoryApiClient(
      IHttpClientFactory httpClientFactory)
  {
    _httpClientFactory =
        httpClientFactory;
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
}