using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using OpenSubtitleDownloader.Config;

using OpenSubtitleDownloader.Comparers;

namespace OpenSubtitleDownloader
{
    public partial class Worker 
    {
        private readonly RuntimeConfig _runtimeConfig;
        

        private readonly IList<IEqualityComparer<string>> Comparers = new List<IEqualityComparer<string>>()
            {
                new ExactEqualyComparer(),
                new IgnoreCaseComparer(),
                new SeasonEpisodeLastComparer(),
                new SplitLastComparer(),
                new SeasonEpisodeComparer()
            };

        public Worker(RuntimeConfig runtimeConfig)
        {
            _runtimeConfig = runtimeConfig;

        }

        

		internal void SingleExecution()
		{
			var extensions = _runtimeConfig.Extensions.Select(x => string.Format("*.{0}", x));
			IterateDirectories(extensions, _runtimeConfig.Directories.Movies.ToArray());
            IterateDirectories(extensions, _runtimeConfig.Directories.Series.ToArray());
		}


        private void SearchDirectories(string directory, IEnumerable<string> extensions)
        {
            var directories = System.IO.Directory.GetDirectories(directory);
            IterateDirectories(extensions, directories);
        }

        private void IterateDirectories(IEnumerable<string> extensions, string[] directories)
        {
            foreach (var dir in directories)
            {
                SearchFiles(dir, extensions);
                SearchDirectories(dir, extensions);
            }
        }

        private void SearchFiles(string directory, IEnumerable<string> extensions)
        {
            if (!System.IO.Directory.Exists(directory)) return;
            var files =  extensions.SelectMany(x => System.IO.Directory.EnumerateFiles(directory, x));
            foreach (var file in files)
            {
                if (!File.Exists(Path.ChangeExtension(file, "srt")))
                {
                    SearchSubtitle(file);
                }
            }
        }

        private void SearchSubtitle(string file)
        {
            try
            {
                var subtitles = new string[] {};//_client.SearchSubtitlesFromFile(Language, file);
                var subtitle = FindBestFit(file, subtitles);

                if (subtitle == null) return;
                DownloadSubtitle(file, subtitle);
            }
            catch (System.Exception exception)
            {
                //_logger.Error("Error trying to download subtitle for {0}./n Exception {1}", file, exception.Message);
            }
        }

        private void DownloadSubtitle(string file, string subtitle)
        {
            var downloaded =  "";//_client.DownloadSubtitleToPath(Path.GetDirectoryName(file), subtitle);
            var comp = new ExactEqualyComparer();
            if (comp.Equals(file, downloaded)) return;
            File.Move(downloaded, Path.ChangeExtension(file, Path.GetExtension(downloaded)));
            //_logger.Info("Downloaded subtitle for {0}", file);
        }

        private string FindBestFit(string file, IList<string> subtitles)
        {
            var filename = Path.GetFileNameWithoutExtension(file);
            foreach (var comparer in Comparers)
            {
                var subtitle = subtitles.FirstOrDefault(x => comparer.Equals(Path.GetFileNameWithoutExtension(x/*.SubtitleFileName*/), filename));
                if (subtitle != null)
                    return subtitle;
            }
            return null;
        }
    }
}
