using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Netaba.Data.ViewModels;

namespace Netaba.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Exeption", Name = "Exeption")]
        public IActionResult Exeption()
        {
            var ExceptionMessage = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error?.Message;
            return View(new ErrorViewModel(ExceptionMessage));
        }

        [Route("/StatusCode", Name = "StatusCode")]
        public IActionResult StatusCode2(int code)
        {
            return View(new StatusCodeViewModel(code));
        }
    }
}
