using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imageboard.Markup
{
    public static class WakabaMark
    {
        public static string MakeMarkup(string sourse)
        {
            var value = sourse.ToCharArray();
            return MarkUp(value);

        }
        private static string MarkUp(char[] value)
        {
            var mstack = new Stack<Mark>();
            var sb = new StringBuilder();
            for(var i = 0; i < value.Length; i++)
            {
                var mappedValue = CharToMarkMapper.Map(value[i]);
                if (mappedValue != Mark.None)
                {
                    if (mstack.Contains(mappedValue))
                    {
                        var m = mstack.Pop();
                        while (true)
                        {
                            if (m == mappedValue)
                            {
                                sb.Append(MarkToHtmlMapper.SecondMap(mappedValue));
                                break;
                            } 
                            else
                            {
                                sb.Append(MarkToHtmlMapper.SecondMap(mappedValue));
                            }
                            m = mstack.Pop();
                        }
                    } 
                    else
                    {
                        mstack.Push(mappedValue);
                        sb.Append(MarkToHtmlMapper.Map(mappedValue));
                    }
                }
            }
            return sb.ToString();
        }
    }
}
