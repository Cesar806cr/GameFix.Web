using System.Text;
using Gamefix.Web.Models;
using Newtonsoft.Json;


namespace Gamefix.Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string messagesJson;
            string routeWhereExceptionOccurred;
            string eventId;
            string path = string.Empty;

            StringBuilder str = new();

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                routeWhereExceptionOccurred = context.Request.Path.ToString();

                path = JsonConvert.SerializeObject(
                    routeWhereExceptionOccurred
                );

                // Crear IdEvent aleatorio
                Random random = new();

                eventId =
                    random.Next(1, 5000).ToString("######")
                    + "-"
                    + DateTime.Now.ToString("yyMMddHHmmss");

                ErrorMiddlewareViewModel result = new()
                {
                    Path = path,
                    IdEvent = eventId
                };

                if (ex is AggregateException aggregateException)
                {
                    result.ListMessages = aggregateException.InnerExceptions
                        .Select(e => e.Message)
                        .ToList();
                }
                else
                {
                    result.ListMessages = new List<string>
                {
                    ex.Message
                };
                }

                str.AppendLine();
                str.AppendLine($"EventId    : {eventId}");
                str.AppendLine(
                    $"ErrorList  : {string.Join(",", result.ListMessages)}"
                );
                str.AppendLine($"StackTrace : {ex.StackTrace}");

                messagesJson = JsonConvert.SerializeObject(result);

                context.Items["ErrorMessagesJson"] = messagesJson;

                _logger.LogError(str.ToString());

                await HandleErrorAsync(context);
            }
        }

        private static Task HandleErrorAsync(HttpContext context)
        {
            string? messageJson =
                context.Items["ErrorMessagesJson"] as string;

            string encodedMessage = Uri.EscapeDataString(
                messageJson ?? string.Empty
            );

            string redirectUrl =
                $"/Home/ErrorHandler?messagesJson={encodedMessage}";

            context.Response.Redirect(redirectUrl);

            return Task.CompletedTask;
        }
    }
}

