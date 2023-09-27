using Aether.Client.Services.AdminService;


namespace Aether.Client.ClientModels
{
    public class AppStateManager
    {

        private readonly IAdminService _admin;
        public User selectedUser;
        public User user = new();
        public event Action OnStateChange;
        public string _selectedUserName;
        public string userName;
        
        public AppStateManager(IAdminService admin)
        {
            _admin = admin;
        }

        //public string UpdateState(string _selectedUserName)
        //{
        //    selectedUser = _admin.Users[0];
        //    if (GetUserName(userName) == selectedUser.UserName)
        //    {
        //        _selectedUserName = selectedUser.UserName;
        //        NotifyStateChanged();
        //    }

        //    return this._selectedUserName;
        //}
        //public string SetUsers()
        //{
        //    return this._selectedUserName;
        //}

        //public string GetUserName(string userName)
        //{
        //    _selectedUserName = userName;
        //    UpdateState(_selectedUserName);
            
        //    return UpdateState(_selectedUserName);
        //}

        public User UpdateState(string userName)
        {
            _selectedUserName = userName;
            selectedUser = _admin.Users.FirstOrDefault(u => u.UserName == _selectedUserName);

            if (selectedUser != null)
            {
                user.UserName = selectedUser.UserName;
                user.Name = selectedUser.Name;
                NotifyStateChanged();
            }

            return user;
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
