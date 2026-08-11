using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace StoreBlazor.Services;

public class AdminPageGuard
{
  private readonly AuthStateService _authState;

  private readonly AuthenticationStateProvider
      _authenticationStateProvider;

  private readonly NavigationManager
      _navigation;


  public AdminPageGuard(
      AuthStateService authState,
      AuthenticationStateProvider authenticationStateProvider,
      NavigationManager navigation)
  {
    _authState =
        authState;

    _authenticationStateProvider =
        authenticationStateProvider;

    _navigation =
        navigation;
  }


  public async Task<bool> EnsureAdminAsync(
      string fallbackUrl)
  {
    await _authState.LoadUserAsync();


    AuthenticationState state =
        await _authenticationStateProvider
            .GetAuthenticationStateAsync();


    bool isAuthenticated =
        state.User.Identity?.IsAuthenticated
        ?? false;


    if (!isAuthenticated)
    {
      _navigation.NavigateTo(
          "/login");

      return false;
    }


    if (!state.User.IsInRole("Admin"))
    {
      _navigation.NavigateTo(
          fallbackUrl);

      return false;
    }


    return true;
  }
}