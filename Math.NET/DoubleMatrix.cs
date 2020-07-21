using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
    public sealed class DoubleMatrix : Matrix<double>
    {
        public DoubleMatrix(double[][] values) : base(values) { }

        public DoubleMatrix(DoubleMatrix toCopy) : base(toCopy) { }

        public DoubleMatrix(DoubleVector toCast, bool isColumnVector) : base(toCast, isColumnVector) { }
        #region internal generic type arithmetic
        protected override double add(double a, double b)
        {
            return a + b;
        }

        protected override double subtract(double a, double b)
        {
            return a - b;
        }

        protected override double multiply(double a, double b)
        {
            return a * b;
        }

        protected override double sqrt(double a)
        {
            return System.Math.Sqrt(a);
        }

        protected override double GetZeroValue()
        {
            return 0f;
        }
        #endregion

        #region Matrix operators
        public static DoubleMatrix operator +(DoubleMatrix A, DoubleMatrix B)
        {
            return new DoubleMatrix(A.matrixAdd(B));
        }

        public static DoubleMatrix operator -(DoubleMatrix A, DoubleMatrix B)
        {
            return new DoubleMatrix(A.matrixSubtract(B));
        }

        public static DoubleMatrix operator -(DoubleMatrix A)
        {
            return new DoubleMatrix(A.matrixScalarMultiply(-1));
        }

        public static DoubleMatrix operator *(DoubleMatrix A, DoubleMatrix B)
        {
            return new DoubleMatrix(A.matrixMultiply(B));
        }
        /// <summary>
        /// Element wise multiplication
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static DoubleMatrix operator &(DoubleMatrix A, DoubleMatrix B)
        {
            return new DoubleMatrix(A.matrixElementwiseMultiply(B));
        }

        public static DoubleMatrix operator *(DoubleMatrix A, double k)
        {
            return new DoubleMatrix(A.matrixScalarMultiply(k));
        }

        public static DoubleMatrix operator *(double k, DoubleMatrix A)
        {
            return new DoubleMatrix(A.matrixScalarMultiply(k));
        }

        public static DoubleMatrix operator /(DoubleMatrix A, double k)
        {
            return new DoubleMatrix(A.matrixScalarMultiply(1 / k));
        }

        public DoubleMatrix T
        {
            get { return new DoubleMatrix(this.transpose()); }
        }
        #endregion

    }
}
