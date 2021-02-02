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

            HandleMark(mstack, result, Mark.End);
            for (var i = 0; i < value.Length; i++)
            {
                var isFirstChar = i == 0;
                var mappedValue = CharToMarkMapper.Map(value[i], isFirstChar);
                if (mappedValue == Mark.NewLine || isFirstChar)
                {
                    HadleNewLine(value, mstack, result, ref i, ref mappedValue, isFirstChar);
                    continue;
                }

                if (mappedValue != Mark.None) HandleMark(mstack, result, mappedValue);
                else result.Append(value[i]);
            }
            HadleEnd(result, mstack);

            return result.ToString();
        }

        static private void HadleNewLine(string sourse, Stack<Mark> mstack, StringBuilder result, 
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

                    HandleMark(mstack, result, Mark.Quote);
                    return;

                case Mark.Link:
                    return;

                case Mark.OList:
                case Mark.UnList:
                    HadleList(mstack, result, value);
                    return;

                default:
                    if (mstack.Contains(Mark.OList)) HandleMark(mstack, result, Mark.OList);
                    if (mstack.Contains(Mark.UnList)) HandleMark(mstack, result, Mark.UnList);

                    if (!isFirstChar) result.Append('\n'); // sourse[position - 1]

                    if (value != Mark.None) HandleMark(mstack, result, value);
                    else result.Append(sourse[position]);
                    return;
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
            if (mstack.Contains(Mark.OList)) HandleMark(mstack, result, Mark.OList);
            if (mstack.Contains(Mark.UnList)) HandleMark(mstack, result, Mark.UnList);
            HandleMark(mstack, result, Mark.End);
        }

        // To close ListElem or Quote.
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
            if (value == Mark.Quote) result.Append('\n');
            stack.Push(value);
        }

        static private void CloseMark(StringBuilder result, Mark value)
        {
            result.Append(MarkToHtmlMapper.MapToClosingElem(value));
        }
    }
}
