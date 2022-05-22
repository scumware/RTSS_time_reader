using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RTSS_time_reader.WindowsInterop
{
    public static class ByteUtils
    {
        public static unsafe void strncpy_s(
            SByte* p_destString,
            //int numberOfElements, //The size of the destination string, in characters.
            SByte* p_srcStr,
            int count //Number of characters to be copied, or _TRUNCATE.
        )
        {
            var i = 0;
            SByte chr = p_srcStr[i];

            var lastNumber = count - 1;
            while (chr != 0 && i < lastNumber)
            {
                p_destString[i] = chr;

                ++i;
                chr = p_srcStr[i];
            }

            while (i < lastNumber)
            {
                p_destString[i] = 0;
                ++i;
            }
        }

        public static unsafe void strcpy_s(sbyte* p_destString, string p_srcStr)
        {
            var lpSrcStr = (SByte*) Marshal.StringToHGlobalAnsi(p_srcStr);
            try
            {
                var i = 0;
                SByte chr = lpSrcStr[i];

                while (chr != 0)
                {
                    p_destString[i] = chr;

                    ++i;
                    chr = lpSrcStr[i];
                }
            }
            finally
            {
                Marshal.FreeHGlobal((IntPtr) lpSrcStr);
            }
        }

        public static unsafe int Strlen(sbyte* p_chars)
        {
            var i = 0;
            var ch = p_chars[i];

            while (ch != 0)
            {
                ++i;
                ch = p_chars[i];
            }

            return i;
        }

        public static unsafe bool StrCmp(sbyte* p_chars, string p_str)
        {
            var ascii = Encoding.ASCII.GetBytes(p_str);

            var i = 0;

            var ch = p_chars[i];

            while (ch != char.MinValue)
            {
                if (i > p_str.Length)
                    return false;
                if (ch != ascii[i])
                    return false;

                ++i;
                ch = p_chars[i];
            }

            return p_str.Length == i;
        }
    }
}
