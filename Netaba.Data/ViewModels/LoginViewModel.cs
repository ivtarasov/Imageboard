using Netaba.Data.Models;

namespace Netaba.Data.ViewModels
{
    public class LoginViewModel
    {
        public Login Login { get; }

        public LoginViewModel(Login login) => Login = login;
    }
}
