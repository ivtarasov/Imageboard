using Imageboard.Data.Enteties;
using Microsoft.AspNetCore.Http;

namespace Imageboard.Services.ImageHandling
{
    public interface IImageHandler
    {
        public Image HandleImage(IFormFile file, string webRootPath);
    }
}