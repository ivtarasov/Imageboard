using Netaba.Data.Models;
using System.Threading.Tasks;

namespace Netaba.Services.Markup
{
    public static class PostParserExtension
    {
        public static async Task ParseMessageAsync(this Post post, IParser parser, string boardName) => 
            post.Message = await parser.ToHtmlAsync(post.Message, boardName);
    }
}
