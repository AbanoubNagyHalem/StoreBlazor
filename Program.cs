using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using StoreBlazor.Components;
using StoreBlazor.Services;

var builder =
    WebApplication.CreateBuilder(args);


builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();


string apiBaseUrl =
    builder.Configuration["ApiSettings:BaseUrl"]
    ?? throw new InvalidOperationException(
        "ApiSettings:BaseUrl is missing.");


// ========================
// Authentication State
// ========================

builder.Services.AddScoped<
    ProtectedSessionStorage>();

builder.Services.AddScoped<
    AuthStateService>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<
    CustomAuthenticationStateProvider>();

builder.Services.AddScoped<
    AuthenticationStateProvider>(
        serviceProvider =>
            serviceProvider.GetRequiredService<
                CustomAuthenticationStateProvider>());


// ========================
// Page Guards
// ========================

builder.Services.AddScoped<
    AdminPageGuard>();


// ========================
// HTTP Infrastructure
// ========================

builder.Services.AddHttpClient(
    "StoreApi",
    client =>
    {
        client.BaseAddress =
            new Uri(apiBaseUrl);
    });

builder.Services.AddScoped<
    ApiRequestFactory>();


// ========================
// API Clients
// ========================

builder.Services.AddScoped<
    AuthApiClient>();

builder.Services.AddScoped<
    ProductApiClient>();

builder.Services.AddScoped<
    CategoryApiClient>();


// ========================
// Shared State
// ========================

builder.Services.AddScoped<
    SelectedProductState>();


// ========================
// UI / API Helpers
// ========================

builder.Services.AddScoped<
    ApiErrorMessageProvider>();


var app =
    builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/Error",
        createScopeForErrors: true);

    app.UseHsts();
}


app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();