using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace OpenSubtitleDownloader.Config
{
    public class RuntimeConfig
    {
        public RuntimeConfig(IConfigurationRoot config)
        {
            Extensions = new List<string>();

            var count = 0;
            while(true)
            {
                if (config[$"Extensions:{count}"] == null) break;
                Extensions.Add(config[$"Extensions:{count++}"]);
            }
            Directories = new Directory(config);
            OpenSubtitles = new OpenSubtitlesConfig(config);
        }
        public OpenSubtitlesConfig OpenSubtitles { get; set; }
        public Directory Directories { get; set; }
        public IList<string> Extensions { get; set; }
    }
}
