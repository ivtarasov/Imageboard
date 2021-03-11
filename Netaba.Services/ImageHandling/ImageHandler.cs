using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Threading.Tasks;
using ImageModel = Netaba.Data.Models.Image;

namespace Netaba.Services.ImageHandling
{
    public class ImageHandler: IImageHandler
    {
        /* 
         * Supported formats: Jpeg, Png, Bmp, Gif, Tga
        */
        public async Task<ImageModel> HandleImageAsync(IFormFile file, string webRootPath)
        {
            if (file == null) return null;

            using var formFileStream = file.OpenReadStream();
            var (imageInfo, imageFormat) = await Image.IdentifyWithFormatAsync(formFileStream);

            if (imageFormat == null || imageFormat == null) throw new UnknownImageFormatException("Inknown image format.");

            double h = imageInfo.Height;
            double w = imageInfo.Width;
            double tmp = 200.0;
            if (imageInfo.Height > tmp || imageInfo.Width > tmp)
            {
                if (imageInfo.Height > imageInfo.Width)
                {
                    h = tmp;
                    w = (tmp / imageInfo.Height) * imageInfo.Width;
                }
                else
                {
                    w = tmp;
                    h = (tmp / imageInfo.Width) * imageInfo.Height;
                }
            }

            string path = "/src/Images/" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            formFileStream.Seek(0, SeekOrigin.Begin);

            using var fs = File.Create(webRootPath + path);
            await formFileStream.CopyToAsync(fs);

            return new ImageModel(path, file.FileName, imageFormat.Name, SizeReformer.ToReadableForm(file.Length), imageInfo.Height, imageInfo.Width, (int)h, (int)w);
        }
    }
}
