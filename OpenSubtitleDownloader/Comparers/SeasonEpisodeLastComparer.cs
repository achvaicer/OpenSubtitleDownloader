using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSubtitleDownloader.Comparers
{
    public class SeasonEpisodeLastComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            var xse = SeasonEpisode.Extract(x);
            var yse = SeasonEpisode.Extract(y);
            var lastx = Split(x).Last().ToLower();
            var lasty = Split(y).Last().ToLower();

            return xse != null && yse != null && xse.Season == yse.Season && xse.Episode == yse.Episode && lastx == lasty;
        }

        private static string[] Split(string x)
        {
            return x.Split(new char[] { '.', '-' });
        }

        public int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
