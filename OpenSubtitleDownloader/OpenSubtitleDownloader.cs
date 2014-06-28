using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using NLog;
using OSDBnet;
using OpenSubtitleDownloader.Comparers;

namespace OpenSubtitleDownloader
{
    public partial class OpenSubtitleDownloader : ServiceBase
    {
        private static Thread _thread;
        private static readonly string UserAgent = ConfigurationManager.AppSettings["UserAgent"];
        private static readonly string Language = ConfigurationManager.AppSettings["Language"];
        private static readonly IAnonymousClient _client = Osdb.Login(Language, UserAgent);
        private static readonly string[] Directories = ConfigurationManager.AppSettings["Directories"].Split(new [] {';'});
        private static readonly string[] VideoExtensions = ConfigurationManager.AppSettings["VideoExtensions"].Split(new[] {';'});
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static readonly IList<IEqualityComparer<string>> Comparers = new List<IEqualityComparer<string>>()
            {
                new ExactEqualyComparer(),
                new IgnoreCaseComparer(),
                new SeasonEpisodeLastComparer(),
                new SplitLastComparer(),
                new SeasonEpisodeComparer()
            };

        public OpenSubtitleDownloader()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _thread = new Thread(LoopCheck);
            _thread.Start();
        }

        protected override void OnStop()
        {
            _thread.Abort();
        }

        internal void LoopCheck()
        {
            var extensions = VideoExtensions.Select(x => string.Format("*.{0}", x));
            while (true)
            {
                IterateDirectories(extensions, Directories);
                Thread.Sleep(600000);
            }
        }

		internal void SingleExecution()
		{
			var extensions = VideoExtensions.Select(x => string.Format("*.{0}", x));
			IterateDirectories(extensions, Directories);
		}


        private static void SearchDirectories(string directory, IEnumerable<string> extensions)
        {
            var directories = Directory.GetDirectories(directory);
            IterateDirectories(extensions, directories);
        }

        private static void IterateDirectories(IEnumerable<string> extensions, string[] directories)
        {
            foreach (var dir in directories)
            {
                SearchFiles(dir, extensions);
                SearchDirectories(dir, extensions);
            }
        }

        private static void SearchFiles(string directory, IEnumerable<string> extensions)
        {
            var files =  extensions.SelectMany(x => Directory.EnumerateFiles(directory, x));
            foreach (var file in files)
            {
                if (!File.Exists(Path.ChangeExtension(file, "srt")))
                {
                    SearchSubtitle(file);
                }
            }
        }

        private static void SearchSubtitle(string file)
        {
            try
            {
                var subtitles = _client.SearchSubtitlesFromFile(Language, file);
                var subtitle = FindBestFit(file, subtitles);

                if (subtitle == null) return;
                DownloadSubtitle(file, subtitle);
            }
            catch (System.Exception exception)
            {
                _logger.Error("Error trying to download subtitle for {0}./n Exception {1}", file, exception.Message);
            }
        }

        private static void DownloadSubtitle(string file, Subtitle subtitle)
        {
            var downloaded = _client.DownloadSubtitleToPath(Path.GetDirectoryName(file), subtitle);
            var comp = new ExactEqualyComparer();
            if (comp.Equals(file, downloaded)) return;
            File.Move(downloaded, Path.ChangeExtension(file, Path.GetExtension(downloaded)));
            _logger.Info("Downloaded subtitle for {0}", file);
        }

        private static Subtitle FindBestFit(string file, IList<Subtitle> subtitles)
        {
            var filename = Path.GetFileNameWithoutExtension(file);
            foreach (var comparer in Comparers)
            {
                var subtitle = subtitles.FirstOrDefault(x => comparer.Equals(Path.GetFileNameWithoutExtension(x.SubtitleFileName), filename));
                if (subtitle != null)
                    return subtitle;
            }
            return null;
        }
    }
}
