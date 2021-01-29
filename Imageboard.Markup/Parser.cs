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
            var result = new StringBuilder();
            for(var i = 0; i < value.Length; i++)
            {
                Mark mappedValue;
                if (i == 0)
                {
                    mappedValue = Mark.NextLine;
                }
                else
                {
                    mappedValue = CharToMarkMapper.Map(value[i]);
                }
                var mappedValue = CharToMarkMapper.Map(value[i]);
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
                            HandleMark(mappedValue, mstack, result);
                            continue;
                        case Mark.Link:
                            continue;
                        case Mark.OList:
                            continue;
                        case Mark.UnList:
                            continue;
                        default:
                            continue;
                    }
                }
                if (mappedValue != Mark.None)
                {
                    HandleMark(mappedValue, mstack, result);
                }
            }
            return result.ToString();
        }
        static private void HandleMark(Mark value, Stack<Mark> mstack, StringBuilder result)
        {
            if (mstack.Contains(value))
            {
                Mark mark;
                var tmpstack = new Stack<Mark>();
                while ((mark = mstack.Pop()) != value)
                {
                    CloseMarkAndPushToStack(result, tmpstack, mark);
                }
                result.Append(MarkToHtmlMapper.MapToClosingElem(value));
                if (value != Mark.End)
                {
                    while (tmpstack.TryPop(out mark))
                    {
                        OpenMarkAndPushToStack(result, mstack, mark);
                    }
                }
            }
            else
            {
                OpenMarkAndPushToStack(result, mstack, value);
            }
        }
        static private void OpenMarkAndPushToStack(StringBuilder result, Stack<Mark> stack, Mark value)
        {
            result.Append(MarkToHtmlMapper.MapToOpeningElem(value));
            stack.Push(value);
        }
        static private void CloseMarkAndPushToStack(StringBuilder result, Stack<Mark> stack, Mark value)
        {
            result.Append(MarkToHtmlMapper.MapToClosingElem(value));
            stack.Push(value);
        }
    }
}
