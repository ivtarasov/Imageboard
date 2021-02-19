using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imageboard.Data.Contexts;
using System.Web;

namespace Imageboard.Services.Markup
{
    public class Parser: IParser
    {
        private readonly IMapper _mapper;

        public Parser(IMapper mapper) => _mapper = mapper;

        public string ToHtml(string value, ApplicationDbContext context)
        {
            if (value == null) return "";
            if (context == null) throw new NullReferenceException("ApplicaionDbContext instance is null in Parser.ToHtml method.");

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
                    mvalue = _mapper.ToMark(smark, false);
                }

                if (mvalue == Mark.NewLine)
                {
                    if (i == value.Length - 1) continue;
                    if (HadleNewLine(value, result, mstack, ++i, ref mvalue)) continue;
                }

                switch (mvalue)
                {
                    case Mark.NewLine:
                        result.Append(_mapper.NewLine());
                        break;
                    case Mark.Link:
                        i++;
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

        private bool HadleNewLine(string sourse, StringBuilder result, Stack<Mark> mstack, int pos, ref Mark value)
        {
            var isFirst = pos == 0;
            CheckForListsAndQuoteInStack(result, mstack);
            var smark = (pos + 1 < sourse.Length) ? (sourse[pos], sourse[pos + 1]) : (sourse[pos], '!');
            value = _mapper.ToMark(smark, true);

            switch (value)
            {
                case Mark.Quote:

                    if (mstack.Contains(Mark.OList)) FixSyntax(result, mstack, Mark.OList);
                    if (mstack.Contains(Mark.UnList)) FixSyntax(result, mstack, Mark.UnList);

                    if (!isFirst) result.Append(_mapper.NewLine());

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

                    if (!isFirst && !isAfterClosingList) result.Append(_mapper.NewLine());

                    return false;
            }
        }

        private void HadleLink(StringBuilder result, string sourse, ref int pos, 
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
                result.Append(_mapper.BlankLink());
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
                result.Append($"{_mapper.HtmlForLink(href, postId)}");

                return;
            }
            else
            {
                result.Append(_mapper.BlankLink() + postId);
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
            result.Append(_mapper.ToOpeningHtml(value));
            stack.Push(value);
        }

        private void CloseMark(StringBuilder result, Stack<Mark> stack, Mark value)
        {
            result.Append(_mapper.ToClosingHtml(value));
            stack.Push(value);
        }

        private void CloseMark(StringBuilder result, Mark value)
        {
            result.Append(_mapper.ToClosingHtml(value));
        }
    }
}
