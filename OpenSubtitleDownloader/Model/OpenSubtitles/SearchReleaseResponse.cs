using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenSubtitleDownloader.Model.OpenSubtitles
{
    public class SearchReleaseResponse
    {

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("data")]
        public IList<Data> Data { get; set; }
    }

}