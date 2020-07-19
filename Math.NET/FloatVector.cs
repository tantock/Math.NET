using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
    public sealed class FloatVector : Vector<float>
    {
        public FloatVector(float[] values) : base(values) { }
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toCopy"></param>
        public FloatVector(FloatVector toCopy) : base(toCopy) { }
        public FloatVector(FloatMatrix toCast) : base(toCast) { }
        #region internal generic type arithmetic
        protected override float add(float a, float b)
        {
            return a + b;
        }

        protected override float GetZeroValue()
        {
            return 0f;
        }

        protected override float multiply(float a, float b)
        {
            return a * b;
        }

        protected override float subtract(float a, float b)
        {
            return a - b;
        }

        protected override float sqrt(float a)
        {
            return (float)Math.Sqrt(a);
        }
        #endregion

        #region Vector operators
        public static FloatVector operator +(FloatVector A, FloatVector B)
        {
            return new FloatVector(A.vectorAdd(B));
        }
        public static FloatVector operator -(FloatVector A, FloatVector B)
        {
            return new FloatVector(A.vectorSubtract(B));
        }
        public static FloatVector operator -(FloatVector A)
        {
            return new FloatVector(A.vectorScalarMultiply(-1));
        }
        public static float operator *(FloatVector A, FloatVector B)
        {
            return A.vectorMultiply(B);
        }
        /// <summary>
        /// Element wise multiplication
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static FloatVector operator &(FloatVector A, FloatVector B)
        {
            return new FloatVector(A.vectorElementWiseMultiply(B));
        }
        public static FloatVector operator *(FloatVector A, float k)
        {
            return new FloatVector(A.vectorScalarMultiply(k));
        }
        public static FloatVector operator *(float k, FloatVector A)
        {
            return new FloatVector(A.vectorScalarMultiply(k));
        }
        public static FloatVector operator /(FloatVector A, float k)
        {
            return new FloatVector(A.vectorScalarMultiply(1/k));
        }
        #endregion

        #region Matrix-Vector operations
        public static FloatVector operator *(FloatMatrix A, FloatVector v)
        {
            FloatMatrix vector = new FloatMatrix(v, true);
            return new FloatVector(A * vector);
        }
        public static FloatVector operator *(FloatVector v, FloatMatrix A)
        {
            FloatMatrix vector = new FloatMatrix(v, false);
            return new FloatVector(vector*A);
        }
        #endregion
    }
}
