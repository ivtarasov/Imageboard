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
                var isFirstChar = i == 0;
                var mappedValue = CharToMarkMapper.Map(value[i], isFirstChar);
                if (mappedValue == Mark.NewLine || isFirstChar)
                {
                    if (HadleNewLine(value, mstack, result, ref i, ref mappedValue)) continue;
                }

                if (mappedValue != Mark.None)
                {
                    HandleMark(mstack, result, mappedValue);
                } 
                else
                {
                    result.Append(value[i]);
                }
            }

            return result.ToString();
        }

        static private bool HadleNewLine(string sourse, Stack<Mark> mstack, StringBuilder result, 
                                         ref int position, ref Mark value)
        {
            if (position != 0)
            {
                CheckForNewLineMarksInStack(mstack, result);

                result.Append(sourse[position]);
                value = CharToMarkMapper.Map(sourse[++position], true);

            }

            switch (value)
            {
                case Mark.Quote:

                    if (CharToMarkMapper.Map(sourse[position + 1], true) == Mark.Quote)
                    {
                        position++;
                        goto case Mark.Link;
                    }

                    HandleMark(mstack, result, Mark.Quote);
                    return true;

                case Mark.Link:
                    return true;

                case Mark.OList:
                case Mark.UnList:
                    HadleList(mstack, result, value);
                    return true;

                default:
                    if (mstack.Contains(Mark.OList)) HandleMark(mstack, result, Mark.OList);
                    if (mstack.Contains(Mark.UnList)) HandleMark(mstack, result, Mark.UnList);
                    return false;
            }
        }

        static private void HadleList(Stack<Mark> mstack, StringBuilder result, Mark value)
        {
            if (mstack.Contains(Mark.OList) || mstack.Contains(Mark.UnList))
            {
                HandleMark(mstack, result, Mark.ListElem);
            }
            else
            {
                HandleMark(mstack, result, value);
                HandleMark(mstack, result, Mark.ListElem);
            }
        }

        static private void HandleMark(Stack<Mark> mstack, StringBuilder result, Mark value)
        {
            if (mstack.Contains(value))
            {
                FixSyntaxAndConvertMarkToResult(mstack, result, value);
            }
            else
            {
                OpenMarkAndPushToStack(result, mstack, value);
            }
        }

        private static void CheckForNewLineMarksInStack(Stack<Mark> mstack, StringBuilder result)
        {
            if (mstack.Contains(Mark.Quote))
            {
                FixSyntaxAndConvertMarkToResult(mstack, result, Mark.Quote);
            }
            if (mstack.Contains(Mark.OList) || mstack.Contains(Mark.UnList))
            {
                FixSyntaxAndConvertMarkToResult(mstack, result, Mark.ListElem);
            }
        }

        private static void FixSyntaxAndConvertMarkToResult(Stack<Mark> mstack, StringBuilder result, Mark value)
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
