
namespace Netaba.Data.ViewModels
{
    public class DeleteAdminViewModel
    {
        public string AdminName { get; }

        public DeleteAdminViewModel(string adminName) => AdminName = adminName;
    }
}