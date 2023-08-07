using DemoWebApi.Extensions;
using Serilog;
using System.Text;

namespace DemoWebApi.Middlewares
{
    public class AutoLogRequestMiddleware
    {
        private RequestDelegate Next { get; }

        public AutoLogRequestMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var key = Guid.NewGuid().ToString();

            var ipAress = context.GetRemoteIPAddress();

            var ip = ipAress == null ? string.Empty : ipAress.ToString();

            await FormatRequest(context.Request, ip, key);

            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();

            context.Response.Body = responseBody;

            await Next(context);

            await FormatResponse(context.Response, ip, key);

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static async Task<string> FormatRequest(HttpRequest request, string ip, string key, Encoding encoding = null)
        {
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();
            }

            request.Body.Position = 0;

            var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);

            var body = await reader.ReadToEndAsync().ConfigureAwait(false);

            request.Body.Position = 0;

            Log.Warning($"\n\n[Request][{ip}][{key}] : [{request.Scheme}:{request.Host}{request.Path}{request.QueryString}]\n\n");

            //Log.Warning($"[Body][{ip}][{key}] : {body}");

            Log.Warning($"\n\n{body}\n\n\n");

            return body;
        }

        private static async Task FormatResponse(HttpResponse response, string ip, string key)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            //Log.Warning($"[Response][{ip}][{key}][{response.StatusCode}] : {text}");
        }
    }
}