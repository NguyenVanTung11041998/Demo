using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace DemoWebApi.Localizers
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private IDistributedCache Cache { get; }

        private JsonSerializer Serializer => new JsonSerializer();

        public JsonStringLocalizer(IDistributedCache cache)
        {
            Cache = cache;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);

                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var actualValue = this[name];

                return !actualValue.ResourceNotFound
                    ? new LocalizedString(name, string.Format(actualValue.Value, arguments), false)
                    : actualValue;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var filePath = $"{currentDirectory}/Localizers/Resources/{GetLanguageName()}.json";

            using var str = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            using var sReader = new StreamReader(str);

            using var reader = new JsonTextReader(sReader);

            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                    continue;

                string key = reader.Value as string;

                reader.Read();

                var value = Serializer.Deserialize<string>(reader);

                yield return new LocalizedString(key, value, false);
            }
        }

        private string GetString(string key)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            string relativeFilePath = $"{currentDirectory}/Localizers/Resources/{GetLanguageName()}.json";

            var fullFilePath = Path.GetFullPath(relativeFilePath);

            if (!File.Exists(fullFilePath))
            {
                relativeFilePath = $"{currentDirectory}/Localizers/Resources/vi.json";

                fullFilePath = Path.GetFullPath(relativeFilePath);

                if (!File.Exists(fullFilePath))
                    return key;
            }

            var cacheKey = $"locale_{GetLanguageName()}_{key}";

            var cacheValue = Cache.GetString(cacheKey);

            if (!string.IsNullOrEmpty(cacheValue))
            {
                return cacheValue;
            }

            var result = GetValueFromJSON(key, Path.GetFullPath(relativeFilePath));

            if (!string.IsNullOrEmpty(result))
            {
                Cache.SetString(cacheKey, result);
            }

            return result;
        }

        private string GetValueFromJSON(string propertyName, string filePath)
        {
            if (propertyName == null)
            {
                return default;
            }

            if (filePath == null)
            {
                return default;
            }

            using var str = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            using var sReader = new StreamReader(str);

            using var reader = new JsonTextReader(sReader);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == propertyName)
                {
                    reader.Read();

                    return Serializer.Deserialize<string>(reader);
                }
            }

            return default;
        }

        private string GetLanguageName()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }
    }
}
