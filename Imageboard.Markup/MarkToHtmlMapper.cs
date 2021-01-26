using System;
using System.Collections.Generic;
using System.Text;

namespace Imageboard.Markup
{
    class MarkToHtmlMapper
    {
        public static string Map(Mark value)
        {
            return value switch
            {
                Mark.Code => "[",
                Mark.Bold => "{",
                Mark.Italic => "(",
                Mark.Spoler => "<",

                Mark.UnList => "UNL",
                Mark.OList => "OL",
                Mark.Quote => "Q",
                Mark.Link => "LI",

                Mark.NextLine => "\n",
                Mark.End => "|",
                _ => "!"
            };
        }
        public static string SecondMap(Mark value)
        {
            return value switch
            {
                Mark.Code => "]",
                Mark.Bold => "}",
                Mark.Italic => ")",
                Mark.Spoler => ">",

                Mark.UnList => "UNL",
                Mark.OList => "OL",
                Mark.Quote => "Q",
                Mark.Link => "LI",

                Mark.NextLine => "\n",
                Mark.End => "|",
                _ => "!"
            };
        }
    }
}
