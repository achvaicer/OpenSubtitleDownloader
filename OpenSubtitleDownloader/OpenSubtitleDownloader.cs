using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
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

        private static readonly IList<IEqualityComparer<string>> Comparers = new List<IEqualityComparer<string>>()
            {
                new ExactEqualyComparer(),
                new IgnoreCaseComparer()
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
                foreach (var directory in Directories)
                {
                    SearchFiles(directory, extensions);
                    SearchDirectories(directory, extensions);
                }
                Thread.Sleep(600000);
            }
        }

        private static void SearchDirectories(string directory, IEnumerable<string> extensions)
        {
            var directories = Directory.GetDirectories(directory);
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
            
            var subtitles = _client.SearchSubtitlesFromFile(Language, file);
            var subtitle = FindBestFit(file, subtitles);

            if (subtitle == null) return;
            DownloadSubtitle(file, subtitle);
        }

        private static void DownloadSubtitle(string file, Subtitle subtitle)
        {
            var downloaded = _client.DownloadSubtitleToPath(Path.GetDirectoryName(file), subtitle);
            var comp = new ExactEqualyComparer();
            if (!comp.Equals(file, downloaded))
                File.Move(downloaded, Path.ChangeExtension(file, Path.GetExtension(downloaded)));
        }

        private static Subtitle FindBestFit(string file, IList<Subtitle> subtitles)
        {
            foreach (var comparer in Comparers)
            {
                var subtitle = subtitles.FirstOrDefault(x => comparer.Equals(x.SubtitleFileName, file));
                if (subtitle != null)
                    return subtitle;
            }
            return null;
        }
    }
}
