﻿using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using Aether.Client.Services.AuthStateProvider;
using Blazored.SessionStorage;

namespace Aether.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;
        private readonly ISessionStorageService _sessionStorage;

        public AuthService(HttpClient http, ILocalStorageService localStorageService, ISessionStorageService sessionStorage)
        {
            _http = http;
            _localStorageService = localStorageService;
            _sessionStorage = sessionStorage;
        }
        public List<UserDto> Creds { get; set; } = new List<UserDto>();

        public async Task<HttpResponseMessage> Login(UserDto creds)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/login", creds);
            //var token = await response.Content.ReadAsStringAsync();
            //await _sessionStorage.SetItemAsStringAsync("token", token);


            return response;
        }

      
    }
}
