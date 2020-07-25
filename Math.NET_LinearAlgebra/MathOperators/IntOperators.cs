using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.MathOperators
{
    class IntOperators : IMathOperators<int>
    {
        public int add(int a, int b)
        {
            return a + b;
        }

        public int divide(int a, int b)
        {
            return a / b;
        }

        public int GetOneValue()
        {
            return 1;
        }

        public int GetZeroValue()
        {
            return 0;
        }

        public int multiply(int a, int b)
        {
            return a * b;
        }

        public int sqrt(int a)
        {
            return (int)Math.Sqrt(a);
        }

        public int subtract(int a, int b)
        {
            return a - b;
        }
    }
}
