using Microsoft.Extensions.Configuration;

namespace OpenSubtitleDownloader.Config
{
    public class OpenSubtitles
    {
        public OpenSubtitles(IConfigurationRoot config)
        {
            UserAgent = config["OpenSubtitles:UserAgent"];
            Language = config["OpenSubtitles:Language"];
            Login = config["OpenSubtitles:Login"];
            Password = config["OpenSubtitles:Password"];
        }
        public string UserAgent { get; set; }
        public string Language { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}