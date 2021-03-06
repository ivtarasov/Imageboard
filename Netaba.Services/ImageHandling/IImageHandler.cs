using Netaba.Data.Models;
using Microsoft.AspNetCore.Http;

namespace Netaba.Services.ImageHandling
{
    public interface IImageHandler
    {
        public Image HandleImage(IFormFile file, string webRootPath);
    }
}