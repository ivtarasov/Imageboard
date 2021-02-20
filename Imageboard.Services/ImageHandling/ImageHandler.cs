using EntetyImage = Imageboard.Data.Enteties.Image;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.IO;
using System;

namespace Imageboard.Services.ImageHandling
{
    public class ImageHandler: IImageHandler
    {
        public EntetyImage HandleImage(IFormFile file, string webRootPath)
        {
            if (file != null && file.ContentType.Split("/")[0] == "image")
            {
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
                return new EntetyImage(path, file.FileName, format.Name, (int)file.Length, image.Height, image.Width, (int)h, (int)w);
            } 
            else
            {
                return null;
            }
        }
    }
}
