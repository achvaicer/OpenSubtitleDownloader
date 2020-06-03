using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenSubtitleDownloader.Model.OpenSubtitles
{
    public class Attributes
    {

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("download_count")]
        public int DownloadCount { get; set; }

        [JsonProperty("new_download_count")]
        public int NewDownloadCount { get; set; }

        [JsonProperty("hearing_impaired")]
        public bool HearingImpaired { get; set; }

        [JsonProperty("hd")]
        public bool Hd { get; set; }

        [JsonProperty("format")]
        public object Format { get; set; }

        [JsonProperty("fps")]
        public double Fps { get; set; }

        [JsonProperty("votes")]
        public int Votes { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("ratings")]
        public double Ratings { get; set; }

        [JsonProperty("from_trusted")]
        public bool FromTrusted { get; set; }

        [JsonProperty("auto_translation")]
        public bool AutoTranslation { get; set; }

        [JsonProperty("ai_translated")]
        public bool AiTranslated { get; set; }

        [JsonProperty("machine_translated")]
        public object MachineTranslated { get; set; }

        [JsonProperty("release")]
        public string Release { get; set; }

        [JsonProperty("feature_details")]
        public FeatureDetail FeatureDetails { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("related_links")]
        public RelatedLink RelatedLinks { get; set; }

        [JsonProperty("files")]
        public IList<File> Files { get; set; }

        [JsonProperty("subtitle_id")]
        public string SubtitleId { get; set; }
    }

}