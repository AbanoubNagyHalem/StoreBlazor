using System.Net;

namespace StoreBlazor.Services;

public class ApiErrorMessageProvider
{
  public string GetMessage(
      HttpStatusCode statusCode,
      string defaultMessage = "The request could not be completed.")
  {
    return statusCode switch
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