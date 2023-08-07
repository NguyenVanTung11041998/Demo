using Microsoft.Net.Http.Headers;
using DemoWebApi.Extensions;
using System.Globalization;

namespace DemoWebApi.Middlewares
{
    public class LocalizationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var cultureKey = context.Request.Headers[HeaderNames.AcceptLanguage];

            if (!$"{cultureKey}".HasValue())
                cultureKey = "vi";

            if (DoesCultureExist(cultureKey))
            {
                var culture = new CultureInfo(cultureKey);

                Thread.CurrentThread.CurrentCulture = culture;

                Thread.CurrentThread.CurrentUICulture = culture;
            }

            await next(context);
        }

        private static bool DoesCultureExist(string cultureName)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Any(culture => string.Equals(culture.Name, cultureName,
                        StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
