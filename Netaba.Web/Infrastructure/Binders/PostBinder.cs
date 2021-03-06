using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;
using Netaba.Data.Models;
using Netaba.Services.Pass;
using Netaba.Services.ImageHandling;

namespace Netaba.Web.Infrastructure.Binders
{
    public class PostBinder : IModelBinder
    {
        private readonly IImageHandler _imageHandler;
        private readonly IWebHostEnvironment _appEnvironment;

        public PostBinder(IImageHandler imageHandler, IWebHostEnvironment appEnvironment)
        {
            _imageHandler = imageHandler;
            _appEnvironment = appEnvironment;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var messageValue = bindingContext.ValueProvider.GetValue("mssage");
            var titleValue = bindingContext.ValueProvider.GetValue("title");
            var isOpValue = bindingContext.ValueProvider.GetValue("isop");
            var isSageValue = bindingContext.ValueProvider.GetValue("issage");
            var passHashValue = bindingContext.ValueProvider.GetValue("pass");
            var formFile = bindingContext.ActionContext.HttpContext.Request.Form.Files.GetFile("file");

            string message = messageValue.FirstValue;
            string title = titleValue.FirstValue;

            bool.TryParse(isOpValue.FirstValue, out bool isOp);
            bool.TryParse(isSageValue.FirstValue, out bool isSage);

            byte[] passHash = HashGenerator.GetHash(bindingContext.HttpContext.Connection.RemoteIpAddress?.ToString(), passHashValue.FirstValue ?? "12345");
            Image image = _imageHandler.HandleImage(formFile, _appEnvironment.WebRootPath);

            bindingContext.Result = ModelBindingResult.Success(new Post(message, title, DateTime.Now, image, isOp, isSage, passHash));
            return Task.CompletedTask;
        }
    }
}
