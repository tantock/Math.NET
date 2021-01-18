using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
    [Serializable]
    /// <summary>
    /// N vector object of type double
    /// </summary>
    public class DoubleVector
    {
        private double[] values;

        readonly int numEntries;

        /// <summary>
        /// Creates a vector object of size N
        /// </summary>
        /// <param name="N">Number of elements in the vector</param>
        public DoubleVector(int N)
        {
            numEntries = N;
            values = new double[N];
        }
        /// <summary>
        /// Copies the reference of an array
        /// </summary>
        /// <param name="values">Pass by reference</param>
        public DoubleVector(double[] values)
        {
            this.values = values;//new double[values.Length];
            numEntries = values.Length;

            //Array.Copy(values, this.values, values.Length);
        }
        /// <summary>
        /// Deep copies a vector object
        /// </summary>
        /// <param name="toCopy"></param>
        public DoubleVector(DoubleVector toCopy)
        {
            this.values = new double[toCopy.values.Length];
            this.numEntries = toCopy.numEntries;
            Array.Copy(toCopy.values, this.values, toCopy.values.Length);
        }
        /// <summary>
        /// Converts a Matrix to a Vector object
        /// </summary>
        /// <param name="toCast"></param>
        /// <exception cref="InvalidCastException">Thrown when the matrix is not 1 dimensional</exception>
        public DoubleVector(DoubleMatrix toCast)
        {
            if (!(toCast.M == 1 || toCast.N == 1))
            {
                throw new InvalidCastException("Matrix not 1 dimensional");
            }
            else
            {
                int counter = 0;
                this.numEntries = toCast.N * toCast.M;
                this.values = new double[this.numEntries];
                for (int i = 0; i < toCast.M; i++)
                {
                    for (int j = 0; j < toCast.N; j++)
                    {
                        values[counter] = toCast.Get(i, j);
                        counter++;
                    }
                }
            }

        }
        /// <summary>
        /// Gets the i-th value of the vector
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double Get(int i)
        {
            return values[i];
        }
        /// <summary>
        /// Sets the i-th value of the vector
        /// </summary>
        /// <param name="i"></param>
        /// <param name="value">Sets as this value</param>
        public void Set(int i, double value)
        {
            values[i] = value;
        }
        /// <summary>
        /// Returns the number of elements contained withing the vector
        /// </summary>
        public int Size
        {
            get
            {
                return numEntries;
            }
        }

        #region Vector operators
        /// <summary>
        /// Vector addition
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="R">Right vector</param>
        /// <returns>new Vector(L+R)</returns>
        public static DoubleVector operator +(DoubleVector L, DoubleVector R)
        {
            double[] result = new double[L.numEntries];
            if (R.numEntries != L.numEntries)
            {
                throw new InvalidOperationException("Invalid dimensions for vector addition");
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = L.values[i] + R.values[i];
                }
                return new DoubleVector(result);
            }
        }
        /// <summary>
        /// Element-wise addition
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="k">Scalar value</param>
        /// <returns>new Vector(L+R)</returns>
        public static DoubleVector operator +(DoubleVector L, double k)
        {
            double[] result = new double[L.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = L.values[i] + k;
            }
            return new DoubleVector(result);
        }
        /// <summary>
        /// Vector subtraction
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="R">Right vector</param>
        /// <returns>new Vector(L-R)</returns>
        public static DoubleVector operator -(DoubleVector L, DoubleVector R)
        {
            double[] result = new double[L.numEntries];
            if (R.numEntries != L.numEntries)
            {
                throw new InvalidOperationException("Invalid dimensions for vector subtraction");
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = L.values[i] - R.values[i];
                }
                return new DoubleVector(result);
            }
        }
        /// <summary>
        /// Element-wise subtraction
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="k">Scalar value</param>
        /// <returns>new Vector(L+R)</returns>
        public static DoubleVector operator -(DoubleVector L, double k)
        {
            double[] result = new double[L.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = L.values[i] - k;
            }
            return new DoubleVector(result);
        }
        /// <summary>
        /// Vector negation
        /// </summary>
        /// <param name="R">Right vector</param>
        /// <returns>new Vector(-R)</returns>
        public static DoubleVector operator -(DoubleVector R)
        {
            double[] result = new double[R.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = -R.values[i];
            }
            return new DoubleVector(result);
        }
        /// <summary>
        /// Vector multiplication
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="R">Right matrix</param>
        /// <returns>scalar(L*R)</returns>
        public static double operator *(DoubleVector L, DoubleVector R)
        {
            double result;
            if (R.numEntries != L.numEntries)
            {
                throw new InvalidOperationException("Invalid dimensions for vector multiplication");
            }
            else
            {
                result = 0;
                for (int i = 0; i < R.numEntries; i++)
                {
                    result += L.values[i] * R.values[i];
                }
                return result;
            }
        }
        /// <summary>
        /// Vector element-wise multiplication
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="R">Right vector</param>
        /// <returns>new Vector(L&amp;R)</returns>
        public static DoubleVector operator &(DoubleVector L, DoubleVector R)
        {
            double[] result = new double[L.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = L.values[i] * R.values[i];
            }
            return new DoubleVector(result);
        }
        /// <summary>
        /// Vector scalar multiplication
        /// </summary>
        /// <param name="k">Scalar multiple</param>
        /// <param name="R">Right vector</param>
        /// <returns>new Vector(k*R)</returns>
        public static DoubleVector operator *(double k, DoubleVector R)
        {
            double[] result = new double[R.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = R.values[i] * k;
            }
            return new DoubleVector(result);
        }
        /// <summary>
        /// Vector scalar multiplication
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="k">Scalar multiple</param>
        /// <returns>new Vector(L*k)</returns>
        public static DoubleVector operator *(DoubleVector L, double k)
        {
            return k * L;
        }
        /// <summary>
        /// Vector scalar division
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="k">Scalar multiple</param>
        /// <returns>new Vector(L/k)</returns>
        public static DoubleVector operator /(DoubleVector L, double k)
        {
            double[] result = new double[L.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = L.values[i] / k;
            }
            return new DoubleVector(result);
        }
        /// <summary>
        /// Vector element-wise division
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="R">Right vector</param>
        /// <returns>new Vector(L&amp;R)</returns>
        public static DoubleVector operator %(DoubleVector L, DoubleVector R)
        {
            double[] result = new double[L.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = L.values[i] / R.values[i];
            }
            return new DoubleVector(result);
        }
        /// <summary>
        /// Vector outer product
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="R">Right vector</param>
        /// <returns>new Matrix(L^R)</returns>
        public static DoubleMatrix operator ^(DoubleVector L, DoubleVector R)
        {
            var result = new DoubleMatrix(L.Size, R.Size);
            for (int i = 0; i < L.Size; i++)
            {
                for (int j = 0; j < R.Size; j++)
                {
                    result.Set(i, j, L.Get(i) * R.Get(j));
                }
            }
            return result;
        }
        #endregion

        #region Matrix-Vector operations
        /// <summary>
        /// Matrix-vector multiplication
        /// </summary>
        /// <param name="A">Matrix</param>
        /// <param name="v">Vector</param>
        /// <returns>new Vector(A*v)</returns>
        public static DoubleVector operator *(DoubleMatrix A, DoubleVector v)
        {
            DoubleMatrix vector = new DoubleMatrix(v, true);
            return new DoubleVector(A * vector);
        }
        /// <summary>
        /// Matrix-vector multiplication
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="A">Matrix</param>
        /// <returns>new Vector(v*A)</returns>
        public static DoubleVector operator *(DoubleVector v, DoubleMatrix A)
        {
            DoubleMatrix vector = new DoubleMatrix(v, false);
            return new DoubleVector(vector * A);
        }
        #endregion

        /// <summary>
        /// Returns the element-wise power of the vector
        /// </summary>
        public DoubleVector Pow(double power)
        {
            double[] result = new double[numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Pow(values[i], power);
            }
            return new DoubleVector(result);
        }
        /// <summary>
        /// Returns the Euclidean length of the vector
        /// </summary>
        public double EuclidLength
        {
            get
            {
                double norm = 0;
                for (int i = 0; i < Size; i++)
                {
                    var value = Get(i);
                    norm += value * value;
                }
                return Math.Sqrt(norm);
            }
        }
        /// <summary>
        /// Vector values
        /// </summary>
        /// <returns>A read only collection</returns>
        public ReadOnlyCollection<double> GetReadOnlyValuesCollection()
        {
            return Array.AsReadOnly(values);
        }
        /// <summary>
        /// String representation of the vector object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Join(",", values);
        }
        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            var item = obj as DoubleVector;
            if (item == null || item.Size != this.Size)
            {
                return false;
            }
            else
            {
                bool isEqual = true;
                for (int i = 0; i < this.Size && isEqual; i++)
                {
                    isEqual = this.Get(i).Equals(item.Get(i));
                }
                return isEqual;
            }
        }
        /// <summary>
        /// Serves as the default hash function
        /// </summary>
        /// <returns>A hash code for the current object</returns>
        public override int GetHashCode()
        {
            return values.GetHashCode();
        }
    }
}
