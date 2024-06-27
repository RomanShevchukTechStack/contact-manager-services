
using ContactManager.Data.Models;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;

namespace ContactManager.Middleware
{
  public static class ExceptionMiddlewareExtensions
  {
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
      app.UseExceptionHandler(appError =>
      {
        appError.Run(async context =>
        {
          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          context.Response.ContentType = "application/json";
          var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
          if (contextFeature != null)
          {
            Log.Logger.Error($"Something went wrong: {contextFeature.Error}");
            await context.Response.WriteAsync(new ErrorDetails()
            {
              StatusCode = context.Response.StatusCode,
              Message = "Something went wrong: Please try again later"
            }.ToString());
          }
        });
      });
    }
  }
}
