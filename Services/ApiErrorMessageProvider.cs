using System.Net;
using System.Net.Http.Json;
using StoreBlazor.Models;

namespace StoreBlazor.Services;

public class ApiErrorMessageProvider
{
    public async Task<string> GetMessageAsync(
        HttpResponseMessage response,
        string defaultMessage,
        CancellationToken cancellationToken = default)
    {
        try
        {
            ApiErrorResponse? apiError =
                await response.Content
                    .ReadFromJsonAsync<ApiErrorResponse>(
                        cancellationToken);


            if (!string.IsNullOrWhiteSpace(
                    apiError?.Message))
            {
                return apiError.Message;
            }


            if (!string.IsNullOrWhiteSpace(
                    apiError?.Detail))
            {
                return apiError.Detail;
            }


            if (!string.IsNullOrWhiteSpace(
                    apiError?.Title))
            {
                return apiError.Title;
            }
        }
        catch
        {
            // Fall back to the HTTP status code.
        }


        return response.StatusCode switch
        {
            HttpStatusCode.Unauthorized =>
                "You must login first.",

            HttpStatusCode.Forbidden =>
                "You do not have permission to perform this action.",

            HttpStatusCode.NotFound =>
                "The requested resource was not found.",

            HttpStatusCode.Conflict =>
                "The request conflicts with existing data.",

            HttpStatusCode.BadRequest =>
                "The request contains invalid data.",

            _ =>
                defaultMessage
        };
    }
}