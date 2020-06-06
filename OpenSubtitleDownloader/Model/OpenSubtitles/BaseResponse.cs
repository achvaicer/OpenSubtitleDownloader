using Newtonsoft.Json;

namespace OpenSubtitleDownloader.Model.OpenSubtitles
{
    public class BaseResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("errors")]
        public string[] Errors { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}