using System;
using Imageboard.Markup;
using System.Text;
using System.Collections.Generic;

namespace MarkupTests
{
    class Test
    {
        private static readonly Dictionary<string, string> _testData = new Dictionary<string, string>
        {
            { "**", "((({})))" },
            { "*_*_", "((({()}())))" },
            { "*_#*_#", "((({(<>)}(<>)<>)))"},
            { "", "((()))" },
            { "*#qq\n*_#_", "((({<qq\n>}<()>())))" },
            { "_", "(((())))" },
            { ">qweqwe", "(((QqweqweQ)))" },
            { "+w\n+ww\n+www", "(((UNLlewlelewwlelewwwleUNL)))" },
            { "№w\n№ww\n№www", "(((OLlewlelewwlelewwwleOL)))" },
            { "№w\n№w*w\n№www\n", "(((OLlewlelew{w}le{lewwwle}OL{})))" },
            { "№w*\n№ww", "(((OLlew{}le{lewwle}OL{})))" },
            { "+w\n№ww", "(((UNLlewleUNLOLlewwleOL)))" }
        };

        static private void TestParser()
        {
            var testOutput = new StringBuilder();
            string result;

            var i = 0;
            foreach (var testPair in _testData)
            {
                result = Parser.MarkUp(testPair.Key);

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
