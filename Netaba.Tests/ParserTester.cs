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
        public ParserTester()
        {
            var testPosts = new [] { new { Id = 100, TreadId = 2 }, new { Id = 107, TreadId = 4 }, new { Id = 123, TreadId = 111 } };
            var mock = new Mock<IRepository>();
            var postPlace = (1, 11);
            mock.Setup(rep => rep.TryFindPost(It.IsAny<int>(), out postPlace))
                .Returns<int, (int , int)>((id, postPlace) => testPosts.FirstOrDefault(p => p.Id == id) != null);
            _parser = new Parser(mock.Object);
        }

        [Fact]
        public void BlankStringTest()
        {
            Assert.Equal("", _parser.ToHtml(""));
        }

        [Fact]
        public void MultipleLineBreaksTest()
        {
            Assert.Equal("<br><br><br>", _parser.ToHtml("\n\n\n\n"));
        }

        [Fact]
        public void NotNewLileMarksTest()
        {
            Assert.Equal("<b></b>", _parser.ToHtml("**"));
            Assert.Equal("<i></i>", _parser.ToHtml("_"));
            Assert.Equal("<b><i></i></b><i></i>", _parser.ToHtml("*_*_"));
            Assert.Equal("<b><span class=\"spoler\">qq<br></span>" +
                "</b><span class=\"spoler\"><i></i></span><i></i>", _parser.ToHtml("*#qq\n*_#_"));
            Assert.Equal("<b><i><span class=\"spoler\"></span></i></b><i>" +
                "<span class=\"spoler\"></span></i><span class=\"spoler\"></span>", _parser.ToHtml("*_#*_#"));
        }

        [Fact]
        public void QuoteMarkTest()
        {
            Assert.Equal("<span class=\"quote\">&gt;qweqwe</span>", _parser.ToHtml(">qweqwe"));
            Assert.Equal("<span class=\"quote\">&gt;test</span><br>" +
                "<span class=\"quote\">&gt;test</span>", _parser.ToHtml(">test\n>test"));
            Assert.Equal("<span class=\"quote\">&gt;qw<b>eqwe</b></span><b></b>", _parser.ToHtml(">qw*eqwe"));
        }

        [Fact]
        public void LinkMarkTest()
        {
            Assert.Equal("<a href=\"1/11/#107\">&gt;&gt;107</a> qqq", _parser.ToHtml(">>107 qqq"));
            Assert.Equal("&gt;&gt;1000000000", _parser.ToHtml(">>1000000000"));
            Assert.Equal("<a href=\"1/11/#100\">&gt;&gt;100</a>", _parser.ToHtml(">>100"));
            Assert.Equal("<a href=\"1/11/#123\">&gt;&gt;123</a><br>" +
                "<span class=\"quote\">&gt;qq</span>", _parser.ToHtml(">>123\n>qq"));
            Assert.Equal("&gt;&gt;r", _parser.ToHtml(">>r"));
            Assert.Equal("<b>&gt;&gt;</b>", _parser.ToHtml("*>>"));
            Assert.Equal("&gt;&gt;<br><br>q", _parser.ToHtml(">>\n\nq"));
        }

        [Fact]
        public void ListMarksTest()
        {
            Assert.Equal("<ul><li>w</li><li>ww</li><li>www</li></ul>", _parser.ToHtml("+w\n+ww\n+www"));
            Assert.Equal("<ol><li>w</li><li>ww</li><li>www</li></ol>", _parser.ToHtml("№w\n№ww\n№www"));
            Assert.Equal("<ol><li>w</li><li>w<b>w</b></li><b><li>www</li></b></ol><b></b>", _parser.ToHtml("№w\n№w*w\n№www\n"));
            Assert.Equal("<ul><li>w</li><li>ww</li></ul><ol><li>w</li></ol>", _parser.ToHtml("+w\n+ww\n№w"));
            Assert.Equal("<ol><li>w<b></b></li><b><li>ww</li></b></ol><b></b>", _parser.ToHtml("№w*\n№ww"));
            Assert.Equal("<b><ul><li>123</li></ul></b>", _parser.ToHtml("*\n+123"));
            Assert.Equal("<ul><li>123</li></ul>ttt", _parser.ToHtml("+123\nttt"));
        }
    }
}
