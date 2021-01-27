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
                /*if (mappedValue == Mark.Quote && CharToMarkMapper.Map(value[i + 1]) == Mark.Quote)
                {
                    i++;
                    mappedValue = Mark.Link;
                }*/
                if (mappedValue == Mark.NextLine)
                {
                    mappedValue = CharToMarkMapper.Map(value[++i]);
                    switch (mappedValue)
                    {
                        case Mark.Quote:
                            if (CharToMarkMapper.Map(value[i + 1]) == Mark.Quote) 
                            {
                                i++;
                                goto case Mark.Link;
                            }
                            break;
                        case Mark.Link:
                            break;
                        case Mark.OList:
                            break;
                        case Mark.UnList:
                            break;
                        default:
                            break;
                    }
                }
                if (mappedValue != Mark.None)
                {
                    if (mstack.Contains(mappedValue))
                    {
                        Mark mark;
                        var tmpstack = new Stack<Mark>();
                        while ((mark = mstack.Pop()) != mappedValue)
                        {
                                stringBuilder.Append(MarkToHtmlMapper.MapToClosingElement(mark));
                                tmpstack.Push(mark);
                        }
                        stringBuilder.Append(MarkToHtmlMapper.MapToClosingElement(mappedValue));
                        if (mappedValue != Mark.End)
                        {
                            while (tmpstack.TryPop(out mark))
                            {
                                stringBuilder.Append(MarkToHtmlMapper.MapToOpeningElement(mark));
                                mstack.Push(mark);
                            }
                        }
                    } 
                    else
                    {
                        mstack.Push(mappedValue);
                        stringBuilder.Append(MarkToHtmlMapper.MapToOpeningElement(mappedValue));
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }
}
