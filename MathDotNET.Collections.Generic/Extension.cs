using System;
using System.Collections.Generic;
using System.Linq;

namespace MathDotNET.Collections.Generic
{
    public static class Extension
    {
        /// <summary>
        /// Returns the population standard deviation of values
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double StandardDeviationP(this IEnumerable<double> values)
        {
            double avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }
    }
}
