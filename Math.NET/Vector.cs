using MathDotNET.MathOperators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
    /// <summary>
    /// N vector object of type T
    /// </summary>
    /// <typeparam name="T">datatype</typeparam>
    public class Vector<T>
    {
        private T[] values;

        readonly int numEntries;

        private static IMathOperators<T> operators;

        private void setOperators()
        {
            if (operators == null)
            {
                Type datatype = typeof(T);
                if (datatype == typeof(double))
                {
                    operators = (IMathOperators<T>)new DoubleOperators();
                }
                else if (datatype == typeof(float))
                {
                    operators = (IMathOperators<T>)new FloatOperators();
                }
                else if (datatype == typeof(int))
                {
                    operators = (IMathOperators<T>)new IntOperators();
                }
                else
                {
                    throw new NotImplementedException("Datatype unknown for math operators");
                }
            }
        }
        /// <summary>
        /// Creates a vector object of size N
        /// </summary>
        /// <param name="N">Number of elements in the vector</param>
        public Vector(int N)
        {
            numEntries = N;
            values = new T[N];
            setOperators();
            for(int i = 0; i < N; i++)
            {
                values[i] = operators.GetZeroValue();
            }
        }
        /// <summary>
        /// Copies the reference of an array
        /// </summary>
        /// <param name="values">Pass by reference</param>
        public Vector(T[] values)
        {
            this.values = values;//new T[values.Length];
            numEntries = values.Length;
            setOperators();
            //Array.Copy(values, this.values, values.Length);
        }
        /// <summary>
        /// Deep copies a vector object
        /// </summary>
        /// <param name="toCopy"></param>
        public Vector(Vector<T> toCopy)
        {
            this.values = new T[toCopy.values.Length];
            this.numEntries = toCopy.numEntries;
            Array.Copy(toCopy.values, this.values, toCopy.values.Length);
            setOperators();
        }
        /// <summary>
        /// Converts a Matrix to a Vector object
        /// </summary>
        /// <param name="toCast"></param>
        /// <exception cref="InvalidCastException">Thrown when the matrix is not 1 dimensional</exception>
        public Vector(Matrix<T> toCast)
        {
            if(!(toCast.M == 1 || toCast.N == 1))
            {
                throw new InvalidCastException("Matrix not 1 dimensional");
            }
            else
            {
                int counter = 0;
                this.numEntries = toCast.N * toCast.M;
                this.values = new T[this.numEntries];
                for (int i = 0; i < toCast.M; i++)
                {
                    for(int j = 0; j < toCast.N; j++)
                    {
                        values[counter] = toCast.Get(i, j);
                        counter++;
                    }
                }
            }
            setOperators();
        }
        /// <summary>
        /// Gets the i-th value of the vector
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T Get(int i)
        {
            return values[i];
        }
        /// <summary>
        /// Sets the i-th value of the vector
        /// </summary>
        /// <param name="i"></param>
        /// <param name="value">Sets as this value</param>
        public void Set(int i, T value)
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
        public static Vector<T> operator +(Vector<T> L, Vector<T> R)
        {
            T[] result = new T[L.numEntries];
            if (R.numEntries != L.numEntries)
            {
                throw new InvalidOperationException("Invalid dimensions for vector addition");
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = operators.add(L.values[i], R.values[i]);
                }
                return new Vector<T>(result);
            }
        }
        /// <summary>
        /// Vector subtraction
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="R">Right vector</param>
        /// <returns>new Vector(L-R)</returns>
        public static Vector<T> operator -(Vector<T> L, Vector<T> R)
        {
            T[] result = new T[L.numEntries];
            if (R.numEntries != L.numEntries)
            {
                throw new InvalidOperationException("Invalid dimensions for vector subtraction");
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = operators.subtract(L.values[i], R.values[i]);
                }
                return new Vector<T>(result);
            }
        }
        /// <summary>
        /// Vector negation
        /// </summary>
        /// <param name="R">Right vector</param>
        /// <returns>new Vector(-R)</returns>
        public static Vector<T> operator -(Vector<T> R)
        {
            T[] result = new T[R.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = operators.subtract(operators.GetZeroValue(), R.values[i]);
            }
            return new Vector<T>(result);
        }
        /// <summary>
        /// Vector multiplication
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="R">Right matrix</param>
        /// <returns>scalar(L*R)</returns>
        public static T operator *(Vector<T> L, Vector<T> R)
        {
            T result;
            if (R.numEntries != L.numEntries)
            {
                throw new InvalidOperationException("Invalid dimensions for vector multiplication");
            }
            else
            {
                result = operators.GetZeroValue();
                for (int i = 0; i < R.numEntries; i++)
                {
                    result = operators.add(operators.multiply(L.values[i], R.values[i]), result);
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
        public static Vector<T> operator &(Vector<T> L, Vector<T> R)
        {
            T[] result = new T[L.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = operators.multiply(L.values[i], R.values[i]);
            }
            return new Vector<T>(result);
        }
        /// <summary>
        /// Vector scalar multiplication
        /// </summary>
        /// <param name="k">Scalar multiple</param>
        /// <param name="R">Right vector</param>
        /// <returns>new Vector(k*R)</returns>
        public static Vector<T> operator *(T k, Vector<T> R)
        {
            T[] result = new T[R.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = operators.multiply(R.values[i], k);
            }
            return new Vector<T>(result);
        }
        /// <summary>
        /// Vector scalar multiplication
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="k">Scalar multiple</param>
        /// <returns>new Vector(L*k)</returns>
        public static Vector<T> operator *(Vector<T> L, T k)
        {
            return k * L;
        }
        /// <summary>
        /// Vector scalar division
        /// </summary>
        /// <param name="L">Left vector</param>
        /// <param name="k">Scalar multiple</param>
        /// <returns>new Vector(L/k)</returns>
        public static Vector<T> operator /(Vector<T> L, T k)
        {
            T[] result = new T[L.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = operators.divide(L.values[i], k);
            }
            return new Vector<T>(result);
        }
        #endregion

        #region Matrix-Vector operations
        /// <summary>
        /// Matrix-vector multiplication
        /// </summary>
        /// <param name="A">Matrix</param>
        /// <param name="v">Vector</param>
        /// <returns>new Vector(A*v)</returns>
        public static Vector<T> operator * (Matrix<T>A, Vector<T> v)
        {
            Matrix<T> vector = new Matrix<T>(v, true);
            return new Vector<T>(A * vector);
        }
        /// <summary>
        /// Matrix-vector multiplication
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="A">Matrix</param>
        /// <returns>new Vector(v*A)</returns>
        public static Vector<T> operator *(Vector<T> v, Matrix<T> A)
        {
            Matrix<T> vector = new Matrix<T>(v, false);
            return new Vector<T>(vector * A);
        }
        #endregion

        /// <summary>
        /// Returns the Euclidean length of the vector
        /// </summary>
        public T EuclidLength
        {
            get
            {
                T norm = operators.GetZeroValue();
                for (int i = 0; i < Size; i++)
                {
                    var value = Get(i);
                    norm = operators.add(norm, operators.multiply(value, value));
                }
                return operators.sqrt(norm);
            }
        }
        /// <summary>
        /// Vector values
        /// </summary>
        /// <returns>A read only collection</returns>
        public ReadOnlyCollection<T> GetReadOnlyValuesCollection()
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
            var item = obj as Vector<T>;
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
