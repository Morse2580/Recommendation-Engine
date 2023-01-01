using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieRecommendationEngine
{
    public class Knn
    {
        public List<Movies> _movies;
        public Knn(List<Movies> movies)
        {
            _movies = movies;
        }

        private double Calculate_EDistance(double[] p, double[] q)
        {
            var distance = 0.0;

            for (int i = 0; i < p.Length; i++)
            {
                distance += Math.Pow(p[i] - q[i], 2);
            }
            return Math.Sqrt(distance);
        }

        public class NearestNeighbour
        {
            public Movies Movie { get; set; }
            public double Similarity { get; set; }
        }

        public List<NearestNeighbour> FindNearestNeighbours(double[] testdata, int k)
        {
            var distances = new List<NearestNeighbour>();

            foreach (var movie in _movies)
            {
                var distance = Calculate_EDistance(testdata, movie.MovieVector);
                distances.Add(new NearestNeighbour
                {
                    Movie = movie,
                    Similarity = distance
                });
            }
            
            // sort the vectors in the dataset based on their distance from the input vector
            distances = distances.OrderBy(d => d.Similarity).ToList();

            return distances.Take(k).ToList();
        }
    }
}