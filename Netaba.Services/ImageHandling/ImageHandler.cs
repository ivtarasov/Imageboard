using ImageModel = Netaba.Data.Models.Image;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.IO;
using System;

namespace Netaba.Services.ImageHandling
{
    public class ImageHandler: IImageHandler
    {
        /* 
         * Supported formats: Jpeg, Png, Bmp, Gif, Tga
        */
        public ImageModel HandleImage(IFormFile file, string webRootPath)
        {
            if (file == null) return null;

            string path = "/src/Images/" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            using var fileStream = file.OpenReadStream();
            using var image = Image.Load(fileStream, out IImageFormat format);

            double h = image.Height;
            double w = image.Width;
            double tmp = 200.0;
            if (image.Height > tmp || image.Width > tmp)
            {
                if (image.Height > image.Width)
                {
                    h = tmp;
                    w = (tmp / image.Height) * image.Width;
                }
                else
                {
                    w = tmp;
                    h = (tmp / image.Width) * image.Height;
                }
            }

            image.Save(webRootPath + path);
            return new ImageModel(path, file.FileName, format.Name, SizeReformer.ToReadableForm(file.Length), image.Height, image.Width, (int)h, (int)w);
        }
    }
}
