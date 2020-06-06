using Newtonsoft.Json;

namespace OpenSubtitleDownloader.Model.OpenSubtitles
{
    public class DownloadSubtitleResponse : BaseResponse
    {

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("fname")]
        public string FileName { get; set; }

        [JsonProperty("requests")]
        public int Requests { get; set; }

        [JsonProperty("allowed")]
        public int Allowed { get; set; }

        [JsonProperty("remaining")]
        public int Remaining { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}