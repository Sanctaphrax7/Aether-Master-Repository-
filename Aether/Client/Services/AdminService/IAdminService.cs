namespace Aether.Client.Services.AdminService
{
    public interface IAdminService
    {
      
        List<User> Users { get; set; }
        List<Role> Roles { get; set; }
        Task GetUserList();
        Task GetRoleList();
        Task<HttpResponseMessage> AddUser(User user);
        Task<HttpResponseMessage> UpdateUser(User user);

    }
}
