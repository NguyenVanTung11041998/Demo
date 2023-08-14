using AutoMapper;
using DemoWebApi.Dtos.Users;
using DemoWebApi.ExceptionHandling;
using DemoWebApi.Extensions;
using DemoWebApi.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace DemoWebApi.Services
{
    public abstract class ApplicationServiceBase
    {
        protected IConfiguration Configuration { get; }
        protected IMapper Mapper { get; }
        protected IStringLocalizer<ApplicationServiceBase> L { get; }
        protected IUserRepository UserRepository { get; }
        protected IHttpContextAccessor HttpContext { get; }

        public ApplicationServiceBase(
            IConfiguration configuration,
            IMapper mapper,
            IStringLocalizer<ApplicationServiceBase> l,
            IUserRepository userRepository,
            IHttpContextAccessor httpContext
        )
        {
            Configuration = configuration;

            Mapper = mapper;

            L = l;

            UserRepository = userRepository;

            HttpContext = httpContext;
        }

        protected int GetUserIdOfCurrentUser()
        {
            return HttpContext.GetUserIdOfUserLogin();
        }

        protected virtual async Task<UserDto> GetCurrentUserAsync()
        {
            int id = GetUserIdOfCurrentUser();

            var user = await UserRepository.GetDbSet().FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) throw new UserFriendlyException(L["UserNotFound"]);

            return Mapper.Map<UserDto>(user);
        }

        protected bool DoesCultureExist(string cultureName)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Any(culture => string.Equals(culture.Name, cultureName,
                        StringComparison.CurrentCultureIgnoreCase));
        }

        protected void SetCurrentLanguage(string language)
        {
            if (!$"{language}".HasValue())
                language = "vi";

            if (DoesCultureExist(language))
            {
                var culture = new CultureInfo(language);

                Thread.CurrentThread.CurrentCulture = culture;

                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }
    }
}
