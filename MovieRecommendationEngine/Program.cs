using System;

namespace MovieRecommendationEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            RecommendationEngine rm = new RecommendationEngine(input);
            rm.OutputMethod();
        }
    }
}