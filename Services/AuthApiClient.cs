using System.Net;
using System.Net.Http.Json;
using StoreBlazor.Models;

namespace StoreBlazor.Services;

public class AuthApiClient
{
  private readonly IHttpClientFactory _httpClientFactory;


  public AuthApiClient(
      IHttpClientFactory httpClientFactory)
  {
    _httpClientFactory = httpClientFactory;
  }


  public async Task<AuthResponse?> LoginAsync(
      LoginRequest request,
      CancellationToken cancellationToken = default)
  {
    HttpClient httpClient =
        _httpClientFactory.CreateClient(
            "StoreApi");


    HttpResponseMessage response =
        await httpClient.PostAsJsonAsync(
            "api/auth/login",
            request,
            cancellationToken);


    if (response.StatusCode ==
        HttpStatusCode.Unauthorized)
    {
      return null;
    }


    response.EnsureSuccessStatusCode();


    return await response.Content
        .ReadFromJsonAsync<AuthResponse>(
            cancellationToken);
  }
}