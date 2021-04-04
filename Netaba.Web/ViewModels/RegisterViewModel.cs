using Netaba.Data.Models;

namespace Netaba.Web.ViewModels
{
    public class RegisterViewModel
    {
        public Register Register { get; }

        public RegisterViewModel(Register register) => Register = register;
    }
}
