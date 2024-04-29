using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace Utils
{
    public class StringUtils // Functions *partly* taken from Youtube Playlist Utility
    {
        public static int[] checkForIn(string checkFor, string checkIn)
        {
            int[] returnArray = new int[20];
            for (int i = 0; i < returnArray.Length; i++)
            {
                returnArray[i] = -1;
            }
            string checkInTemp = checkIn;
            int arrayPos = 0;
            if (checkFor.Length <= checkIn.Length)
            {
                for (int i = 0; i <= checkIn.Length - checkFor.Length; i++)
                {
                    //checkInTemp = deleteInterval(checkInTemp, i, checkInTemp.Length);
                    for (int j = 0; j < checkFor.Length; j++)
                    {
                        if (checkFor[j] != checkIn[i + j])
                        {
                            break;
                        }
                        if (j == checkFor.Length - 1)
                        {
                            returnArray[arrayPos] = i;
                            arrayPos++;
                        }
                    }
                }
            }
            return returnArray;
        }
        public static string deleteInterval(string text, int from, int to) //can be composed easily; from==0 --> char 0 deleted, to>=length --> last char deleted; from==to --> one char at that pos deleted
        {
            string temp = "";
            //int j = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (!(i >= from && i <= to))
                {
                    temp = temp + text[i];
                    //temp[j] = text[i];
                    //j++;
                }
            }
            return temp;
        }
        public static string getInterval(string text, int from, int to) // fully inclusive
        {
            return deleteInterval(deleteInterval(text, 0, from - 1), to - from + 1, text.Length); // text.Length always greater so it is fine
        }

        public static float expNotationToFloat(string expText) // e.g. -2.0952600000e-02 to -0.029526f
        {
            int decimalsDigits = 10;
            int expDigits = 2;
            short isNegative = 0;
            if (expText[0] == '-')
            {
                isNegative = 1;
            }
            string left = getInterval(expText, 0, decimalsDigits +1+isNegative);
            string expSignString = getInterval(expText, decimalsDigits +3+ isNegative, decimalsDigits +3+ isNegative);
            string right = getInterval(expText, decimalsDigits +4+ isNegative, decimalsDigits +5+ isNegative);

            int sign = 0;
            if (expSignString == "+")
            {
                sign = 1;
            }
            if (expSignString == "-")
            {
                sign = -1;
            }
            return float.Parse(left)*MathF.Pow(10,sign* float.Parse(right));
        }
    }
}
