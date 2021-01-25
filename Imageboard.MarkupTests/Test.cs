using System;
using Imageboard.Markup;
using System.Text;

namespace MarkupTests
{
    class Test
    {
        static private string FirstTest()
        {
            var sourse = "|**|";
            var expectedResult = "|{}|";
            var stringBuilder = new StringBuilder();
            var result = Markup.MakeMarkup(sourse);
            if (result == expectedResult)
            {
                stringBuilder.Append("First test passed.\n")
                             .Append($"#####\n");
            } else
            {
                stringBuilder.Append("First test failed.\n")
                             .Append($"Expected result: {expectedResult}\n")
                             .Append($"The result: {result}\n")
                             .Append($"#####\n");
            }
            return stringBuilder.ToString();
        }
        static void Main(string[] args)
        {
            Console.WriteLine(Test.FirstTest());
        }
    }
}
