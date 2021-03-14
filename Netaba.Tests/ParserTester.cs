using Netaba.Services.Markup;
using Netaba.Services.Repository;
using System.Linq;
using System.Threading.Tasks;
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
            var mock = new Mock<IBoardRepository>();
            var treadId = 11;
            mock.Setup(rep => rep.TryGetPostLocationAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns<int, string>((id, boardName) => Task.FromResult((testPosts.FirstOrDefault(p => p.Id == id && p.BoardName == boardName) != null, treadId)));
            _parser = new Parser(mock.Object);
        }

        [Fact]
        public async Task BlankStringTest()
        {
            Assert.Equal("", await _parser.ToHtmlAsync("", defaultBoardName));
        }

        [Fact]
        public async Task MultipleLineBreaksTest()
        {
            Assert.Equal("<br><br><br>", await _parser.ToHtmlAsync("\n\n\n\n", defaultBoardName));
        }

        [Fact]
        public async Task NotNewLileMarksTest()
        {
            Assert.Equal("<b></b>", await _parser.ToHtmlAsync("**", defaultBoardName));
            Assert.Equal("<i></i>", await _parser.ToHtmlAsync("_", defaultBoardName));
            Assert.Equal("<b><i></i></b><i></i>", await _parser.ToHtmlAsync("*_*_", defaultBoardName));
            Assert.Equal("<b><span class=\"spoler\">qq<br></span>" +
                "</b><span class=\"spoler\"><i></i></span><i></i>", await _parser.ToHtmlAsync("*#qq\n*_#_", defaultBoardName));
            Assert.Equal("<b><i><span class=\"spoler\"></span></i></b><i>" +
                "<span class=\"spoler\"></span></i><span class=\"spoler\"></span>", await _parser.ToHtmlAsync("*_#*_#", defaultBoardName));
        }

        [Fact]
        public async Task QuoteMarkTest()
        {
            Assert.Equal("<span class=\"quote\">&gt;qweqwe</span>", await _parser.ToHtmlAsync(">qweqwe", defaultBoardName));
            Assert.Equal("<span class=\"quote\">&gt;test</span><br>" +
                "<span class=\"quote\">&gt;test</span>", await _parser.ToHtmlAsync(">test\n>test", defaultBoardName));
            Assert.Equal("<span class=\"quote\">&gt;qw<b>eqwe</b></span><b></b>", await _parser.ToHtmlAsync(">qw*eqwe", defaultBoardName));
        }

        [Fact]
        public async Task LinkMarkTest()
        {
            Assert.Equal("<a href=\"a/11/#1\">&gt;&gt;1</a> qqq", await _parser.ToHtmlAsync(">>1 qqq", "a"));
            Assert.Equal("&gt;&gt;1000000000", await _parser.ToHtmlAsync(">>1000000000", "dom"));
            Assert.Equal("<a href=\"b/11/#12\">&gt;&gt;12</a>", await _parser.ToHtmlAsync(">>12", "b"));
            Assert.Equal("<a href=\"c/11/#123\">&gt;&gt;123</a><br>" +
                "<span class=\"quote\">&gt;qq</span>", await _parser.ToHtmlAsync(">>123\n>qq", "c"));
            Assert.Equal("&gt;&gt;r", await _parser.ToHtmlAsync(">>r", "heh"));
            Assert.Equal("<b>&gt;&gt;</b>", await _parser.ToHtmlAsync("*>>", "heh"));
            Assert.Equal("&gt;&gt;<br><br>q", await _parser.ToHtmlAsync(">>\n\nq", "heh"));
        }

        [Fact]
        public async Task ListMarksTest()
        {
            Assert.Equal("<ul><li>w</li><li>ww</li><li>www</li></ul>", await _parser.ToHtmlAsync("+w\n+ww\n+www", defaultBoardName));
            Assert.Equal("<ol><li>w</li><li>ww</li><li>www</li></ol>", await _parser.ToHtmlAsync("№w\n№ww\n№www", defaultBoardName));
            Assert.Equal("<ol><li>w</li><li>w<b>w</b></li><b><li>www</li></b></ol><b></b>", await _parser.ToHtmlAsync("№w\n№w*w\n№www\n", defaultBoardName));
            Assert.Equal("<ul><li>w</li><li>ww</li></ul><ol><li>w</li></ol>", await _parser.ToHtmlAsync("+w\n+ww\n№w", defaultBoardName));
            Assert.Equal("<ol><li>w<b></b></li><b><li>ww</li></b></ol><b></b>", await _parser.ToHtmlAsync("№w*\n№ww", defaultBoardName));
            Assert.Equal("<b><ul><li>123</li></ul></b>", await _parser.ToHtmlAsync("*\n+123", defaultBoardName));
            Assert.Equal("<ul><li>123</li></ul>ttt", await _parser.ToHtmlAsync("+123\nttt", defaultBoardName));
        }
    }
}
