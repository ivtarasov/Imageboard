using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Netaba.Web.ViewModels;

namespace Netaba.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error")]
        public IActionResult Error()
        {
            var ExceptionMessage = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error?.Message;
            return View(new ErrorViewModel(ExceptionMessage));
        }

        [Route("/StatusCode")]
        public new IActionResult StatusCode(int code)
        {
            if (code == 0) return NotFound();
            return View(new StatusCodeViewModel(code));
        }
    }
}
