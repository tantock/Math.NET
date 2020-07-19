using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
    public sealed class FloatMatrix : Matrix<float>
    {
        public FloatMatrix(float[][] values) : base(values) { }

        public FloatMatrix(FloatMatrix toCopy) : base(toCopy) { }

        public FloatMatrix(FloatVector toCast, bool isColumnVector) : base(toCast, isColumnVector) { }
        #region internal generic type arithmetic
        protected override float add(float a, float b)
        {
            return a + b;
        }

        protected override float subtract(float a, float b)
        {
            return a - b;
        }

        protected override float multiply(float a, float b)
        {
            return a * b;
        }

        protected override float sqrt(float a)
        {
            return (float)System.Math.Sqrt((double)a);
        }

        protected override float GetZeroValue()
        {
            return 0f;
        }
        #endregion

        #region Matrix operators
        public static FloatMatrix operator +(FloatMatrix A, FloatMatrix B)
        {
            return new FloatMatrix(A.matrixAdd(B));
        }

        public static FloatMatrix operator -(FloatMatrix A, FloatMatrix B)
        {
            return new FloatMatrix(A.matrixSubtract(B));
        }

        public static FloatMatrix operator -(FloatMatrix A)
        {
            return new FloatMatrix(A.matrixScalarMultiply(-1));
        }

        public static FloatMatrix operator *(FloatMatrix A, FloatMatrix B)
        {
            return new FloatMatrix(A.matrixMultiply(B));
        }
        /// <summary>
        /// Element wise multiplication
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static FloatMatrix operator &(FloatMatrix A, FloatMatrix B)
        {
            return new FloatMatrix(A.matrixElementwiseMultiply(B));
        }

        public static FloatMatrix operator *(FloatMatrix A, float k)
        {
            return new FloatMatrix(A.matrixScalarMultiply(k));
        }

        public static FloatMatrix operator *(float k, FloatMatrix A)
        {
            return new FloatMatrix(A.matrixScalarMultiply(k));
        }

        public static FloatMatrix operator /(FloatMatrix A, float k)
        {
            return new FloatMatrix(A.matrixScalarMultiply(1 / k));
        }

        public FloatMatrix T
        {
            get { return new FloatMatrix(this.transpose()); }
        }
        #endregion

    }
}
