using CsvHelper.Configuration.Attributes;

namespace MovieRecommendationEngine
{
    // This class represents the movie within the dataset
    public class Movies
    {
        [Name("Series_Title")]
        public string Title { get; set; }

        [Name("Genre")] 
        public string Genre { get; set; }
        
        [Name("Overview")] 
        public string PlotOverview { get; set; }
        
        [Name("Director")] 
        public string Director { get; set; }
        
        [Name("Star1")] 
        public string StarOne { get; set; }
        
        [Name("Star2")] 
        public string StarTwo { get; set; }
        
        [Name("Star3")] 
        public string StarThree { get; set; }
        
        [Name("IMDB_Rating")] 
        public float ImdbRating { get; set; }
        public double[] MovieVector { get; set; }
    }
}