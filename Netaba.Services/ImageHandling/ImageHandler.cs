using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Threading.Tasks;
using ImageModel = Netaba.Models.Image;

namespace Netaba.Services.ImageHandling
{
    public class ImageHandler: IImageHandler
    {
         // Supported formats: Jpeg, Png, Bmp, Gif, Tga.
        public async Task<ImageModel> HandleImageAsync(IFormFile file, string webRootPath)
        {
            if (file == null) return null;

            using var formFileStream = file.OpenReadStream();
            var (imageInfo, imageFormat) = await Image.IdentifyWithFormatAsync(formFileStream);

            if (imageFormat == null || imageFormat == null) throw new UnknownImageFormatException("Unknown image format.");

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

            var dirForImages = "/images/";
            var fullDirectoryPath = webRootPath + dirForImages;
            Directory.CreateDirectory(fullDirectoryPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = dirForImages + fileName;
            var fullFilePath = fullDirectoryPath + fileName;

            using var fs = File.Create(fullFilePath);

            formFileStream.Seek(0, SeekOrigin.Begin);
            await formFileStream.CopyToAsync(fs);

            return new ImageModel(filePath, file.FileName, imageFormat.Name, SizeReformer.ToReadableForm(file.Length), imageInfo.Height, imageInfo.Width, (int)h, (int)w);
        }
    }
}
