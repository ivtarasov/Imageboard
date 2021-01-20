using System;
using System.Collections.Generic;
using System.Text;

namespace Imageboard.Markup
{
    class MarkToHtmlMapper
    {
        // временный вид
        public static char Map(Marks value)
        {
            switch (value)
            {
                case Marks.Code:
                    return '[';
                case Marks.Bold:
                    return '{';
                case Marks.Italic:
                    return '(';
                case Marks.Spoler:
                    return '<';
                default:
                    return '!';
            }
        }
        public static char SecondMap(Marks value)
        {
            switch (value)
            {
                case Marks.Code:
                    return ']';
                case Marks.Bold:
                    return '}';
                case Marks.Italic:
                    return ')';
                case Marks.Spoler:
                    return '>';
                default:
                    return '!';
            }
        }
    }
}
