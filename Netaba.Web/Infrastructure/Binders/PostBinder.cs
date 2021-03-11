using ImageModel = Netaba.Data.Models.Image;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;
using Netaba.Data.Models;
using Netaba.Services.Pass;
using Netaba.Services.ImageHandling;
using SixLabors.ImageSharp;

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

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var messageValue = bindingContext.ValueProvider.GetValue("message");
            var titleValue = bindingContext.ValueProvider.GetValue("title");
            var isOpValue = bindingContext.ValueProvider.GetValue("isop");
            var isSageValue = bindingContext.ValueProvider.GetValue("issage");
            var passHashValue = bindingContext.ValueProvider.GetValue("pass");
            var formFile = bindingContext.ActionContext.HttpContext.Request.Form.Files.GetFile("file");

            string message = messageValue.FirstValue;
            string title = titleValue.FirstValue;

            _ = bool.TryParse(isOpValue.FirstValue, out bool isOp);
            _ = bool.TryParse(isSageValue.FirstValue, out bool isSage);

            byte[] passHash = HashGenerator.GetHash(bindingContext.HttpContext.Connection.RemoteIpAddress?.ToString(), passHashValue.FirstValue ?? "12345");

            ImageModel image = null;
            try
            {
                image = await _imageHandler.HandleImageAsync(formFile, _appEnvironment.WebRootPath);
            }
            catch(UnknownImageFormatException)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Unknown image format. Supported formats: Jpeg, Png, Bmp, Gif, Tga.");
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Unable to upload image.");
            }

            bindingContext.Result = ModelBindingResult.Success(new Post(message, title, DateTime.Now, image, isOp, isSage, passHash));
        }
    }
}
