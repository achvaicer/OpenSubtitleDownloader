using Newtonsoft.Json;

namespace OpenSubtitleDownloader.Model.OpenSubtitles
{
    public class SearchReleaseRequest
    {

        [JsonProperty("languages")]
        public string Languages { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("type")]
        public string @Type { get; set; }

        [JsonProperty("moviehash")]
        public string MovieHash { get; set; }

        
    }
}