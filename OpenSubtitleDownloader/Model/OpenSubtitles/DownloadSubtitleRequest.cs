using Newtonsoft.Json;

namespace OpenSubtitleDownloader.Model.OpenSubtitles
{
    public class DownloadSubtitleRequest
    {

        [JsonProperty("file_id")]
        public long FileId { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("sub_format")]
        public string SubFormat { get; set; }
    }
}