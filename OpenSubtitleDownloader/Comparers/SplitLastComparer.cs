using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSubtitleDownloader.Comparers
{
    public class SplitLastComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            var lastx = Split(x).Last().ToLower();
            var lasty = Split(y).Last().ToLower();
            return lastx == lasty;
        }

        private static string[] Split(string x)
        {
            return x.Split(new char[] {'.', '-'});
        }

        public int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
