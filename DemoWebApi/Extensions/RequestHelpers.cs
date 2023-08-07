using DemoWebApi.Consts;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Net.Http.Headers;
using System.Text;

namespace DemoWebApi.Extensions
{
    public static class RequestHelpers
    {
        public static async Task<T> PostDataAsJsonAsync<T>(string requestUrl, object body, IDictionary<string, string> defaultRequestHeaders = null) where T : class
        {
            using var httpClient = new HttpClient();

            if (defaultRequestHeaders != null)
            {
                foreach (var item in defaultRequestHeaders)
                {
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            var response = await httpClient.PostAsJsonAsync(requestUrl, body);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(content);
            }

            return (T)Activator.CreateInstance(typeof(T));
        }


        public static async Task<T> PostDataAsync<T>(string requestUrl, object body, string formatDate = "yyyy-MM-dd", IDictionary<string, string> defaultRequestHeaders = null) where T : class
        {
            using var httpClient = new HttpClient();

            if (defaultRequestHeaders != null)
            {
                foreach (var item in defaultRequestHeaders)
                {
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            string contentJson = JsonConvert.SerializeObject(body);

            var content = new StringContent(contentJson, Encoding.UTF8, MimeTypeNames.ApplicationJson);

            var response = await httpClient.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string contentData = await response.Content.ReadAsStringAsync();

                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = formatDate };

                return JsonConvert.DeserializeObject<T>(contentData, dateTimeConverter);
            }
            else
                Log.Error($"----{requestUrl} {response.StatusCode} -- {response.ReasonPhrase}");

            return (T)Activator.CreateInstance(typeof(T));
        }

        public static async Task<T> GetDataAsJsonAsync<T>(string requestUrl, IDictionary<string, string> defaultRequestHeaders = null) where T : class
        {
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Clear();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MimeTypeNames.ApplicationJson));

            if (defaultRequestHeaders != null)
            {
                foreach (var item in defaultRequestHeaders)
                {
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            var response = await httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(content);
            }

            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}
