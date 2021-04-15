using System.ComponentModel.DataAnnotations;

namespace Netaba.Web.ViewModels
{
    public class DeleteAdminViewModel
    {
        [Required(ErrorMessage = "Name is not specified.")]
        public string AdminName { get; }

        public DeleteAdminViewModel(string adminName) => AdminName = adminName;
    }
}