using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using OpenSubtitleDownloader.Config;

using OpenSubtitleDownloader.Comparers;
using OpenSubtitleDownloader.Model;
using OpenSubtitleDownloader.Model.OpenSubtitles;

namespace OpenSubtitleDownloader
{
    public partial class Worker 
    {
        private readonly RuntimeConfig _runtimeConfig;
        private readonly OpenSubtitlesClient _client;

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
            _client = new OpenSubtitlesClient(_runtimeConfig.OpenSubtitles);
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
                if (!System.IO.File.Exists(Path.ChangeExtension(file, "srt")))
                {
                    SearchSubtitle(file);
                }
            }
        }

        private void SearchSubtitle(string file)
        {
            try
            {
                var filename = Path.GetFileName(file);
                var hash = HashGenerator.ComputeMovieHash(file);
                var subtitles = _client.SearchMovieSubtitles(filename, hash);
                var subtitleId = FindBestFit(file, subtitles);

                if (subtitleId == null) return;
                DownloadSubtitle(file, subtitleId.Value);
            }
            catch (System.Exception exception)
            {
                //_logger.Error("Error trying to download subtitle for {0}./n Exception {1}", file, exception.Message);
            }
        }

        private void DownloadSubtitle(string file, long subtitleId)
        {
            var downloaded =  _client.DownloadSubtitle(Path.GetFileName(file), subtitleId, Path.GetFullPath(file));
            var comp = new ExactEqualyComparer();
            if (comp.Equals(file, downloaded)) return;
            System.IO.File.Move(downloaded, Path.ChangeExtension(file, Path.GetExtension(downloaded)));
            //_logger.Info("Downloaded subtitle for {0}", file);
        }

        private long? FindBestFit(string file, IList<Data> subtitles)
        {
            var filename = Path.GetFileNameWithoutExtension(file);
            foreach (var comparer in Comparers)
            {
                var subtitle = subtitles.FirstOrDefault(x => comparer.Equals(Path.GetFileNameWithoutExtension(x.Attributes.Files.FirstOrDefault()?.FileName ?? ""), filename));
                if (subtitle != null)
                    return subtitle.Attributes.Files.First().Id;
            }
            return null;
        }
    }
}
