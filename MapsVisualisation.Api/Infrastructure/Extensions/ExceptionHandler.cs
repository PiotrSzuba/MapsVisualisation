using MapsVisualisation.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace MapsVisualisation.Api.Infrastructure.Extensions;

public static class ExceptionHandler
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseExceptionHandler(new ExceptionHandlerOptions()
        {
            AllowStatusCode404Response = true,
            ExceptionHandler = async context =>
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var exceptionHandlerPath = context.Features.Get<IExceptionHandlerPathFeature>();
                var error = exceptionHandlerPath?.Error;

                var responseError = error switch
                {
                    EntityInvalidStateException => new ResponseError(error?.Message ?? "Error with no message"),
                    EntityNotFoundException => new ResponseError(error?.Message ?? "Error with no message"),
                    _ => null,
                };

                response.StatusCode = error switch
                {
                    EntityInvalidStateException => (int)HttpStatusCode.BadRequest,
                    EntityNotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                await response.WriteAsJsonAsync(responseError);
            }
        });
    }
}
