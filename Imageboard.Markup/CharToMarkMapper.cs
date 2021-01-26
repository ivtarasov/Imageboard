using System;
using System.Collections.Generic;
using System.Text;

namespace Imageboard.Markup
{
    class CharToMarkMapper
    {
        public static Mark Map(char value)
        {
            return value switch
            {
                '`' => Mark.Code,
                '*' => Mark.Bold,
                '_' => Mark.Italic,
                '#' => Mark.Spoler,

                '\n' => Mark.NextLine,
                '№' => Mark.OList,
                '+' => Mark.UnList,
                '>' => Mark.Quote,

                '|' => Mark.End,
                _ => Mark.None
            };
        }
    }
}
