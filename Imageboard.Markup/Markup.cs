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
            var val = sourse.ToCharArray();
            return MarkUp(val);

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
                        var mark = mstack.Pop();
                        var tmpstack = new Stack<Mark>();
                        while (true)
                        {
                            if (mark == mappedValue)
                            {
                                stringBuilder.Append(MarkToHtmlMapper.SecondMap(mappedValue));
                                break;
                            } 
                            else
                            {
                                stringBuilder.Append(MarkToHtmlMapper.SecondMap(mappedValue));
                            }
                            mark = mstack.Pop();
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
