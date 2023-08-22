namespace Aether.Client.Services.AuthService
{
    public interface IAuthService
    {
         Task<HttpResponseMessage> Login(UserDto creds);
        
    }
}
