using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace StringToInt.Library
{
    public static class StringToIntConverter
    {
        public static int Convert(string text)
        {
            ValidateInput(text);

            return ConvertString(text);
        }

        public static bool TryConvert(string text, out int result)
        {
            try
            {
                result = Convert(text);
            }
            catch
            {
                result = -1;
                return false;
            }

            return true;
        }

        private static int ConvertString(string text)
        {
            int startIndex = 0;
            int sign = 1;
            int result = 0;

            if (text[0] == '-')
            {
                sign = -1;
                startIndex++;
            }

            for (int i = startIndex; i < text.Length; i++)
            {
                int digit = text[i] - '0';
                result = checked(result * 10 + digit);
            }

            return sign * result;
        }
        
        private static void ValidateInput(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text == string.Empty
                || !Regex.IsMatch(text, "[-]?\\d+"))
            {
                throw new ArgumentException(nameof(text));
            }
        }
    }
}