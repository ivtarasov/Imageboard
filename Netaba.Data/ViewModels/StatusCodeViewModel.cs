
namespace Netaba.Data.ViewModels
{
    public class StatusCodeViewModel
    {
        public int ErrorStatusCode { get; }

        public StatusCodeViewModel(int code) => ErrorStatusCode = code;
    }
}
