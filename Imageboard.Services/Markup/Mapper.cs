
namespace Imageboard.Services.Markup
{
    public class Mapper: IMapper
    {
        public Mark ToMark((char, char) mark, bool isNewLine) =>
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

        public string ToOpeningHtml(Mark value) =>
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

                Mark.Edge => "",
                _ => "!"
            };

        public string ToClosingHtml(Mark value) =>
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
    }
}
