using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace DemoWebApi.Localizers
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private IDistributedCache Cache { get; }

        public JsonStringLocalizerFactory(IDistributedCache cache)
        {
            Cache = cache;
        }

        public IStringLocalizer Create(Type resourceSource) =>
            new JsonStringLocalizer(Cache);

        public IStringLocalizer Create(string baseName, string location) =>
            new JsonStringLocalizer(Cache);
    }
}
