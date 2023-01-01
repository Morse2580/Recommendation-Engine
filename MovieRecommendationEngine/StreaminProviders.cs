using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using Newtonsoft.Json;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
namespace MovieRecommendationEngine
// https://github.com/LordMike/TMDbLib
{
    public class StreamingProvider
    {
        const string _apiKey = "86fcfda0891ee84831c70e27491391c1";
        public string _movieTitle;
        HttpClient _client = new HttpClient();
        public List<string> streamingProviders;
        public StreamingProvider(string movieTitle)
        {
            _movieTitle = movieTitle;
        }

        public async Task<int> GetMovieId()
        {
            // create client for API
            TMDbClient client = new TMDbClient(_apiKey);

            SearchContainer<SearchMovie> results = await client.SearchMovieAsync(_movieTitle);
            SearchMovie movie = results.Results.FirstOrDefault();
            int id = 0;
            if (movie != null) id += movie.Id;
            return id;
        }

        public async Task<string> ImdBid()
        {
            // Get the movie's details
            string url = $"https://api.themoviedb.org/3/movie/{GetMovieId()}?api_key={_apiKey}";
            HttpResponseMessage response = await _client.GetAsync(url);
            string responseJson = await response.Content.ReadAsStringAsync();
            
            // Parse the response to get the IMDb ID of the movie
            var movie = JsonConvert.DeserializeObject<Movie>(responseJson);
            var imdbId = "";
            if (movie != null)
            {
                imdbId = movie.ImdbId;
            }
            return imdbId;
        }
        public async Task<string> StreamingProviders()
        {
            // Get the movie's details
            string url = $"https://api.themoviedb.org/3/movie/{GetMovieId()}/watch/providers?api_key={_apiKey}";
            HttpResponseMessage response = await _client.GetAsync(url);
            string responseJson = await response.Content.ReadAsStringAsync();
            
            // Parse the response to get the IMDb ID of the movie
            List<string> result = JsonConvert.DeserializeObject<List<string>>(responseJson);
            if (result != null)
            {
                return string.Join(" ", result);
            }

            return "nowhere available";
        }
    }
}