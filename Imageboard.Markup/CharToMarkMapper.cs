using System;
using System.Collections.Generic;
using System.Text;

namespace Imageboard.Markup
{
    class CharToMarkMapper
    {
        public static Mark Map(char value)
        {
            switch (value)
            {
                case '\'':
                    return Mark.Code;
                case '*':
                    return Mark.Bold;
                case '_':
                    return Mark.Italic;
                case '#':
                    return Mark.Spoler;
                case '|':
                    return Mark.End;
                default:
                    return Mark.None;
            }
        }
    }
}
