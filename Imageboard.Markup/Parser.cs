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
                var mappedValue = CharToMarkMapper.Map(value[i]);
                if (mappedValue == Mark.NewLine || i == 0)
                {
                    if (i != 0)
                    {
                        result.Append(value[i]);
                        mappedValue = CharToMarkMapper.Map(value[++i]);

                        CheckForNewLineMarksAndParseIt(mstack, result);
                    }

                    switch (mappedValue)
                    {
                        case Mark.Quote:

                            if (CharToMarkMapper.Map(value[i+1]) == Mark.Quote) 
                            {
                                i++;
                                goto case Mark.Link;
                            }

                            ParseMark(mstack, result, mappedValue);
                            continue;

                        case Mark.Link:
                            continue;

                        case Mark.OList:
                            continue;

                        case Mark.UnList:
                            continue;

                        default:
                            break;
                    }
                }

                if (mappedValue != Mark.None)
                {
                    ParseMark(mstack, result, mappedValue);
                } else
                {
                    result.Append(value[i]);
                }
            }

            return result.ToString();
        }

        static private void ParseMark(Stack<Mark> mstack, StringBuilder result, Mark value)
        {
            if (mstack.Contains(value))
            {
                HadleMark(mstack, result, value);
            }
            else
            {
                OpenMarkAndPushToStack(result, mstack, value);
            }
        }

        private static void CheckForNewLineMarksAndParseIt(Stack<Mark> mstack, StringBuilder result)
        {
            if (mstack.Contains(Mark.Quote))
                HadleMark(mstack, result, Mark.Quote);
            if (mstack.Contains(Mark.OList))
                HadleMark(mstack, result, Mark.OList);
            if (mstack.Contains(Mark.UnList))
                HadleMark(mstack, result, Mark.UnList);
        }

        private static void HadleMark(Stack<Mark> mstack, StringBuilder result, Mark value)
        {
            Mark mark;
            var tmpstack = new Stack<Mark>();

            while ((mark = mstack.Pop()) != value)
            {
                CloseMarkAndPushToStack(result, tmpstack, mark);
            }

            CloseMark(result, value);

            if (value != Mark.End)
            {
                while (tmpstack.TryPop(out mark))
                {
                    OpenMarkAndPushToStack(result, mstack, mark);
                }
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

        static private void CloseMark(StringBuilder result, Mark value)
        {
            result.Append(MarkToHtmlMapper.MapToClosingElem(value));
        }
    }
}
