using Imageboard.Data.Contexts;

namespace Imageboard.Services.Markup
{
    public interface IParser
    {
        public string ToHtml(string sourse);
    }
}