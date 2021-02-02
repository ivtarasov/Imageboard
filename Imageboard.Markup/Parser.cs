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

            HandleMark(result, mstack, Mark.End);
            for (var i = 0; i < value.Length; i++)
            {
                var isFirstChar = i == 0;
                var mappedValue = CharToMarkMapper.Map(value[i], isFirstChar);
                if (mappedValue == Mark.NewLine || isFirstChar)
                {
                    HadleNewLine(value, result, mstack, ref i, ref mappedValue, isFirstChar);
                    continue;
                }

                if (mappedValue != Mark.None) HandleMark(result, mstack, mappedValue);
                else result.Append(value[i]);
            }
            HadleEnd(result, mstack);

            return result.ToString();
        }

        static private void HadleNewLine(string sourse, StringBuilder result, Stack<Mark> mstack,
                                         ref int position, ref Mark value, bool isFirstChar)
        {
            if (!isFirstChar)
            {
                if (position == sourse.Length - 1) return;
                CheckForNewLineMarksInStack(result, mstack);
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

                    if (mstack.Contains(Mark.OList)) HandleMark(result, mstack, Mark.OList);
                    if (mstack.Contains(Mark.UnList)) HandleMark(result, mstack, Mark.UnList);

                    HandleMark(result, mstack, Mark.Quote);
                    return;

                case Mark.Link:
                    if (mstack.Contains(Mark.OList)) HandleMark(result, mstack, Mark.OList);
                    if (mstack.Contains(Mark.UnList)) HandleMark(result, mstack, Mark.UnList);
                    return;

                case Mark.OList:
                    if (mstack.Contains(Mark.UnList)) HandleMark(result, mstack, Mark.UnList);
                    HadleList(mstack, result, value);
                    return;
                case Mark.UnList:
                    if (mstack.Contains(Mark.OList)) HandleMark(result, mstack, Mark.OList);
                    HadleList(mstack, result, value);
                    return;

                default:
                    if (mstack.Contains(Mark.OList)) HandleMark(result, mstack, Mark.OList);
                    if (mstack.Contains(Mark.UnList)) HandleMark(result, mstack, Mark.UnList);

                    if (!isFirstChar) result.Append("<br>"); // sourse[position - 1]

                    if (value != Mark.None) HandleMark(result, mstack, value);
                    else result.Append(sourse[position]);
                    return;
            }
        }

        // Only to open list.
        static private void HadleList(Stack<Mark> mstack, StringBuilder result, Mark value)
        {
            if (mstack.Contains(value))
            {
                HandleMark(result, mstack, Mark.ListElem);
            }
            else
            {
                HandleMark(result, mstack, value);
                HandleMark(result, mstack, Mark.ListElem);
            }
        }

        static private void HandleMark(StringBuilder result, Stack<Mark> mstack, Mark value)
        {
            if (mstack.Contains(value))
            {
                FixSyntaxAndConvertMarkToResult(result, mstack, value);
            }
            else
            {
                OpenMarkAndPushToStack(result, mstack, value);
            }
        }

        static private void HadleEnd(StringBuilder result, Stack<Mark> mstack)
        {
            CheckForListsAndQuoteInStack(result, mstack);
            if (mstack.Contains(Mark.OList)) HandleMark(result, mstack, Mark.OList);
            if (mstack.Contains(Mark.UnList)) HandleMark(result, mstack, Mark.UnList);
            HandleMark(result, mstack, Mark.End);
        }

        // Only to close ListElem or Quote.
        private static void CheckForNewLineMarksInStack(StringBuilder result, Stack<Mark> mstack)
        {
            CheckForListsAndQuoteInStack(result, mstack);
        }

        private static void CheckForListsAndQuoteInStack(StringBuilder result, Stack<Mark> mstack)
        {
            if (mstack.Contains(Mark.Quote))
            {
                FixSyntaxAndConvertMarkToResult(result, mstack, Mark.Quote);
            }
            if (mstack.Contains(Mark.OList) || mstack.Contains(Mark.UnList))
            {
                FixSyntaxAndConvertMarkToResult(result, mstack, Mark.ListElem);
            }
        }

        private static void FixSyntaxAndConvertMarkToResult(StringBuilder result, Stack<Mark> mstack, Mark value)
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
            if (value == Mark.Quote) result.Append("<br>");
            stack.Push(value);
        }

        static private void CloseMark(StringBuilder result, Mark value)
        {
            result.Append(MarkToHtmlMapper.MapToClosingElem(value));
        }
    }
}
