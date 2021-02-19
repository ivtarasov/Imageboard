
namespace Imageboard.Services.Markup
{
    public interface IMapper
    {
        public Mark ToMark((char, char) mark, bool isNewLine);
        public string ToOpeningHtml(Mark value);
        public string ToClosingHtml(Mark value);

        public string HtmlForLink(string href, int postId) => $"<a href=\"{href}\">&gt;&gt;{postId}</a>";
        public string BlankLink() => "&gt;&gt;";
        public string NewLine() => "<br>";
    }
}
