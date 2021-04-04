
namespace Netaba.Web.ViewModels
{
    public class StatusCodeViewModel
    {
        public int ErrorStatusCode { get; }

        public StatusCodeViewModel(int code) => ErrorStatusCode = code;
    }
}
