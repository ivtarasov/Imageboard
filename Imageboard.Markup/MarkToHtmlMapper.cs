using System;
using System.Collections.Generic;
using System.Text;

namespace Imageboard.Markup
{
    class MarkToHtmlMapper
    {
        public static string MapToOpeningElem(Mark value)
        {
            return value switch
            {
                Mark.Monospace => "<code>",
                Mark.Bold => "<b>",
                Mark.Italic => "<i>",
                Mark.Spoler => "<span>",

                Mark.UnList => "<ul>",
                Mark.OList => "<ol>",
                Mark.ListElem => "<li>",
                Mark.Quote => "<span style=\"color: blue;\">&gt;",

                Mark.NewLine => "<br>",
                Mark.End => "<article>",
                _ => "!"
            };
        }

        public static string MapToClosingElem(Mark value)
        {
            return value switch
            {
                Mark.Monospace => "</code>",
                Mark.Bold => "</b>",
                Mark.Italic => "</i>",
                Mark.Spoler => "</span>",

                Mark.UnList => "</ul>",
                Mark.OList => "</ol>",
                Mark.ListElem => "</li>",
                Mark.Quote => "</span><br>",

                Mark.NewLine => "</br>",
                Mark.End => "</article>",
                _ => "!"
            };
        }

        public static string HtmlForLink(string href, int postId)
        {
            return $"<a href=\"{href}\">&gt;&gt;{postId}</a><br>";
        }
    }
}
