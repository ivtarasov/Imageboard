using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Netaba.Services.Repository;
using System.Threading.Tasks;

namespace Netaba.Services.Markup
{
    public class Parser: IParser
    {
        private readonly IBoardRepository _repository;
        public Parser(IBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> ToHtmlAsync(string value, string boardName)
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
                        i = await HadleLinkAsync(result, value, i, boardName);
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

        private async Task<int> HadleLinkAsync(StringBuilder result, string sourse, int pos, string boardName)
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
                return pos;
            }

            var postId = 0;
            var i = 0;
            while (digits.TryPop(out digit))
            {
                postId += digit * (int) Math.Pow(10, i++);
            }

            var (isFound, treadId) = await _repository.TryGetPostLocationAsync(postId, boardName);
            if (isFound)
            {
                result.Append($"{Mapper.HtmlForLink(boardName, treadId, postId)}");
                return pos;
            }
            else
            {
                result.Append(Mapper.BlankLink() + postId);
                return pos;
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

        private static void OpenMark(StringBuilder result, Stack<Mark> stack, Mark value)
        {
            result.Append(Mapper.ToOpeningHtml(value));
            stack.Push(value);
        }

        private static void CloseMark(StringBuilder result, Stack<Mark> stack, Mark value)
        {
            result.Append(Mapper.ToClosingHtml(value));
            stack.Push(value);
        }

        private static void CloseMark(StringBuilder result, Mark value)
        {
            result.Append(Mapper.ToClosingHtml(value));
        }
    }
}
