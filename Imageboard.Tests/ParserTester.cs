using Imageboard.Data.Contexts;
using Imageboard.Services.Markup;
using Xunit;
using Moq;

namespace Tests
{
    public class ParserTester
    {
        private static readonly Parser _parser = new Parser();
        private static readonly ApplicationDbContext _db = ApplicationDbContextFactory.CreateDbContext();

        [Fact]
        public void BlankStringTest()
        {
            Assert.Equal("", _parser.ToHtml("", _db));
        }

        [Fact]
        public void MultipleLineBreaksTest()
        {
            Assert.Equal("<br><br><br>", _parser.ToHtml("\n\n\n\n", _db));
        }

        [Fact]
        public void NotNewLileMarksTest()
        {
            Assert.Equal("<b></b>", _parser.ToHtml("**", _db));
            Assert.Equal("<i></i>", _parser.ToHtml("_", _db));
            Assert.Equal("<b><i></i></b><i></i>", _parser.ToHtml("*_*_", _db));
            Assert.Equal("<b><span class=\"spoler\">qq<br></span>" +
                "</b><span class=\"spoler\"><i></i></span><i></i>", _parser.ToHtml("*#qq\n*_#_", _db));
            Assert.Equal("<b><i><span class=\"spoler\"></span></i></b><i>" +
                "<span class=\"spoler\"></span></i><span class=\"spoler\"></span>", _parser.ToHtml("*_#*_#", _db));
        }

        [Fact]
        public void QuoteMarkTest()
        {
            Assert.Equal("<span class=\"quote\">&gt;qweqwe</span>", _parser.ToHtml(">qweqwe", _db));
            Assert.Equal("<span class=\"quote\">&gt;test</span><br>" +
                "<span class=\"quote\">&gt;test</span>", _parser.ToHtml(">test\n>test", _db));
            Assert.Equal("<span class=\"quote\">&gt;qw<b>eqwe</b></span><b></b>", _parser.ToHtml(">qw*eqwe", _db));
        }

        [Fact]
        public void LinkMarkTest()
        {
            Assert.Equal("<a href=\"/Home/DisplayTread/6/#107\">&gt;&gt;107</a> qqq", _parser.ToHtml(">>107 qqq", _db));
            Assert.Equal("&gt;&gt;1000000000", _parser.ToHtml(">>1000000000", _db));
            Assert.Equal("<a href=\"/Home/DisplayTread/7/#100\">&gt;&gt;100</a>", _parser.ToHtml(">>100", _db));
            Assert.Equal("<a href=\"/Home/DisplayTread/6/#123\">&gt;&gt;123</a><br>" +
                "<span class=\"quote\">&gt;qq</span>", _parser.ToHtml(">>123\n>qq", _db));
            Assert.Equal("&gt;&gt;r", _parser.ToHtml(">>r", _db));
            Assert.Equal("<b>&gt;&gt;</b>", _parser.ToHtml("*>>", _db));
            Assert.Equal("&gt;&gt;<br><br>q", _parser.ToHtml(">>\n\nq", _db));
        }

        [Fact]
        public void ListMarksTest()
        {
            Assert.Equal("<ul><li>w</li><li>ww</li><li>www</li></ul>", _parser.ToHtml("+w\n+ww\n+www", _db));
            Assert.Equal("<ol><li>w</li><li>ww</li><li>www</li></ol>", _parser.ToHtml("№w\n№ww\n№www", _db));
            Assert.Equal("<ol><li>w</li><li>w<b>w</b></li><b><li>www</li></b></ol><b></b>", _parser.ToHtml("№w\n№w*w\n№www\n", _db));
            Assert.Equal("<ul><li>w</li><li>ww</li></ul><ol><li>w</li></ol>", _parser.ToHtml("+w\n+ww\n№w", _db));
            Assert.Equal("<ol><li>w<b></b></li><b><li>ww</li></b></ol><b></b>", _parser.ToHtml("№w*\n№ww", _db));
            Assert.Equal("<b><ul><li>123</li></ul></b>", _parser.ToHtml("*\n+123", _db));
            Assert.Equal("<ul><li>123</li></ul>ttt", _parser.ToHtml("+123\nttt", _db));
        }
    }
}
