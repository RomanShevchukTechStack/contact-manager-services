using Ardalis.Result;
using FastEndpoints;

namespace ContactManager.API.Common.Extensions
{
  public static class EndpointExtensions
  {

    public static async Task SendResultAsync<TResult, TResponse>(this IEndpoint ep, TResult result, Func<TResult, TResponse> mapper, CancellationToken ct = default)
  where TResult : Ardalis.Result.IResult
    {
      switch (result.Status)
      {
        case ResultStatus.Ok:
          await ep.HttpContext.Response.SendAsync(mapper(result), cancellation: ct);
          break;

        case ResultStatus.Invalid:
        case ResultStatus.Error:
        case ResultStatus.Conflict:
          await ep.HttpContext.Response.SendErrorsAsync(ep.ValidationFailures, cancellation: ct);
          break;

        case ResultStatus.NotFound:
          await ep.HttpContext.Response.SendNotFoundAsync(ct);
          break;

        case ResultStatus.Forbidden:
          await ep.HttpContext.Response.SendForbiddenAsync(ct);
          break;

        case ResultStatus.Unauthorized:
          await ep.HttpContext.Response.SendUnauthorizedAsync(ct);
          break;

        default:
          throw new ArgumentOutOfRangeException(nameof(result), $"Current result Status '{result.Status}' does not support by EndpointExtensions.SendResultAsync");
      }
    }
  }
}
