using System;

namespace MathDotNET.Random
{
    /// <summary>
    /// An extension to the random class object
    /// </summary>
    public static class Extension
    {
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
    }
}
