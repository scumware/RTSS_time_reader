namespace RTSS_time_reader
{
    public static class StringExtention
    {
        public static unsafe string ReplaceInplaceLeadingChars(this string p_str, char p_c1, char p_c2)
        {
            if (string.IsNullOrEmpty(p_str))
                return p_str;

            var countDown = p_str.Length -1;
            fixed (char* p = p_str)
            {
                var pchar = p;
                while (*pchar == p_c1)
                {
                    *pchar = p_c2;
                    pchar++;
                    --countDown;

                    if (countDown == 0)
                        break;
                }
            }

            return p_str;
        }
    }
}