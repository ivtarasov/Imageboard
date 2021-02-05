using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imageboard.Data.Contexts;
using System.Web;

namespace Imageboard.Markup
{
    public static class Parser
    {
        public static string MarkUp(string value, ApplicationDbContext context)
        {
            var mstack = new Stack<Mark>();
            var result = new StringBuilder();

            HandleMark(result, mstack, Mark.End);
            for (var i = 0; i < value.Length; i++)
            {
                var isFirstChar = i == 0;
                var mappedValue = CharToMarkMapper.Map(value, ref i, isFirstChar);
                if (mappedValue == Mark.NewLine || isFirstChar)
                {
                    HadleNewLine(value, result, mstack, ref i, ref mappedValue, isFirstChar, context);
                    continue;
                }

                if (mappedValue != Mark.None)
                {   
                    if (mappedValue == Mark.Link) HadleLink(result, value, ref i, context);
                    else HandleMark(result, mstack, mappedValue);
                }
                else result.Append(HttpUtility.HtmlEncode(value[i]));
            }
            HadleEnd(result, mstack);

            return result.ToString();
        }

        static private void HadleNewLine(string sourse, StringBuilder result, Stack<Mark> mstack,
                                         ref int pos, ref Mark value, bool isFirstChar, 
                                         ApplicationDbContext context)
        {
            if (!isFirstChar)
            {
                if (pos == sourse.Length - 1) return;
                CheckForNewLineMarksInStack(result, mstack);
                pos++;
                value = CharToMarkMapper.Map(sourse, ref pos, true);
            }

            switch (value)
            {
                case Mark.Quote:

                    if (mstack.Contains(Mark.OList)) HandleMark(result, mstack, Mark.OList);
                    if (mstack.Contains(Mark.UnList)) HandleMark(result, mstack, Mark.UnList);

                    if (!isFirstChar) result.Append(MarkToHtmlMapper.NewLine());

                    HandleMark(result, mstack, Mark.Quote);
                    return;

                case Mark.OList:
                    if (mstack.Contains(Mark.UnList)) HandleMark(result, mstack, Mark.UnList);
                    HadleList(result, mstack, value);
                    return;

                case Mark.UnList:
                    if (mstack.Contains(Mark.OList)) HandleMark(result, mstack, Mark.OList);
                    HadleList(result, mstack, value);
                    return;

                default:
                    bool isAfterClosingList = false;
                    if (mstack.Contains(Mark.OList))
                    {
                        HandleMark(result, mstack, Mark.OList);
                        isAfterClosingList = true;
                    }
                    if (mstack.Contains(Mark.UnList))
                    {
                        HandleMark(result, mstack, Mark.UnList);
                        isAfterClosingList = true;
                    }

                    if (!isFirstChar && !isAfterClosingList) result.Append(MarkToHtmlMapper.NewLine());

                    if (value != Mark.None)
                    {
                        if (value == Mark.Link) HadleLink(result, sourse, ref pos, context);
                        else HandleMark(result, mstack, value);
                    }
                    else result.Append(HttpUtility.HtmlEncode(sourse[pos]));
                    return;
            }
        }

        static private void HadleLink(StringBuilder result, string sourse, ref int pos, 
                                      ApplicationDbContext context)
        {
            var digits = new Stack<int>();
            int digit;
            while (++pos < sourse.Length && (digit = (int) char.GetNumericValue(sourse[pos])) != -1.0)
            {
                digits.Push(digit);
            }
            pos--;

            if (!digits.Any())
            {
                result.Append(MarkToHtmlMapper.BlankLink());
                return;
            }

            var postId = 0;
            var i = 0;
            while (digits.TryPop(out digit))
            {
                postId += digit * (int) Math.Pow(10, i++);
            }

            var post = context.Posts.Find(postId);
            if (post != null)
            {
                context.Entry(post).Reference(p => p.Tread).Load();
                context.Entry(post.Tread).Reference(t => t.Board).Load();

                string href = $"/Home/DisplayTread/{post.TreadId}/#{post.Id}";
                result.Append($"{MarkToHtmlMapper.HtmlForLink(href, postId)}");

                return;
            }
            else
            {
                result.Append(MarkToHtmlMapper.BlankLink() + postId);
                return;
            }
        }

        // Only to open list.
        static private void HadleList(StringBuilder result, Stack<Mark> mstack, Mark value)
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
            stack.Push(value);
        }

        static private void CloseMark(StringBuilder result, Mark value)
        {
            result.Append(MarkToHtmlMapper.MapToClosingElem(value));
        }
    }
}
