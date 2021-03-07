
namespace Netaba.Web.ViewModels
{
    public class ErrorViewModel
    {
        public string ExceptionMessage { get; private set; }

        public ErrorViewModel(string exceptionMessage) => ExceptionMessage = exceptionMessage;
    }
}
