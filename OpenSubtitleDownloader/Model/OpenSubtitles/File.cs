using Newtonsoft.Json;

namespace OpenSubtitleDownloader.Model.OpenSubtitles
{
    public class File
    {

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("cd_number")]
        public int CdNumber { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }
    }

}