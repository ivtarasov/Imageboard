using System;
using System.Collections.Generic;
using System.Text;

namespace Imageboard.WakabaMark
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
                default:
                    return Mark.None;
            }
        }
    }
}
