using Application.DTO.Enums;
using Application.DTO.Responses.General;
using FluentValidation;

namespace Presentation.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error during execute request");
            var errorCode = ex.Errors.First().ErrorCode!; // value always will be provided
            var internalStatusCode = Enum.Parse<CustomStatusCodes>(errorCode);
            BaseResponse response = internalStatusCode;
            var httpStatusCode = int.Parse(internalStatusCode.ToString("D")[..3]);
            context.Response.StatusCode = httpStatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unknown error during execute request");
            const CustomStatusCodes internalStatusCode = CustomStatusCodes.Unknown;
            BaseResponse response = internalStatusCode;
            var httpStatusCode = int.Parse(internalStatusCode.ToString("D")[..3]);
            context.Response.StatusCode = httpStatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}