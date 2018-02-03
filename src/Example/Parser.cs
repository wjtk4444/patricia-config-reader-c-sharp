using System;
using System.Collections.Generic;
using System.Linq;

namespace Example
{
    static class Parser
    {
        //https://stackoverflow.com/questions/5120308
        //https://stackoverflow.com/questions/79126
        static public T parseEnum<T>(string s, T defVal, bool ignoreCase = false) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            if (string.IsNullOrEmpty(s)) return defVal;

            if (s.Any(x => Char.IsWhiteSpace(x)))
                return defVal;

            foreach (var v in Enum.GetValues(typeof(T)))
                if (String.Equals(s, v.ToString(), ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                    return (T)v;

            return defVal;
        }

        static public bool parseBool(string s, bool defVal)
        {
            if (string.IsNullOrEmpty(s)) return defVal;

            if (string.Equals(s, "True", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (string.Equals(s, "False", StringComparison.OrdinalIgnoreCase))
                return false;
            else
                return defVal;
        }

        static public int parseInt(string s, int defVal)
        {
            if (string.IsNullOrEmpty(s)) return defVal;

            bool minus = false;
            int value = 0;
            int i = 0;

            if (s[0] == '-')
            {
                minus = true;
                i++;
            }

            for (; i < s.Length; i++)
            {
                if (s[i] < '0' || s[i] > '9')
                    return defVal;
                else
                {
                    value *= 10;
                    value += s[i] - '0';
                }
            }

            return (minus ? -value : value);
        }

        static public uint parseUint(string s, uint defVal)
        {
            if (string.IsNullOrEmpty(s)) return defVal;

            uint value = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] < '0' || s[i] > '9')
                    return defVal;
                else
                {
                    value *= 10;
                    value += (uint)(s[i] - '0');
                }
            }

            return value;
        }

        static public string parseWord(string s, string defVal)
        {
            if (string.IsNullOrEmpty(s)) return defVal;

            if (s.Any(x => Char.IsWhiteSpace(x)))
                return defVal;

            return s;
        }

        static public string parseString(string s, string defVal, bool acceptWhitespace = false, bool acceptEmptyStrings = false)
        {
            if (acceptEmptyStrings && acceptWhitespace && s != null) return s;
            else if (acceptEmptyStrings && s != null) return s.Trim();
            else if (acceptWhitespace && !string.IsNullOrEmpty(s)) return s;
            else if (!string.IsNullOrEmpty(s)) return s.Trim();

            return defVal;
        }

        static public string[] parseStringArray(string s, string[] defVal, char arraySplitter = '`', bool acceptWhitespace = false, bool acceptEmptyStrings = false)
        {
            if (string.IsNullOrEmpty(s)) return defVal;

            List<string> list = new List<string>();

            //split input string to array, either including or ignoring empty entries
            string[] arr = s.Split(new char[] { arraySplitter }, acceptEmptyStrings ? StringSplitOptions.None : StringSplitOptions.RemoveEmptyEntries);

            foreach (string str in arr)
            {
                //prepare tmp string either with or without whitepaces removed
                string tmp = (acceptWhitespace ? str : str.Trim());

                //if string is not empty -> add it to the result
                if (!string.IsNullOrEmpty(tmp))
                    list.Add(tmp);
                //if string is empty, but acceptEmptyStrings option is set to true -> add it to the result as well
                else if (acceptEmptyStrings)
                    list.Add(tmp);
            }

            return (list.Count != 0 ? list.ToArray() : defVal);
        }
    }
}
