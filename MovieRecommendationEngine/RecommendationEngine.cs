using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;

namespace MovieRecommendationEngine
{
    
    public class RecommendationEngine
    {
        // Create a private list for the Movie dataset
        private List<Movies> _movies;
        private double[] _inputVector;
        private string _inputString;
        
        // Populate the list with multiple movies
        public RecommendationEngine(string inputString)
        {
            _inputString = inputString;
            _movies = new List<Movies>();
            // Read in the CSV file
            string csvFilePath = "imdbTop1000.csv";
            using (var reader = new CsvReader(File.OpenText(csvFilePath), CultureInfo.InvariantCulture))
            {
                _movies = reader.GetRecords<Movies>().ToList();
            }
        }
        
        // Method to preprocess the movie data by converting the text-based features into numerical vectors using TF-IDF
        private void PreprocessData()
        {
            // calculate the TF-IDF for each of the movie features
            foreach (var movie in _movies)
            {
                movie.MovieVector = CalculateTfIdf($"{movie.PlotOverview} {movie.Title} {movie.Genre}" +
                                                   $" {movie.Director} {movie.StarOne} {movie.StarTwo} {movie.StarThree}");
            }
            
            // Calculate for the input vector
            _inputVector = CalculateTfIdf(_inputString);
        }
        
        // Method to calculate the calculate the TF-IDF vector for a given input sentence
        private double[] CalculateTfIdf(string input)
        {
            // Tokenize the input sentence into individual words
            var tokens = Tokenize(input);
            
            // Calculate the word counts for each word in the input string
            var termFreq = new Dictionary<string, double>();
            foreach (var token in tokens)
            {
                if (!termFreq.ContainsKey(token))
                {
                    termFreq[token] = 0;
                }

                termFreq[token]++;
            }
            var numTokens = tokens.Length;
            
            // Calculate the term frequency (TF) for each word in the input string
            foreach (var token in termFreq.Keys.ToList())
            {
                termFreq[token] /= numTokens;
            }
            
            // Calculate the inverse document frequency (IDF) for each word in the input string 
            var inverseDocFreq = new Dictionary<string, double>();
            foreach (var token in termFreq.Keys)
            {
                var numDocsContainingToken = _movies.Count(m => m.PlotOverview.Contains(token));
                inverseDocFreq[token] = Math.Log((double)_movies.Count / numDocsContainingToken);
            }

            var tfidf = new double[termFreq.Count];
            var index = 0;

            foreach (var token in termFreq.Keys)
            {
                tfidf[index] = termFreq[token] * inverseDocFreq[token];
                index++;
            }


            return tfidf;
        }

        private string[] Tokenize(string input)
        {
            return Regex.Split(input, @"\W+") 
                .Where(w => !string.IsNullOrEmpty(w))
                .Select(w => w.ToLowerInvariant())
                .ToArray();
        }
        
        // Method to get movie Recommendations
        public List<MovieRecommendation> GetRecommendations()
        {
            PreprocessData();
            Knn knn = new Knn(_movies);
            var nearestNeighbors = knn.FindNearestNeighbours(_inputVector, 10);
            return nearestNeighbors.Select(nn => new MovieRecommendation
            {
                Movie = nn.Movie,
                Similarity = nn.Similarity
            }).ToList();
        }

        public async Task OutputMethod()
        {
            var recoMovies = GetRecommendations();
            // var result = String.Join("", recoMovies.Select(x => x.Movie.Title).ToList());
            string result = "";
            // StreamingProvider sp = new StreamingProvider(result);
            //Console.WriteLine($"{result} ");
            foreach (var m in recoMovies)
            {
                StreamingProvider sp = new StreamingProvider(m.Movie.Title);
                //int movieId = await sp.GetMovieId();
                //var streamProviders = await sp.StreamingProviders();
                result += $"{m.Movie.Title}: {m.Similarity}: {m.Movie.ImdbRating}: Available on\n";
            }
            Console.WriteLine(result);
        }
    }
}