using System.Net;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using UserListBackend.Models.ApiModels;
using FluentValidation;

namespace UserListBackend
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        private readonly JsonSerializerOptions _jsonSerializerOptions =
            new()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
            };

        public CustomExceptionMiddleware(ILogger<CustomExceptionMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (KeyNotFoundException exception)
            {
                _logger.LogError($"Exception was thrown: {exception}");
                await HandleExceptionAsync(httpContext,
                    new()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = exception.Message
                    })
                    .ConfigureAwait(false);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError($"Exception was thrown: {exception}");
                await HandleExceptionAsync(httpContext,
                    new()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = exception.Message
                    })
                    .ConfigureAwait(false);
            }
            catch (TargetException exception)
            {
                _logger.LogError($"Exception was thrown: {exception}");
                await HandleExceptionAsync(httpContext,
                    new()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = exception.Message
                    })
                    .ConfigureAwait(false);
            }
            catch (ValidationException exception)
            {
                _logger.LogError($"Exception was thrown: {exception}");
                await HandleExceptionAsync(httpContext,
                    new()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Validation failed",
                        Errors = exception.Errors.Select(a => a.ErrorMessage)
                    })
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception was thrown: {exception}");
                await HandleExceptionAsync(httpContext,
                    new()
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Message = "Internal server error"
                    })
                    .ConfigureAwait(false);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, ErrorResponse response)
        {
            string jsonResponse = JsonSerializer.Serialize(response, _jsonSerializerOptions);
            _logger.LogDebug($"Error responce: {jsonResponse}");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
