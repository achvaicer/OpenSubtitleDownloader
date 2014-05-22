using System.Collections.Generic;
using System.IO;

namespace OpenSubtitleDownloader.Comparers
{
    class ExactEqualyComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return Path.GetFileNameWithoutExtension(x) == Path.GetFileNameWithoutExtension(y);
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
