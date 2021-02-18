using System;
using Imageboard.Markup;
using System.Text;
using System.Collections.Generic;
using Imageboard.Data.Contexts;

namespace MarkupTests
{
    class Test
    {
        private static readonly Dictionary<string, string> _testData = new Dictionary<string, string>
        {
            { "**", "<b></b>" },
            { "*_*_", "<b><i></i></b><i></i>" },
            { "*_#*_#", "<b><i><span></span></i></b><i><span></span></i><span></span>"},
            { "", "" },
            { "*#qq\n*_#_", "<b><span>qq<br></span></b><span><i></i></span><i></i>" },
            { "_", "<i></i>" },
            { ">qweqwe", "<span style=\"color: #789922;\">&gt;qweqwe</span>" },
            { "+w\n+ww\n+www", "<ul><li>w</li><li>ww</li><li>www</li></ul>" },
            { "№w\n№ww\n№www", "<ol><li>w</li><li>ww</li><li>www</li></ol>" },
            { "№w\n№w*w\n№www\n", "<ol><li>w</li><li>w<b>w</b></li><b><li>www</li></b></ol><b></b>" },
            { "№w*\n№ww", "<ol><li>w<b></b></li><b><li>ww</li></b></ol><b></b>" },
            { "+w\n№ww", "<ul><li>w</li></ul><ol><li>ww</li></ol>" },
            { ">>107 qqq", "<a href=\"/Home/DisplayTread/6/#107\">&gt;&gt;107</a> qqq"},
            { ">>1000000000", "&gt;&gt;1000000000" },
            { ">>100", "<a href=\"/Home/DisplayTread/7/#100\">&gt;&gt;100</a>" },
            { ">test\n>test", "<span style=\"color: #789922;\">&gt;test</span><br><span style=\"color: #789922;\">&gt;test</span>" },
            { ">>123\n>qq", "<a href=\"/Home/DisplayTread/6/#123\">&gt;&gt;123</a><br><span style=\"color: #789922;\">&gt;qq</span>" },
            { "+123\nttt", "<ul><li>123</li></ul>ttt" },
            { ">>r", "&gt;&gt;r" },
            { "*\n+123", "<b><ul><li>123</li></ul></b>" },
            { "*>>" , "<b>&gt;&gt;</b>"},
            { "\n\n\n\n", "<br><br><br>" },
            { ">>\n\nq", "&gt;&gt;<br><br>q" },

        };

        static private void TestParser()
        {

            var contex = ApplicationDbContextFactory.CreateDbContext();

            var testOutput = new StringBuilder();
            string result;

            var i = 0;
            foreach (var testPair in _testData)
            {
                result = Parser.ToHtml(testPair.Key, contex);

                if (result == testPair.Value)
                {
                    testOutput.Append($"Test №{i} passed.\n")
                                 .Append($"#########\n");
                }
                else
                {
                    testOutput.Append($"Test №{i} failed.\n")
                                 .Append($"Expected result: {testPair.Value}\n")
                                 .Append($"The result: {result}\n")
                                 .Append($"#########\n");
                }

                Console.WriteLine(testOutput.ToString());
                testOutput.Clear();
                i++;
            }
        }

        public static void Main()
        {
            TestParser();
        }
    }
}
