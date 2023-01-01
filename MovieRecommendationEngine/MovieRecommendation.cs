using System.Collections.Generic;

namespace MovieRecommendationEngine
{
    // Class to represent a movie recommendation based on a nearest neighbor
    public class MovieRecommendation
    {
        public Movies Movie { get; set; }
        public double Similarity { get; set; }
        
    }
}
