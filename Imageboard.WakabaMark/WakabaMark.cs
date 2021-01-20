using System;
using System.Collections.Generic;

namespace Imageboard.Markup
{
    public static class WakabaMark
    {
        public static string MakeMarkUp(string sourse)
        {
            Dictionary<Marks, int> marksPosition = new Dictionary<Marks, int>();
            char[] value = sourse.ToCharArray();
            MarkUp(ref value, marksPosition);
            return new string(value);

        }
        private static void MarkUp(ref char[] value, Dictionary<Marks, int> marksPosition)
        {
            var i = 0;
            while (i < value.Length)
            {
                var mappedValue = CharToMarkMapper.Map(value[i]);
                if (mappedValue != Marks.None)
                {
                    if (marksPosition.ContainsKey(mappedValue))
                    {
                        foreach (var mark in marksPosition)
                        {
                            Console.WriteLine($"{mark.Key} -- {mark.Value}");
                        }
                        value[marksPosition[mappedValue]] = MarkToHtmlMapper.Map(mappedValue);
                        value[i] = MarkToHtmlMapper.SecondMap(mappedValue);
                        marksPosition.Remove(mappedValue);
                    }
                    marksPosition.Add(mappedValue, i);
                }
                i++;
            }
        }
    }
}
