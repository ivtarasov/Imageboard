using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;
using Netaba.Services.Pass;

namespace Netaba.Web.Infrastructure.Binders
{
    public class PassHashBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            byte[] result;
            var passwordValue = bindingContext.ValueProvider.GetValue("Post.PassHash");
            string ip = bindingContext.HttpContext.Connection.RemoteIpAddress.ToString();

            if (passwordValue == ValueProviderResult.None)
            {
                result = HashGenerator.GetHash(ip, "12345");
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                result = HashGenerator.GetHash(ip, passwordValue.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(result);
            }

            return Task.CompletedTask;
        }
    }
}
