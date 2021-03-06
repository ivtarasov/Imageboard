
namespace Netaba.Services.Markup
{
    public static class Mapper
    {
        static public Mark ToMark((char, char) mark, bool isNewLine) =>
            mark switch
            {
                ('`', _) => Mark.Monospace,
                ('*', _) => Mark.Bold,
                ('_', _) => Mark.Italic,
                ('#', _) => Mark.Spoler,
                ('\n', _) => Mark.NewLine,
                ('>', '>') => Mark.Link,
                ('№', _) when isNewLine => Mark.OList,
                ('+', _) when isNewLine => Mark.UnList,
                ('>', _) when isNewLine => Mark.Quote,
                _ => Mark.None,
            };

        static public string ToOpeningHtml(Mark value) =>
            value switch
            {
                Mark.Monospace => "<code>",
                Mark.Bold => "<b>",
                Mark.Italic => "<i>",
                Mark.Spoler => "<span class=\"spoler\">",

                Mark.UnList => "<ul>",
                Mark.OList => "<ol>",
                Mark.ListElem => "<li>",
                Mark.Quote => "<span class=\"quote\">&gt;",

                Mark.Edge => "",
                _ => "!"
            };

        static public string ToClosingHtml(Mark value) =>
            value switch
            {
                Mark.Monospace => "</code>",
                Mark.Bold => "</b>",
                Mark.Italic => "</i>",
                Mark.Spoler => "</span>",

                Mark.UnList => "</ul>",
                Mark.OList => "</ol>",
                Mark.ListElem => "</li>",
                Mark.Quote => "</span>",

                Mark.Edge => "",
                _ => "!"
            };

        static public string HtmlForLink(int boardId, int treadId, int postId) => $"<a href=\"{boardId}/{treadId}/#{postId}\">&gt;&gt;{postId}</a>";
        static public string BlankLink() => "&gt;&gt;";
        static public string NewLine() => "<br>";
    }
}
