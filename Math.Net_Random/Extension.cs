using System;

namespace MathDotNET.Random
{
    /// <summary>
    /// An extension to the random class object
    /// </summary>
    public static class Extension
    {
        private const double RND_U_BOUND = 0.99999999999999978;
        /// <summary>
        /// Generates a random variable with a gaussian distribution probability.
        /// </summary>
        /// <param name="RndVar"></param>
        /// <param name="mean">Data mean value</param>
        /// <param name="stdDev">Standard deviation of values</param>
        /// <returns></returns>
        public static double Gaussian(this System.Random RndVar, double mean, double stdDev)
        {
            double u1 = RndVar.NextDouble(); //these are uniform(0,1) random doubles
            double u2 = RndVar.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            return mean + stdDev * randStdNormal;
        }
        /// <summary>
        /// Generates a random variable with a binomial distribution probability.
        /// </summary>
        /// <param name="RndVar"></param>
        /// <param name="n">Number of parameters</param>
        /// <param name="p">Probability of success</param>
        /// <returns></returns>
        public static double Binomial(this System.Random RndVar, int n, double p)
        {
            double log_q = Math.Log(1.0 - p);
            int x = 0;
            double sum = 0;
            for (; ; )
            {
                sum += Math.Log(RndVar.NextDouble() - RND_U_BOUND + 1) / (n - x);
                if (sum < log_q)
                {
                    return x;
                }
                x++;
            }
        }
    }
}
