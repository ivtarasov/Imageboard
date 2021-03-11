using System.Threading.Tasks;

namespace Netaba.Services.Markup
{
    public interface IParser
    {
        public Task<string> ToHtmlAsync(string sourse, string boardName);
    }
}