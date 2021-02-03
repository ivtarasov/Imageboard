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
                Mark.Quote => "<span>",

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
                Mark.Quote => "</span>",

                Mark.NewLine => "</br>",
                Mark.End => "</article>",
                _ => "!"
            };
        }
    }
}
