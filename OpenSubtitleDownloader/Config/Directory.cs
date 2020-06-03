using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace OpenSubtitleDownloader.Config
{
    public class Directory
    {
        public Directory(IConfigurationRoot config)
        {
            Movies = GetValues(config, "Directories:Movies");
            Series = GetValues(config, "Directories:Series");

            
        }
        public IList<string> Movies { get; set; }
        public IList<string> Series { get; set; }

        private IList<string> GetValues(IConfigurationRoot config, string key)
        {
            var list = new List<string>();

            var count = 0;
            while(true)
            {
                if (config[$"{key}:{count}"] == null) break;
                list.Add(config[$"{key}:{count++}"]);
            }
            return list;
        }
    }
}