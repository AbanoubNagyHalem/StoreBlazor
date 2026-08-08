using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using StoreBlazor.Components;
using StoreBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

string apiBaseUrl =
    builder.Configuration["ApiSettings:BaseUrl"]
    ?? throw new InvalidOperationException(
        "ApiSettings:BaseUrl is missing.");

builder.Services.AddScoped<AuthStateService>();

builder.Services.AddScoped<ProtectedSessionStorage>();

builder.Services.AddHttpClient(
    "StoreApi",
    client =>
    {
        client.BaseAddress =
            new Uri(apiBaseUrl);
    });

builder.Services.AddScoped<AuthApiClient>();

builder.Services.AddScoped<ProductApiClient>();

builder.Services.AddScoped<CategoryApiClient>();

var app = builder.Build();

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