using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using OSDBnet;

namespace OpenSubtitleDownloader
{
    public partial class OpenSubtitleDownloader : ServiceBase
    {
        private static Thread _thread;
        private static readonly string UserAgent = ConfigurationManager.AppSettings["UserAgent"];
        private static readonly string Language = ConfigurationManager.AppSettings["Language"];
        private static IAnonymousClient _client = Osdb.Login(Language, UserAgent);
        private static string[] Directories = ConfigurationManager.AppSettings["Directories"].Split(new [] {';'});
        private static string[] VideoExtensions = ConfigurationManager.AppSettings["VideoExtensions"].Split(new[] {';'});

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
            var extensions = string.Join("|", VideoExtensions.Select(x => string.Format("*.{0}", x)));
            while (true)
            {
                foreach (var directory in Directories)
                {
                    var files = Directory.GetFiles(directory, extensions);
                    foreach (var file in files)
                    {
                        if (!File.Exists(Path.ChangeExtension(file, "srt")))
                        {
                            var subtitle = _client.SearchSubtitlesFromFile(Language, file).FirstOrDefault();
                            if (subtitle != null)
                                _client.DownloadSubtitleToPath(Path.GetFullPath(file), subtitle);
                        }
                    }
                }
            }
        }
    }
}
