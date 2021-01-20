using System;
using System.Collections.Generic;

namespace Imageboard.WakabaMark
{
    public class WakabaMark
    {
        public static void MakeMarkUp(string sourse)
        {
            Dictionary<Mark, int> marksDict = new Dictionary<Mark, int>();
            char[] value = sourse.ToCharArray();
            MarkUp(ref value, marksDict);
        }
        private static void MarkUp(ref char[] value, Dictionary<Mark, int> marksDict)
        {
            var i = 0;
            while (i < value.Length)
            {
                var mappedValue = CharToMarkMapper.Map(value[i]);
                if (mappedValue != Mark.None)
                {
                    if (marksDict.ContainsKey(mappedValue))
                    {
                        value[marksDict[mappedValue]] = MarkToHtmlMapper.Map(mappedValue);
                        value[i] = MarkToHtmlMapper.SecondMap(mappedValue);
                        MarkUp(ref value, marksDict);
                    }
                    marksDict.Add(mappedValue, i);
                    MarkUp(ref value, marksDict);
                    return;
                }
                i++;
            }
        }
        static private void ChangeChars(ref char value)
        {
            
        }
    }
}
