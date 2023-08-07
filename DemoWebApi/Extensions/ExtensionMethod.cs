using DemoWebApi.Dtos;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace DemoWebApi.Extensions
{
    public static class ExtensionMethod
    {
        public static IQueryable<TSource> WhereIf<TSource>(
            this IQueryable<TSource> source,
            bool condition,
            Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);

            return source;
        }

        public static T CloneObject<T>(this T obj) where T : class
        {
            string objContent = JsonConvert.SerializeObject(obj);

            return JsonConvert.DeserializeObject<T>(objContent);
        }

        public static FileBase64Dto ConvertToBase64(
            this byte[] fileBytes,
            string fileName,
            string contentType
        )
        {
            string fileBase64 = Convert.ToBase64String(fileBytes, 0, fileBytes.Length);

            return new FileBase64Dto
            {
                FileName = fileName,
                Base64 = fileBase64,
                ContentType = contentType
            };
        }
    }
}
