using Newtonsoft.Json;

namespace OpenSubtitleDownloader.Model.OpenSubtitles
{
    public class FeatureDetail
    {

        [JsonProperty("feature_id")]
        public int FeatureId { get; set; }

        [JsonProperty("feature_type")]
        public string FeatureType { get; set; }

        [JsonProperty("movie_name")]
        public string MovieName { get; set; }

        [JsonProperty("imdbid")]
        public int Imdbid { get; set; }

        [JsonProperty("tmdbid")]
        public int Tmdbid { get; set; }
    }

}