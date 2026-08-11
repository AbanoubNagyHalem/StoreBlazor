using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace StoreBlazor.Services;

public class CustomAuthenticationStateProvider
    : AuthenticationStateProvider
{
  private readonly AuthStateService _authState;

  public CustomAuthenticationStateProvider(
      AuthStateService authState)
  {
    _authState = authState;

    _authState.OnChange +=
        HandleAuthStateChanged;
  }

  public override Task<AuthenticationState>
      GetAuthenticationStateAsync()
  {
    ClaimsIdentity identity;

    if (!_authState.IsAuthenticated)
    {
      identity =
          new ClaimsIdentity();
    }
    else
    {
      List<Claim> claims =
      [
          new Claim(
                    ClaimTypes.NameIdentifier,
                    _authState.UserId?.ToString() ?? ""),

                new Claim(
                    ClaimTypes.Name,
                    _authState.Name ?? ""),

                new Claim(
                    ClaimTypes.Email,
                    _authState.Email ?? ""),

                new Claim(
                    ClaimTypes.Role,
                    _authState.Role ?? "")
      ];

      identity =
          new ClaimsIdentity(
              claims,
              authenticationType: "jwt");
    }

    ClaimsPrincipal user =
        new(identity);

    AuthenticationState state =
        new(user);

    return Task.FromResult(
        state);
  }

  private void HandleAuthStateChanged()
  {
    NotifyAuthenticationStateChanged(
        GetAuthenticationStateAsync());
  }
}