using System;
using System.Collections.Generic;
using System.Text;

namespace Imageboard.Markup
{
    class CharToMarkMapper
    {
        public static Marks Map(char value)
        {
            switch (value)
            {
                case '\'':
                    return Marks.Code;
                case '*':
                    return Marks.Bold;
                case '_':
                    return Marks.Italic;
                case '#':
                    return Marks.Spoler;
                default:
                    return Marks.None;
            }
        }
    }
}
