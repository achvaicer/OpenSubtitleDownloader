using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenSubtitleDownloader.Comparers
{
    public class SeasonEpisodeComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            var xse = SeasonEpisode.Extract(x);
            var yse = SeasonEpisode.Extract(y);
            return xse != null && yse != null && xse.Season == yse.Season && xse.Episode == yse.Episode;
        }

        public int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }

        
    }
}
