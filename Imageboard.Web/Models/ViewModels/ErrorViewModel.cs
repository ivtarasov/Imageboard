
namespace Imageboard.Web.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; init; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
