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
        public static string ToHtml(string value, ApplicationDbContext context)
        {
            var mstack = new Stack<Mark>();
            var result = new StringBuilder();

            OpenMark(result, mstack, Mark.End);
            for (var i = 0; i < value.Length; i++)
            {
                Mark mvalue;
                if (i == 0)
                {
                    mvalue = Mark.NewLine;
                    i--;
                }
                else mvalue = Mapper.ToMark(value, ref i, false);

                if (mvalue == Mark.NewLine)
                {
                    if (HadleNewLine(value, result, mstack, ref i, ref mvalue, context)) continue;
                }

                switch (mvalue)
                {
                    case Mark.NewLine:
                        result.Append(Mapper.NewLine());
                        break;
                    case Mark.Link:
                        HadleLink(result, value, ref i, context);
                        break;
                    case not Mark.None:
                        HandleMark(result, mstack, mvalue);
                        break;
                    default:
                        result.Append(HttpUtility.HtmlEncode(value[i]));
                        break;
                }
            }
            CloseMarkup(result, mstack);

            return result.ToString();
        }

        static private bool HadleNewLine(string sourse, StringBuilder result, Stack<Mark> mstack,
                                         ref int pos, ref Mark value, ApplicationDbContext context)
        {
            if (pos == sourse.Length - 1) return true;
            var isFirst = pos == -1;
            CheckForListsAndQuoteInStack(result, mstack);
            pos++;
            value = Mapper.ToMark(sourse, ref pos, true);

            switch (value)
            {
                case Mark.Quote:

                    if (mstack.Contains(Mark.OList)) FixSyntax(result, mstack, Mark.OList);
                    if (mstack.Contains(Mark.UnList)) FixSyntax(result, mstack, Mark.UnList);

                    if (!isFirst) result.Append(Mapper.NewLine());

                    OpenMark(result, mstack, Mark.Quote);
                    return true;

                case Mark.OList:
                    if (mstack.Contains(Mark.UnList)) FixSyntax(result, mstack, Mark.UnList);
                    OpenList(result, mstack, value);
                    return true;

                case Mark.UnList:
                    if (mstack.Contains(Mark.OList)) FixSyntax(result, mstack, Mark.OList);
                    OpenList(result, mstack, value);
                    return true;

                default:
                    bool isAfterClosingList = false;
                    if (mstack.Contains(Mark.OList))
                    {
                        FixSyntax(result, mstack, Mark.OList);
                        isAfterClosingList = true;
                    }
                    if (mstack.Contains(Mark.UnList))
                    {
                        FixSyntax(result, mstack, Mark.UnList);
                        isAfterClosingList = true;
                    }

                    if (!isFirst && !isAfterClosingList) result.Append(Mapper.NewLine());

                    return false;
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
                result.Append(Mapper.BlankLink());
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
                result.Append($"{Mapper.HtmlForLink(href, postId)}");

                return;
            }
            else
            {
                result.Append(Mapper.BlankLink() + postId);
                return;
            }
        }

        static private void OpenList(StringBuilder result, Stack<Mark> mstack, Mark value)
        {
            if (!mstack.Contains(value)) OpenMark(result, mstack, value);
            OpenMark(result, mstack, Mark.ListElem);
        }

        static private void HandleMark(StringBuilder result, Stack<Mark> mstack, Mark value)
        {
            if (mstack.Contains(value)) FixSyntax(result, mstack, value);
            else OpenMark(result, mstack, value);
        }

        static private void CloseMarkup(StringBuilder result, Stack<Mark> mstack)
        {
            CheckForListsAndQuoteInStack(result, mstack);
            if (mstack.Contains(Mark.OList)) FixSyntax(result, mstack, Mark.OList);
            if (mstack.Contains(Mark.UnList)) FixSyntax(result, mstack, Mark.UnList);
            FixSyntax(result, mstack, Mark.End);
        }

        private static void CheckForListsAndQuoteInStack(StringBuilder result, Stack<Mark> mstack)
        {
            if (mstack.Contains(Mark.Quote)) FixSyntax(result, mstack, Mark.Quote);
            if (mstack.Contains(Mark.OList) || mstack.Contains(Mark.UnList))  FixSyntax(result, mstack, Mark.ListElem);
        }

        private static void FixSyntax(StringBuilder result, Stack<Mark> mstack, Mark value)
        {
            Mark mark;
            var tmpstack = new Stack<Mark>();

            while ((mark = mstack.Pop()) != value)
            {
                CloseMark(result, tmpstack, mark);
            }

            CloseMark(result, value);

            if (value != Mark.End)
            {
                while (tmpstack.TryPop(out mark))
                {
                    OpenMark(result, mstack, mark);
                }
            }
        }

        static private void OpenMark(StringBuilder result, Stack<Mark> stack, Mark value)
        {
            result.Append(Mapper.ToOpeningHtml(value));
            stack.Push(value);
        }

        static private void CloseMark(StringBuilder result, Stack<Mark> stack, Mark value)
        {
            result.Append(Mapper.ToClosingHtml(value));
            stack.Push(value);
        }

        static private void CloseMark(StringBuilder result, Mark value)
        {
            result.Append(Mapper.ToClosingHtml(value));
        }
    }
}
