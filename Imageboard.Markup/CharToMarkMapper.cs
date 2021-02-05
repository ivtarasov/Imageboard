using System;
using System.Collections.Generic;
using System.Text;

namespace Imageboard.Markup
{
    class CharToMarkMapper
    {
        public static Mark Map(string sourse, ref int pos, bool isNewLine)
        {
            (char, char) mark;
            mark = (pos + 1 < sourse.Length) ? (sourse[pos], sourse[pos + 1]): (sourse[pos], '!');
            switch (mark)
            {
                case ('`', _):
                    return Mark.Monospace;
                case ('*', _):
                    return Mark.Bold;
                case ('_', _):
                    return Mark.Italic;
                case ('#', _):
                    return Mark.Spoler;

                case ('\n', _):
                    return Mark.NewLine;
                case ('>', '>'):
                    pos++;
                    return Mark.Link;

                case ('№', _) when isNewLine:
                    return Mark.OList;
                case ('+', _) when isNewLine:
                    return Mark.UnList;
                case ('>', _) when isNewLine:
                    return Mark.Quote;

                default: 
                    return Mark.None;
            };
        }
    }
}
