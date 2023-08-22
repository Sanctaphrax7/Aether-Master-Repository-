using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Aether.Client.Services.AuthStateProvider
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;
        private readonly ISessionStorageService _sessionStorage;

        public AuthStateProvider(HttpClient http, ILocalStorageService localStorageService, ISessionStorageService sessionStorage)
        {
            _http = http;
            _localStorageService = localStorageService;
            _sessionStorage = sessionStorage;
        }

        //public User user { get; private set; } = new();
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
           
            //Test Section
                


            //string token = await _localStorageService.GetItemAsStringAsync("token");
            string token = await _sessionStorage.GetItemAsStringAsync("token");
            var identity = new ClaimsIdentity(); 
            _http.DefaultRequestHeaders.Authorization = null;
          
           if (!string.IsNullOrEmpty(token))
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("\"",""));
            }
            var user = new ClaimsPrincipal(identity); 
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));//Components will check this for app's current authentication state
            return state;
        }
       
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        public static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }

}
