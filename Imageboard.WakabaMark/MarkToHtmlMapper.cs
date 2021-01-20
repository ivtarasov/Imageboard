using System;
using System.Collections.Generic;
using System.Text;

namespace Imageboard.WakabaMark
{
    class MarkToHtmlMapper
    {
        // временный вид
        public static char Map(Mark value)
        {
            switch (value)
            {
                case Mark.Code:
                    return '[';
                case Mark.Bold:
                    return '{';
                case Mark.Italic:
                    return '(';
                case Mark.Spoler:
                    return '<';
                default:
                    return '!';
            }
        }
        public static char SecondMap(Mark value)
        {
            switch (value)
            {
                case Mark.Code:
                    return ']';
                case Mark.Bold:
                    return '}';
                case Mark.Italic:
                    return ')';
                case Mark.Spoler:
                    return '>';
                default:
                    return '!';
            }
        }
    }
}
