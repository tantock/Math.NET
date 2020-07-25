using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.MathOperators
{
    public class DoubleOperators : IMathOperators<double>
    {
        public double add(double a, double b)
        {
            return a + b;
        }

        public double divide(double a, double b)
        {
            return a / b;
        }

        public double GetOneValue()
        {
            return 1;
        }

        public double GetZeroValue()
        {
            return 0;
        }

        public double multiply(double a, double b)
        {
            return a * b;
        }

        public double sqrt(double a)
        {
            return Math.Sqrt(a);
        }

        public double subtract(double a, double b)
        {
            return a - b;
        }
    }
}
