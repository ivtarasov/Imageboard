using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Netaba.Services.Repository;
using System.Threading.Tasks;

namespace Netaba.Web.Infrastructure.Filters
{
    public class NavBarFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is Controller controller)
            {
                var repository = (IBoardRepository)context.HttpContext.RequestServices.GetService(typeof(IBoardRepository));
                var boardNames = await repository.GetBoardNamesAsync();
                controller.ViewBag.BoardNames = boardNames;
            }
            await next();
        }
    }
}
