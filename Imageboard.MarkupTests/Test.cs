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
            { "**", "<article><b></b></article>" },
            { "*_*_", "<article><b><i></i></b><i></i></article>" },
            { "*_#*_#", "<article><b><i><span></span></i></b><i><span></span></i><span></span></article>"},
            { "", "<article></article>" },
            { "*#qq\n*_#_", "<article><b><span>qq<br></span></b><span><i></i></span><i></i></article>" },
            { "_", "<article><i></i></article>" },
            { ">qweqwe", "<article><span>qweqwe</span></article>" },
            { "+w\n+ww\n+www", "<article><ul><li>w</li><li>ww</li><li>www</li></ul></article>" },
            { "№w\n№ww\n№www", "<article><ol><li>w</li><li>ww</li><li>www</li></ol></article>" },
            { "№w\n№w*w\n№www\n", "<article><ol><li>w</li><li>w<b>w</b></li><b><li>www</li></b></ol><b></b></article>" },
            { "№w*\n№ww", "<article><ol><li>w<b></b></li><b><li>ww</li></b></ol><b></b></article>" },
            { "+w\n№ww", "<article><ul><li>w</li></ul><ol><li>ww</li></ol></article>" },
            { ">>107 qqq", "<article><a href=/Home/DisplayTread/6/#107>107</a> qqq</article>"},
            { ">>1000000000", "<article>1000000000</article>" },
            { ">>100", "<article><a href=/Home/DisplayTread/7/#100>100</a></article>" }
        };

        static private void TestParser()
        {

            var contex = ApplicationDbContextFactory.CreateDbContext();

            var testOutput = new StringBuilder();
            string result;

            var i = 0;
            foreach (var testPair in _testData)
            {
                result = Parser.MarkUp(testPair.Key, contex);

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
