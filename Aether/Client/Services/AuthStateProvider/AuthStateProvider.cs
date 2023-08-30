using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.JavaScript;
using Aether.Client.Extensions;

namespace Aether.Client.Services.AuthStateProvider
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;
        private readonly ISessionStorageService _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public AuthStateProvider(HttpClient http,ISessionStorageService sessionStorage)
        {
            _http = http;
            _sessionStorage = sessionStorage;
        }

        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
            
        //    string token = await _sessionStorage.GetItemAsStringAsync("token");
        //    var identity = new ClaimsIdentity(); 
        //    _http.DefaultRequestHeaders.Authorization = null;
          
        //   if (!string.IsNullOrEmpty(token))
        //    {
        //        identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        //        _http.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Bearer", token.Replace("\"",""));
        //    }
        //    var user = new ClaimsPrincipal(identity); 
        //    var state = new AuthenticationState(user);

        //    NotifyAuthenticationStateChanged(Task.FromResult(state));//Components will check this for app's current authentication state
        //    return state;
        //}
       
        //public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        //{
        //    var payload = jwt.Split('.')[1];
        //    var jsonBytes = ParseBase64WithoutPadding(payload);
        //    var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        //    return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        //}

        //public static byte[] ParseBase64WithoutPadding(string base64)
        //{
        //    switch (base64.Length % 4)
        //    {
        //        case 2: base64 += "=="; break;
        //        case 3: base64 += "="; break;
        //    }
        //    return Convert.FromBase64String(base64);
        //}
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await _sessionStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession == null)
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                    
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role)
                }, "JwtAuth"));
                   
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
                
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));

            }
        }

        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role)

                }));
                userSession.ExpiryTimeStamp = DateTime.UtcNow.AddSeconds(userSession.ExpiresIn);
                await _sessionStorage.SaveItemEncryptedAsync("UserSession", userSession);
            }
            else
            {
                claimsPrincipal = _anonymous;
                await _sessionStorage.RemoveItemAsync("UserSession");
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task<string> GetToken()
        {
            var result = string.Empty;

            try
            {
                var userSession = await _sessionStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now < userSession.ExpiryTimeStamp)
                    result = userSession.Token;
            }
            catch { }

            return result;
        }
    }

}
