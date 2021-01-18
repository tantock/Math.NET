using System;
using System.Collections.Generic;
using System.Linq;

namespace MathDotNET.Collections.Statistics
{
    public static class Linq
    {
        /// <summary>
        /// Returns the population standard deviation of values
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double StandardDeviationP(this IEnumerable<double> values)
        {
            return Math.Sqrt(VarianceP(values));
        }

        /// <summary>
        /// Returns the population variance of values
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double VarianceP(this IEnumerable<double> values)
        {
            double avg = values.Average();
            return values.Average(v => Math.Pow(v - avg, 2));
        }
    }
}
