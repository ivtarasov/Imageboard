
namespace Netaba.Web.ViewModels
{
    public class DeleteAdminViewModel
    {
        public string AdminName { get; }

        public DeleteAdminViewModel(string adminName) => AdminName = adminName;
    }
}