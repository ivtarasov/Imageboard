using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imageboard.Markup
{
    public static class Parser
    {
        public static string MarkUp(string value)
        {
            var mstack = new Stack<Mark>();
            var stringBuilder = new StringBuilder();
            for(var i = 0; i < value.Length; i++)
            {

                var mappedValue = CharToMarkMapper.Map(value[i]);
                if (mappedValue == Mark.NextLine)
                {

                }
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
