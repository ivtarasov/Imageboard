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
        private static void MarkUp(ref char[]  value, Dictionary<Mark, int> marksDict)
        {
            for (int i = 0; i < value.Length; i++)
            {
                var mappedValue = CharToMarkMapper.Map(value[i]);
                switch (mappedValue)
                {
                    case Mark.Code:
                        if (marksDict.ContainsKey(mappedValue))
                        {
                            value[marksDict[mappedValue]] = MarkToHtmlMapper.Map(mappedValue);
                            value[i] = MarkToHtmlMapper.SecondMap(mappedValue);
                            MarkUp(ref value, marksDict);
                        }
                        marksDict.Add(mappedValue, i);
                        MarkUp(ref value, marksDict);
                        return;
                    case Mark.Bold:
                        return;
                    case Mark.Italic:
                        return;
                    case Mark.Spoler:
                        return;
                    default:
                        return;
                }

            }
        }
        static private void ChangeChars(ref char value)
        {
            
        }
    }
}
