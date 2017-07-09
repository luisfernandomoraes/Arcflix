using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.TMDb;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Arcflix.Services.Api
{
    public class TMDBExtendedApi : ITMDBExtendedApi
    {
        readonly HttpClient _client;
        private string _apiKey;
        private const string BaseAddress = @"https://api.themoviedb.org/3";


        public TMDBExtendedApi(string apiKey)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));
            _apiKey = apiKey;
        }

        /// <summary>
        /// Method that returns details from the specific movie in TmDB Api. 
        /// </summary>
        /// <param name="movieId">The movie Id.</param>
        /// <param name="language">The required language.</param>
        /// <returns></returns>
        public async Task<Movie> GetMovieDetailsAsync(int movieId, string language)
        {
            Movie result = null;
            try
            {
                string resourcePath = $@"{BaseAddress}/movie/{movieId}?api_key={_apiKey}&language={language}";

                var uri = new Uri(resourcePath);

                var response = await _client.GetAsync(uri);

                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<Movie>(content);

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return result;
            }
        }

        public Task<Show> GetShowDetailsAsync(int showId, string language)
        {
            throw new NotImplementedException();
        }
    }
}