using Netaba.Services.Markup;
using Netaba.Services.Repository;
using System.Linq;
using Xunit;
using Moq;

namespace Tests
{
    public class ParserTester
    {
        private readonly Parser _parser;
        private readonly string defaultBoardName = "a";
        public ParserTester()
        {
            var testPosts = new [] { new { Id = 1, BoardName = "a" }, new { Id = 12, BoardName = "b" }, new { Id = 123, BoardName = "c" } };
            var mock = new Mock<IRepository>();
            var treadId = 11;
            mock.Setup(rep => rep.TryGetPostLocation(It.IsAny<int>(), It.IsAny<string>(), out treadId))
                .Returns<int, string, int>((id, boardName, treadId) => testPosts.FirstOrDefault(p => p.Id == id && p.BoardName == boardName) != null);
            _parser = new Parser(mock.Object);
        }

        [Fact]
        public void BlankStringTest()
        {
            Assert.Equal("", _parser.ToHtml("", defaultBoardName));
        }

        [Fact]
        public void MultipleLineBreaksTest()
        {
            Assert.Equal("<br><br><br>", _parser.ToHtml("\n\n\n\n", defaultBoardName));
        }

        [Fact]
        public void NotNewLileMarksTest()
        {
            Assert.Equal("<b></b>", _parser.ToHtml("**", defaultBoardName));
            Assert.Equal("<i></i>", _parser.ToHtml("_", defaultBoardName));
            Assert.Equal("<b><i></i></b><i></i>", _parser.ToHtml("*_*_", defaultBoardName));
            Assert.Equal("<b><span class=\"spoler\">qq<br></span>" +
                "</b><span class=\"spoler\"><i></i></span><i></i>", _parser.ToHtml("*#qq\n*_#_", defaultBoardName));
            Assert.Equal("<b><i><span class=\"spoler\"></span></i></b><i>" +
                "<span class=\"spoler\"></span></i><span class=\"spoler\"></span>", _parser.ToHtml("*_#*_#", defaultBoardName));
        }

        [Fact]
        public void QuoteMarkTest()
        {
            Assert.Equal("<span class=\"quote\">&gt;qweqwe</span>", _parser.ToHtml(">qweqwe", defaultBoardName));
            Assert.Equal("<span class=\"quote\">&gt;test</span><br>" +
                "<span class=\"quote\">&gt;test</span>", _parser.ToHtml(">test\n>test", defaultBoardName));
            Assert.Equal("<span class=\"quote\">&gt;qw<b>eqwe</b></span><b></b>", _parser.ToHtml(">qw*eqwe", defaultBoardName));
        }

        [Fact]
        public void LinkMarkTest()
        {
            Assert.Equal("<a href=\"a/11/#1\">&gt;&gt;1</a> qqq", _parser.ToHtml(">>1 qqq", "a"));
            Assert.Equal("&gt;&gt;1000000000", _parser.ToHtml(">>1000000000", "dom"));
            Assert.Equal("<a href=\"b/11/#12\">&gt;&gt;12</a>", _parser.ToHtml(">>12", "b"));
            Assert.Equal("<a href=\"c/11/#123\">&gt;&gt;123</a><br>" +
                "<span class=\"quote\">&gt;qq</span>", _parser.ToHtml(">>123\n>qq", "c"));
            Assert.Equal("&gt;&gt;r", _parser.ToHtml(">>r", "heh"));
            Assert.Equal("<b>&gt;&gt;</b>", _parser.ToHtml("*>>", "heh"));
            Assert.Equal("&gt;&gt;<br><br>q", _parser.ToHtml(">>\n\nq", "heh"));
        }

        [Fact]
        public void ListMarksTest()
        {
            Assert.Equal("<ul><li>w</li><li>ww</li><li>www</li></ul>", _parser.ToHtml("+w\n+ww\n+www", defaultBoardName));
            Assert.Equal("<ol><li>w</li><li>ww</li><li>www</li></ol>", _parser.ToHtml("№w\n№ww\n№www", defaultBoardName));
            Assert.Equal("<ol><li>w</li><li>w<b>w</b></li><b><li>www</li></b></ol><b></b>", _parser.ToHtml("№w\n№w*w\n№www\n", defaultBoardName));
            Assert.Equal("<ul><li>w</li><li>ww</li></ul><ol><li>w</li></ol>", _parser.ToHtml("+w\n+ww\n№w", defaultBoardName));
            Assert.Equal("<ol><li>w<b></b></li><b><li>ww</li></b></ol><b></b>", _parser.ToHtml("№w*\n№ww", defaultBoardName));
            Assert.Equal("<b><ul><li>123</li></ul></b>", _parser.ToHtml("*\n+123", defaultBoardName));
            Assert.Equal("<ul><li>123</li></ul>ttt", _parser.ToHtml("+123\nttt", defaultBoardName));
        }
    }
}
