using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Netaba.Services.Repository;

namespace Netaba.Services.Markup
{
    public class Parser: IParser
    {
        private readonly IRepository _repository;
        public Parser(IRepository repository) => _repository = repository;

        public string ToHtml(string value)
        {
            if (value == null) return "";

            var mstack = new Stack<Mark>();
            var result = new StringBuilder();

            OpenMark(result, mstack, Mark.Edge);
            for (var i = 0; i < value.Length; i++)
            {
                Mark mvalue;
                if (i == 0)
                {
                    i--;
                    mvalue = Mark.NewLine;
                }
                else
                {
                    var smark = (i + 1 < value.Length) ? (value[i], value[i + 1]) : (value[i], '!');
                    mvalue = Mapper.ToMark(smark, false);
                }

                if (mvalue == Mark.NewLine)
                {
                    if (i == value.Length - 1) continue;
                    if (HadleNewLine(value, result, mstack, ++i, ref mvalue)) continue;
                }

                switch (mvalue)
                {
                    case Mark.NewLine:
                        result.Append(Mapper.NewLine());
                        break;
                    case Mark.Link:
                        i++;
                        HadleLink(result, value, ref i);
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

        private bool HadleNewLine(string sourse, StringBuilder result, Stack<Mark> mstack, int pos, ref Mark value)
        {
            var isFirst = pos == 0;
            CheckForListsAndQuoteInStack(result, mstack);
            var smark = (pos + 1 < sourse.Length) ? (sourse[pos], sourse[pos + 1]) : (sourse[pos], '!');
            value = Mapper.ToMark(smark, true);

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

        private void HadleLink(StringBuilder result, string sourse, ref int pos)
        {
            var digits = new Stack<int>();
            int digit;
            while (++pos < sourse.Length && (digit = (int) char.GetNumericValue(sourse[pos])) != -1)
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

            var post = _repository.FindPost(postId);
            if (post != null)
            {
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

        private void OpenList(StringBuilder result, Stack<Mark> mstack, Mark value)
        {
            if (!mstack.Contains(value)) OpenMark(result, mstack, value);
            OpenMark(result, mstack, Mark.ListElem);
        }

        private void HandleMark(StringBuilder result, Stack<Mark> mstack, Mark value)
        {
            if (mstack.Contains(value)) FixSyntax(result, mstack, value);
            else OpenMark(result, mstack, value);
        }

        private void CloseMarkup(StringBuilder result, Stack<Mark> mstack)
        {
            CheckForListsAndQuoteInStack(result, mstack);
            if (mstack.Contains(Mark.OList)) FixSyntax(result, mstack, Mark.OList);
            if (mstack.Contains(Mark.UnList)) FixSyntax(result, mstack, Mark.UnList);
            FixSyntax(result, mstack, Mark.Edge);
        }

        void CheckForListsAndQuoteInStack(StringBuilder result, Stack<Mark> mstack)
        {
            if (mstack.Contains(Mark.Quote)) FixSyntax(result, mstack, Mark.Quote);
            if (mstack.Contains(Mark.OList) || mstack.Contains(Mark.UnList))  FixSyntax(result, mstack, Mark.ListElem);
        }

        private void FixSyntax(StringBuilder result, Stack<Mark> mstack, Mark value)
        {
            Mark mark;
            var tmpstack = new Stack<Mark>();

            while ((mark = mstack.Pop()) != value)
            {
                CloseMark(result, tmpstack, mark);
            }

            CloseMark(result, value);
            if (value != Mark.Edge)
            {
                while (tmpstack.TryPop(out mark))
                {
                    OpenMark(result, mstack, mark);
                }
            }
        }

        private void OpenMark(StringBuilder result, Stack<Mark> stack, Mark value)
        {
            result.Append(Mapper.ToOpeningHtml(value));
            stack.Push(value);
        }

        private void CloseMark(StringBuilder result, Stack<Mark> stack, Mark value)
        {
            result.Append(Mapper.ToClosingHtml(value));
            stack.Push(value);
        }

        private void CloseMark(StringBuilder result, Mark value)
        {
            result.Append(Mapper.ToClosingHtml(value));
        }
    }
}
