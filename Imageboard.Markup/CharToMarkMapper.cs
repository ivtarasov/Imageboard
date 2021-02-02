using System;
using System.Collections.Generic;
using System.Text;

namespace Imageboard.Markup
{
    class CharToMarkMapper
    {
        public static Mark Map(char value, bool isNewLine)
        {
            return value switch
            {
                '`' => Mark.Monospace,
                '*' => Mark.Bold,
                '_' => Mark.Italic,
                '#' => Mark.Spoler,

                '\n' => Mark.NewLine,

                '№' when isNewLine => Mark.OList,
                '+' when isNewLine => Mark.UnList,
                '>' when isNewLine => Mark.Quote,
                /* Double quoting is the Mark.Link. 
                 * It's checked in the parser.  
                 */

                 _ => Mark.None
            };
        }
    }
}
