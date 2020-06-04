using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace OpenSubtitleDownloader.Model.OpenSubtitles
{
    [DataContract]
    public class LoginRequest
    {

        [JsonProperty("username")]
        [DataMember(Name = "username")]
        public string username { get; set; }

        [JsonProperty("password")]
        [DataMember(Name = "password")]
        public string password { get; set; }
    }
}