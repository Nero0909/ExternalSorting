using System;
using System.Text;

namespace ExternalSorting.Infrastructure
{
    public class SizeHelper
    {
        private static readonly Encoding Encoding;
        private static readonly int NewLineSize;

        static SizeHelper()
        {
            Encoding = new UTF8Encoding(false);
            NewLineSize = Encoding.GetByteCount(Environment.NewLine);
        }


        public static int GetLineBytes(string line)
        {
            return Encoding.GetByteCount(line) + NewLineSize;
        }
    }
}