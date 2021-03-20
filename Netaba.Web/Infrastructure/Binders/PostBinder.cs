using ImageModel = Netaba.Data.Models.Image;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;
using Netaba.Data.Models;
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

            var messageValue = bindingContext.ValueProvider.GetValue("Post.Message");
            var titleValue = bindingContext.ValueProvider.GetValue("Post.Title");
            var isSageValue = bindingContext.ValueProvider.GetValue("Post.IsSage");
            var passwordValue = bindingContext.ValueProvider.GetValue("Post.Password");
            var formFile = bindingContext.ActionContext.HttpContext.Request.Form.Files.GetFile("Post.Image");
            var treadIdValue = bindingContext.ValueProvider.GetValue("TreadId");

            string message = messageValue.FirstValue;
            string title = titleValue.FirstValue;

            _ = bool.TryParse(isSageValue.FirstValue, out bool isSage);

            bool isOp = false;
            if (treadIdValue == ValueProviderResult.None) isOp = true;

            string password = passwordValue.FirstValue;
            string ip = bindingContext.HttpContext.Connection.RemoteIpAddress?.ToString();

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

            bindingContext.Result = ModelBindingResult.Success(new Post(bindingContext.HttpContext.User.Identity?.Name, message, title, DateTime.Now, ip, password, image, isOp, isSage));
        }
    }
}
