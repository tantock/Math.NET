using MathDotNET.MathOperators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
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
                else
                {
                    throw new NotImplementedException("Datatype unknown for math operators");
                }
            }
        }
        public Vector(int Size)
        {
            numEntries = Size;
            values = new T[Size];
            setOperators();
            for(int i = 0; i < numEntries; i++)
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
        /// Copy constructor
        /// </summary>
        /// <param name="toCopy"></param>
        public Vector(Vector<T> toCopy)
        {
            this.values = new T[toCopy.values.Length];
            this.numEntries = toCopy.numEntries;
            Array.Copy(toCopy.values, this.values, toCopy.values.Length);
            setOperators();
        }

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

        public T Get(int i)
        {
            return values[i];
        }

        public void Set(int i, T value)
        {
            values[i] = value;
        }

        public int Size
        {
            get
            {
                return numEntries;
            }
        }

        #region Vector operators
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
        public static Vector<T> operator -(Vector<T> R)
        {
            T[] result = new T[R.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = operators.subtract(operators.GetZeroValue(), R.values[i]);
            }
            return new Vector<T>(result);
        }
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
        public static Vector<T> operator &(Vector<T> L, Vector<T> R)
        {
            T[] result = new T[L.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = operators.multiply(L.values[i], R.values[i]);
            }
            return new Vector<T>(result);
        }
        public static Vector<T> operator *(T k, Vector<T> R)
        {
            T[] result = new T[R.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = operators.multiply(R.values[i], k);
            }
            return new Vector<T>(result);
        }
        public static Vector<T> operator *(Vector<T> L, T k)
        {
            return k * L;
        }
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
        public static Vector<T> operator * (Matrix<T>A, Vector<T> v)
        {
            Matrix<T> vector = new Matrix<T>(v, true);
            return new Vector<T>(A * vector);
        }
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

        public ReadOnlyCollection<T> GetReadOnlyValuesCollection()
        {
            return Array.AsReadOnly(values);
        }
        public override string ToString()
        {
            return String.Join(",", values);
        }
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

        public override int GetHashCode()
        {
            return values.GetHashCode();
        }
    }
}
