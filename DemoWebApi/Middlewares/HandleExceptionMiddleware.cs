using DemoWebApi.ExceptionHandling;
using Microsoft.Extensions.Localization;
using Serilog;

namespace DemoWebApi.Middlewares
{
    public class HandleExceptionMiddleware
    {
        private RequestDelegate Next { get; }

        private IStringLocalizer<HandleExceptionMiddleware> L { get; }

        public HandleExceptionMiddleware(RequestDelegate next, IStringLocalizer<HandleExceptionMiddleware> l)
        {
            Next = next;

            L = l;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (UserFriendlyException ex)
            {
                HandleException(context, ex);
            }
            catch (Exception exceptionObj)
            {
                HandleException(context, exceptionObj);
            }
        }

        private void HandleException(HttpContext context, UserFriendlyException exception)
        {
            Log.Error($"\n\n{exception.Message}\n{exception.StackTrace}\n");

            context.Response.StatusCode = 500;
        }

        private void HandleException(HttpContext context, Exception exception)
        {
            Log.Error($"\n\n{exception.Message}\n\n");

            Log.Error(exception.StackTrace);

            context.Response.StatusCode = 500;
        }
    }
}
