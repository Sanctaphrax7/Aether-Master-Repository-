using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Headers;
using Aether.Client.ClientModels;
using Aether.Shared.Models;

namespace Aether.Client.Services.AuthStateProvider
{
    public class AuthStateProvider : AuthenticationStateProvider//, IDisposable
    {
        private readonly HttpClient _http;
        private readonly ISessionStorageService _sessionStorage;

        //public AuthUser CurrentUser { get; private set; } = new();
        public AuthUser CurrentUser { get; private set; } = new();
        public AuthStateProvider(HttpClient http, ISessionStorageService sessionStorage)
        {
            _http = http;
            _sessionStorage = sessionStorage;
       
        }

        //public AuthStateProvider()
        //{
        //    AuthenticationStateChanged += OnAuthenticationStateChangeAsync;
        //}

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal();
           
            string token = await _sessionStorage.GetItemAsStringAsync("token");
            _http.DefaultRequestHeaders.Authorization = null;
            
            var authUser = await AuthenticationRequest();
            CurrentUser = authUser;
               
                if (authUser != null)
                {
                    user = authUser.ToClaimsPrincipal();
                }
            //var user = new ClaimsPrincipal(identity);
            //if (!string.IsNullOrEmpty(token))
            //{
            //    identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            //    _http.DefaultRequestHeaders.Authorization =
            //        new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            //}

            var state = new AuthenticationState(user);
            
            NotifyAuthenticationStateChanged(Task.FromResult(state));//Components will check this for App's current authentication state
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
        //TEST

        private async Task<AuthUser> AuthenticationRequest()
        {
            string token = await _sessionStorage.GetItemAsStringAsync("token");

            var claimsPrincipal = CreateClaims(token);//Recently Added
            var currentUser = AuthUser.FromClaimsPrinciple(claimsPrincipal);//Recently Added

            return currentUser;
        }
        private ClaimsPrincipal CreateClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();//Recently Added
            var identity = new ClaimsIdentity();
           
            if (tokenHandler.CanReadToken(token))
            { 
                var jwtSecurityToken = tokenHandler.ReadJwtToken(token); //Recently Added
                 identity = new ClaimsIdentity(jwtSecurityToken.Claims, "Claims");//Recently Added

            }
            return new ClaimsPrincipal(identity);
        }


        //private async void OnAuthenticationStateChangeAsync(Task<AuthenticationState> task)
        //{

        //    var authState = await task;

        //    if (authState != null)
        //    {
        //        CurrentUser = AuthUser.FromClaimsPrinciple(authState.AuthUser);
        //    }
        //}
        //public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangeAsync;
    }

}
