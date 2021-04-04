using Netaba.Data.Models;

namespace Netaba.Web.ViewModels
{
    public class LoginViewModel
    {
        public Login Login { get; }

        public LoginViewModel(Login login) => Login = login;
    }
}
