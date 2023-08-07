using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace DemoWebApi.Extensions
{
    public static class HttpContextExtensions
    {
        public static IPAddress GetRemoteIPAddress(this HttpContext context, bool allowForwarded = true)
        {
            try
            {
                if (allowForwarded)
                {
                    string header = context.Request?.Headers["CF-Connecting-IP"].FirstOrDefault() ?? context.Request?.Headers["X-Forwarded-For"].FirstOrDefault();
                    if (IPAddress.TryParse(header, out IPAddress ip))
                    {
                        return ip;
                    }
                }
                return context.Connection.RemoteIpAddress;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static IPAddress GetRemoteIPAddress(this IHttpContextAccessor httpContextAccessor, bool allowForwarded = true)
        {
            try
            {
                if (allowForwarded)
                {
                    string header = (httpContextAccessor.HttpContext?.Request?.Headers["CF-Connecting-IP"].FirstOrDefault() ?? httpContextAccessor.HttpContext?.Request?.Headers["X-Forwarded-For"].FirstOrDefault());
                    if (IPAddress.TryParse(header, out IPAddress ip))
                    {
                        return ip;
                    }
                }
                return httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress;
            }
            catch (Exception)
            {
            }
            return null;
        }

        //public static string GetUserCodeOfUserLogin(this IHttpContextAccessor httpContextAccessor)
        //{
        //    _ = httpContextAccessor.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var tokenString);

        //    var jwtEncodedString = tokenString.ToString()[7..];

        //    var token = new JwtSecurityToken(jwtEncodedString);

        //    var claim = token.Claims.FirstOrDefault(x => x.Type == ClaimTypeConst.UserCode);

        //    string userCode = claim.Value;

        //    return userCode;
        //}
    }
}
