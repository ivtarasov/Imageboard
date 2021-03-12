using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Netaba.Web.ViewModels;
using System;

namespace Netaba.Web.Controllers
{
    public class ErrorsController : Controller
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
