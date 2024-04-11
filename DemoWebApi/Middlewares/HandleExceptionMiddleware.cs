using DemoWebApi.Consts;
using DemoWebApi.Dtos;
using DemoWebApi.ExceptionHandling;
using DemoWebApi.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Serilog;
using System.Net;

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

                context.Response.Body.Seek(0, SeekOrigin.Begin);

                string text = await new StreamReader(context.Response.Body).ReadToEndAsync();

                var obj = JsonConvert.DeserializeObject(text);

                bool isSuccess = context.Response.StatusCode >= (int)HttpStatusCode.OK && context.Response.StatusCode < (int)HttpStatusCode.MultipleChoices;

                int statusCode = isSuccess ? (int)HttpStatusCode.OK : context.Response.StatusCode;

                var res = new BaseReponse
                {
                    ResponseText = isSuccess ? L["Success"] : L["InternalServerError"],
                    ResponseCode = statusCode,
                    Data = obj
                }.ToString();

                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();

                    context.Response.ContentType = MimeTypeNames.ApplicationJson;

                    context.Response.StatusCode = statusCode;

                    await context.Response.WriteAsync(res);
                }
            }
            catch (UserFriendlyException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(context, exceptionObj);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, UserFriendlyException exception)
        {
            Log.Error($"\n\n{exception.Message}\n{exception.StackTrace}\n");

            string result;

            context.Response.ContentType = MimeTypeNames.ApplicationJson;

            if (exception is not null)
            {
                bool isValidJson = exception.Details.IsValidJson();

                var obj = isValidJson ? JsonConvert.DeserializeObject(exception.Details) : exception.Details;

                result = new BaseReponse
                {
                    ResponseText = exception.Message,
                    ResponseCode = (int)HttpStatusCode.Forbidden,
                    Data = obj
                }.ToString();

                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            else
            {
                result = new BaseReponse
                {
                    ResponseText = L["InternalServerError"],
                    ResponseCode = (int)HttpStatusCode.InternalServerError,
                }.ToString();

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            await context.Response.WriteAsync(result);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Log.Error($"\n\n{exception.Message}\n\n");

            Log.Error(exception.StackTrace);

            context.Response.ContentType = "application/json";

            string result = new BaseReponse
            {
                ResponseText = L["InternalServerError"],
                ResponseCode = (int)HttpStatusCode.InternalServerError,
                Data = new
                {
                    exception.StackTrace,
                    exception.Message
                }
            }.ToString();

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(result);
        }
    }
}
