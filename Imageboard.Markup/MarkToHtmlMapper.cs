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
                Mark.Monospace => "[",
                Mark.Bold => "{",
                Mark.Italic => "(",
                Mark.Spoler => "<",

                Mark.UnList => "UNL",
                Mark.OList => "OL",
                Mark.ListElem => "le",
                Mark.Quote => "Q",
                Mark.Link => "LI",

                Mark.NewLine => "\n",
                Mark.End => "|",
                _ => "!"
            };
        }

        public static string MapToClosingElem(Mark value)
        {
            return value switch
            {
                Mark.Monospace => "]",
                Mark.Bold => "}",
                Mark.Italic => ")",
                Mark.Spoler => ">",

                Mark.UnList => "UNL",
                Mark.OList => "OL",
                Mark.ListElem => "le",
                Mark.Quote => "Q",
                Mark.Link => "LI",

                Mark.NewLine => "\n",
                Mark.End => "|",
                _ => "!"
            };
        }
    }
}
