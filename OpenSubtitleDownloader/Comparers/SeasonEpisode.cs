using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenSubtitleDownloader.Comparers
{
    internal class SeasonEpisode
    {
        private SeasonEpisode()
        {
            
        }

        public string Season { get; set; }
        public string Episode { get; set; }

        public static SeasonEpisode Extract(string input)
        {
            var regex = new Regex("(.*?)[.\\s][sS](\\d{2})[eE](\\d{2}).*");
            if (!regex.IsMatch(input)) return null;
            var matches = regex.Matches(input);
            return new SeasonEpisode() { Season = matches[0].Groups[2].Value, Episode = matches[0].Groups[3].Value };
        }
    }
}
