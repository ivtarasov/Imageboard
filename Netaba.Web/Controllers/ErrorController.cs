using Microsoft.AspNetCore.Mvc;
using Netaba.Web.ViewModels;

namespace Netaba.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error")]
        public IActionResult Error()
        {
            return View();
        }

        [Route("/StatusCode")]
        public new IActionResult StatusCode(int code)
        {
            if (code == 0) return NotFound();
            return View(new StatusCodeViewModel(code));
        }
    }
}
