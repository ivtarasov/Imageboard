
namespace Imageboard.Markup
{
    class Mapper
    {
        public static Mark ToMark(string sourse, ref int pos, bool isNewLine)
        {
            (char, char) mark;
            mark = (pos + 1 < sourse.Length) ? (sourse[pos], sourse[pos + 1]) : (sourse[pos], '!');
            switch (mark)
            {
                case ('`', _):
                    return Mark.Monospace;
                case ('*', _):
                    return Mark.Bold;
                case ('_', _):
                    return Mark.Italic;
                case ('#', _):
                    return Mark.Spoler;

                case ('\n', _):
                    return Mark.NewLine;
                case ('>', '>'):
                    pos++;
                    return Mark.Link;

                case ('№', _) when isNewLine:
                    return Mark.OList;
                case ('+', _) when isNewLine:
                    return Mark.UnList;
                case ('>', _) when isNewLine:
                    return Mark.Quote;

                default:
                    return Mark.None;
            };
        }

        public static string ToOpeningHtml(Mark value) =>
            value switch
            {
                Mark.Monospace => "<code>",
                Mark.Bold => "<b>",
                Mark.Italic => "<i>",
                Mark.Spoler => "<span>",

                Mark.UnList => "<ul>",
                Mark.OList => "<ol>",
                Mark.ListElem => "<li>",
                Mark.Quote => "<span style=\"color: #789922;\">&gt;",

                Mark.End => "<article>",
                _ => "!"
            };

        public static string ToClosingHtml(Mark value) =>
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

                Mark.End => "</article>",
                _ => "!"
            };

        public static string HtmlForLink(string href, int postId) => $"<a href=\"{href}\">&gt;&gt;{postId}</a>";

        public static string BlankLink() => "&gt;&gt;";

        public static string NewLine() => "<br>";
    }
}
