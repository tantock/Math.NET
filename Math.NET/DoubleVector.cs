using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
    public sealed class DoubleVector : Vector<double>
    {
        public DoubleVector(double[] values) : base(values) { }
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toCopy"></param>
        public DoubleVector(DoubleVector toCopy) : base(toCopy) { }
        public DoubleVector(DoubleMatrix toCast) : base(toCast) { }
        #region internal generic type arithmetic
        protected override double add(double a, double b)
        {
            return a + b;
        }

        protected override double GetZeroValue()
        {
            return 0f;
        }

        protected override double multiply(double a, double b)
        {
            return a * b;
        }

        protected override double subtract(double a, double b)
        {
            return a - b;
        }

        protected override double sqrt(double a)
        {
            return (double)Math.Sqrt(a);
        }
        #endregion

        #region Vector operators
        public static DoubleVector operator +(DoubleVector A, DoubleVector B)
        {
            return new DoubleVector(A.vectorAdd(B));
        }
        public static DoubleVector operator -(DoubleVector A, DoubleVector B)
        {
            return new DoubleVector(A.vectorSubtract(B));
        }
        public static DoubleVector operator -(DoubleVector A)
        {
            return new DoubleVector(A.vectorScalarMultiply(-1));
        }
        public static double operator *(DoubleVector A, DoubleVector B)
        {
            return A.vectorMultiply(B);
        }
        /// <summary>
        /// Element wise multiplication
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static DoubleVector operator &(DoubleVector A, DoubleVector B)
        {
            return new DoubleVector(A.vectorElementWiseMultiply(B));
        }
        public static DoubleVector operator *(DoubleVector A, double k)
        {
            return new DoubleVector(A.vectorScalarMultiply(k));
        }
        public static DoubleVector operator *(double k, DoubleVector A)
        {
            return new DoubleVector(A.vectorScalarMultiply(k));
        }
        public static DoubleVector operator /(DoubleVector A, double k)
        {
            return new DoubleVector(A.vectorScalarMultiply(1/k));
        }
        #endregion

        #region Matrix-Vector operations
        public static DoubleVector operator *(DoubleMatrix A, DoubleVector v)
        {
            DoubleMatrix vector = new DoubleMatrix(v, true);
            return new DoubleVector(A * vector);
        }
        public static DoubleVector operator *(DoubleVector v, DoubleMatrix A)
        {
            DoubleMatrix vector = new DoubleMatrix(v, false);
            return new DoubleVector(vector*A);
        }
        #endregion
    }
}
