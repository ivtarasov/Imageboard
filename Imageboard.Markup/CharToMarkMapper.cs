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
                '`' => Mark.Monospace,
                '*' => Mark.Bold,
                '_' => Mark.Italic,
                '#' => Mark.Spoler,

                '\n' => Mark.NextLine,
                '№' => Mark.OList,
                '+' => Mark.UnList,
                '>' => Mark.Quote,
                /* Double quoting is the Mark.Link. 
                 * It's checked in the parser.  
                 */

                '|' => Mark.End,
                 _ => Mark.None
            };
        }
    }
}
