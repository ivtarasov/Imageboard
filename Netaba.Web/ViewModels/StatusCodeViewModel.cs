
namespace Netaba.Web.ViewModels
{
    public class StatusCodeViewModel
    {
        public int ErrorStatusCode { get; set; }

        public StatusCodeViewModel(int code) => ErrorStatusCode = code;
    }
}
