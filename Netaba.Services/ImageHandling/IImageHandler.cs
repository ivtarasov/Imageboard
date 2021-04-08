using Netaba.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Netaba.Services.ImageHandling
{
    public interface IImageHandler
    {
        public Task<Image> HandleImageAsync(IFormFile file, string webRootPath);
    }
}