using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.MathOperators
{
    class FloatOperators : IMathOperators<float>
    {
        public float add(float a, float b)
        {
            return a + b;
        }

        public float divide(float a, float b)
        {
            return a / b;
        }

        public float GetOneValue()
        {
            return 1f;
        }

        public float GetZeroValue()
        {
            return 0f;
        }

        public float multiply(float a, float b)
        {
            return a * b;
        }

        public float sqrt(float a)
        {
            return (float)Math.Sqrt(a);
        }

        public float subtract(float a, float b)
        {
            return a - b;
        }
    }
}
