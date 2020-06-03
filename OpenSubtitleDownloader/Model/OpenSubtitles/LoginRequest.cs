using Newtonsoft.Json;

namespace OpenSubtitleDownloader.Model.OpenSubtitles
{
    public class LoginRequest
    {

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}