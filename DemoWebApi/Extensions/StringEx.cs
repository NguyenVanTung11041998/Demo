using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace DemoWebApi.Extensions
{
    public static class StringEx
    {
        public static bool HasValue(this string value, bool isTrimSpace = true)
        {
            return isTrimSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
        }

        public static string CreateMD5Hash(string input)
        {
            var md5 = MD5.Create();

            var originalBytes = Encoding.ASCII.GetBytes(input);

            var encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).ToLower().Replace("-", "");
        }

        public static bool IsValidJson(this string strInput)
        {
            if (!strInput.HasValue())
                return false;

            strInput = strInput.Trim();

            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) ||
                (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try
                {
                    var obj = JToken.Parse(strInput);

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static string GetDescription<T>(this T enumValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return string.Empty;

            string description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(description);

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs?.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description;
        }

        public static string ToCamelCase(this string str)
        {
            if (str.HasValue() && str.Length > 1)
                return char.ToLowerInvariant(str[0]) + str.Substring(1);

            return str.ToLowerInvariant();
        }
    }
}
