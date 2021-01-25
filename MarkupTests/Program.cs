using System;
using Imageboard.Markup;

namespace MarkupTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourse = "|te*sdfsfr*eet|";
            Console.WriteLine(WakabaMark.MakeMarkup(sourse));
        }
    }
}
