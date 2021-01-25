using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imageboard.Markup
{
    public static class Markup
    {
        public static string MakeMarkup(string sourse)
        {
            var value = sourse.ToCharArray();
            return MarkUp(value);
        }
        private static string MarkUp(char[] value)
        {
            var mstack = new Stack<Mark>();
            var stringBuilder = new StringBuilder();
            for(var i = 0; i < value.Length; i++)
            {
                var mappedValue = CharToMarkMapper.Map(value[i]);
                if (mappedValue != Mark.None)
                {
                    if (mstack.Contains(mappedValue))
                    {
                        Mark mark;
                        var tmpstack = new Stack<Mark>();
                        while ((mark = mstack.Pop()) != mappedValue)
                        {
                                stringBuilder.Append(MarkToHtmlMapper.SecondMap(mark));
                                tmpstack.Push(mark);
                        }
                        stringBuilder.Append(MarkToHtmlMapper.SecondMap(mappedValue));
                        while (tmpstack.TryPop(out mark))
                        {
                            stringBuilder.Append(MarkToHtmlMapper.Map(mark));
                            mstack.Push(mark);
                        }
                    } 
                    else
                    {
                        mstack.Push(mappedValue);
                        stringBuilder.Append(MarkToHtmlMapper.Map(mappedValue));
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }
}
