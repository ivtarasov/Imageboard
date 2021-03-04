using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Netaba.Web.Infrastructure.Binders
{
    public class TimeBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            bindingContext.Result = ModelBindingResult.Success(DateTime.Now);
            return Task.CompletedTask;
        }
    }
}