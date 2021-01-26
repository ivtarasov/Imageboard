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
            var result = Parser.MarkUp(sourse);
            if (result == expectedResult)
            {
                stringBuilder.Append("First test passed.\n")
                             .Append($"#########\n");
            } else
            {
                stringBuilder.Append("First test failed.\n")
                             .Append($"Expected result: {expectedResult}\n")
                             .Append($"The result: {result}\n")
                             .Append($"#########\n");
            }
            return stringBuilder.ToString();
        }
        static private string SecondTest()
        {
            var sourse = "|*_*_|";
            var expectedResult = "|{()}()|";
            var stringBuilder = new StringBuilder();
            var result = Parser.MarkUp(sourse);
            if (result == expectedResult)
            {
                stringBuilder.Append("Second test passed.\n")
                             .Append($"#########\n");
            }
            else
            {
                stringBuilder.Append("Second test failed.\n")
                             .Append($"Expected result: {expectedResult}\n")
                             .Append($"The result: {result}\n")
                             .Append($"#########\n");
            }
            return stringBuilder.ToString();
        }
        static private string ThirdTest()
        {
            var sourse = "|*_#*_#|";
            var expectedResult = "|{(<>)}(<>)<>|";
            var stringBuilder = new StringBuilder();
            var result = Parser.MarkUp(sourse);
            if (result == expectedResult)
            {
                stringBuilder.Append("Third test passed.\n")
                             .Append($"#########\n");
            }
            else
            {
                stringBuilder.Append("Third test failed.\n")
                             .Append($"Expected result: {expectedResult}\n")
                             .Append($"The result: {result}\n")
                             .Append($"#########\n");
            }
            return stringBuilder.ToString();
        }
        static private string FourthTest()
        {
            var sourse = "||";
            var expectedResult = "||";
            var stringBuilder = new StringBuilder();
            var result = Parser.MarkUp(sourse);
            if (result == expectedResult)
            {
                stringBuilder.Append("Fourth test passed.\n")
                             .Append($"#########\n");
            }
            else
            {
                stringBuilder.Append("Fourth test failed.\n")
                             .Append($"Expected result: {expectedResult}\n")
                             .Append($"The result: {result}\n")
                             .Append($"#########\n");
            }
            return stringBuilder.ToString();
        }
        static private string FifthTest()
        {
            var sourse = "|*#*_#_|";
            var expectedResult = "|{<>}<()>()|";
            var stringBuilder = new StringBuilder();
            var result = Parser.MarkUp(sourse);
            if (result == expectedResult)
            {
                stringBuilder.Append("Fifth test passed.\n")
                             .Append($"#########\n");
            }
            else
            {
                stringBuilder.Append("Fifth test failed.\n")
                             .Append($"Expected result: {expectedResult}\n")
                             .Append($"The result: {result}\n")
                             .Append($"#########\n");
            }
            return stringBuilder.ToString();
        }
        static void Main(string[] args)
        {
            Console.WriteLine(Test.FirstTest());
            Console.WriteLine(Test.SecondTest());
            Console.WriteLine(Test.ThirdTest());
            Console.WriteLine(Test.FourthTest());
            Console.WriteLine(Test.FifthTest());
        }
    }
}
