using System.Collections.Generic;
using System.IO;
using System.Net;
using OpenSubtitleDownloader.Config;
using OpenSubtitleDownloader.Model.OpenSubtitles;

using RestSharp;

namespace OpenSubtitleDownloader
{
    public class OpenSubtitlesClient
    {
        private readonly RestClient _client;
        private readonly string _token;
        private readonly OpenSubtitlesConfig _config;
    
        public OpenSubtitlesClient(OpenSubtitlesConfig config)
        {
            _config = config;
            _client = new RestClient("https://www.opensubtitles.com/api/v1/");
            _token = Login();
        }

        private string Login()
        {
            var request = new RestRequest("login", Method.POST);
            request.AddJsonBody(new LoginRequest() { username = _config.Login, password = _config.Password });
            request.AddHeader("User-Agent", _config.UserAgent);

            var response = _client.Post<LoginResponse>(request);
            
            if (response.Data.Status == 200)
                return response.Data.Token;
            return null;
        }

        public IList<Data> SearchMovieSubtitles(string name, string hash)
        {
            return SearchSubtitles(name, hash, "Movie");
        }

        public IList<Data> SearchSerieSubtitles(string name, string hash)
        {
            return SearchSubtitles(name, hash, "Episode");
        }

        private IList<Data> SearchSubtitles(string name, string hash, string type)
        {
            var request = new RestRequest("search", Method.POST);
            request.AddParameter("languages", _config.Language);
            request.AddParameter("query", name);
            request.AddParameter("type", type);
            request.AddParameter("moviehash", hash);
            request.AddHeader("Authorization", _token);

            var response = _client.Post<SearchReleaseResponse>(request);
            return response.Data.Data;
        }

        public string DownloadSubtitle(string filename, long subtitileId, string path)
        {
            var request = new RestRequest("download", Method.POST);
            request.AddJsonBody(new DownloadSubtitleRequest { FileId = subtitileId, FileName = filename, SubFormat = "srt"});
            request.AddHeader("Authorization", _token);
            
            var response = _client.Post<DownloadSubtitleResponse>(request);
            var webclient = new WebClient();
            string fileName = Path.Combine(path, response.Data.FileName);
            webclient.DownloadFile(response.Data.Link, fileName);
            return fileName;
        }
    }
}