using System.Security.Claims;
using Blazored.SessionStorage;

namespace Aether.Server.Services
{
    public class UserService: IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly ISessionStorageService _sessionStorage;

        public UserService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient, ISessionStorageService sessionStorage)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _sessionStorage = sessionStorage;

        }

        public string GetName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return result;
        }
    }
}
