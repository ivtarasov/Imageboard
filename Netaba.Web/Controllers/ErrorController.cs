using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Netaba.Web.ViewModels;

namespace Netaba.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/exeption", Name = "Exeption")]
        public IActionResult Exeption()
        {
            var ExceptionMessage = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error?.Message;
            return View(new ErrorViewModel(ExceptionMessage));
        }

        [Route("/code", Name = "StatusCode")]
        public IActionResult Code(int code)
        {
            return View(new StatusCodeViewModel(code));
        }
    }
}
