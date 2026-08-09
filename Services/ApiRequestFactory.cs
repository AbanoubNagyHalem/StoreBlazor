using System.Net.Http.Headers;

namespace StoreBlazor.Services;

public class ApiRequestFactory
{
  private readonly AuthStateService _authState;

  public ApiRequestFactory(
      AuthStateService authState)
  {
    _authState = authState;
  }

  public HttpRequestMessage Create(
      HttpMethod method,
      string url,
      HttpContent? content = null,
      bool requiresAuthentication = false)
  {
    HttpRequestMessage request =
        new(method, url);

    if (content is not null)
    {
      request.Content = content;
    }

    if (requiresAuthentication &&
        !string.IsNullOrWhiteSpace(_authState.Token))
    {
      request.Headers.Authorization =
          new AuthenticationHeaderValue(
              "Bearer",
              _authState.Token);
    }

    return request;
  }
}