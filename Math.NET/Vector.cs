using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
    public abstract class Vector<T>
    {
        private T[] values;


        readonly int numEntries;

        protected abstract T add(T a, T b);
        protected abstract T subtract(T a, T b);
        protected abstract T multiply(T a, T b);
        protected abstract T sqrt(T a);

        protected abstract T GetZeroValue();
        /// <summary>
        /// Copies the reference of an array
        /// </summary>
        /// <param name="values">Pass by reference</param>
        public Vector(T[] values)
        {
            this.values = values;//new T[values.Length];
            numEntries = values.Length;
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
        internal T[] vectorAdd(Vector<T> B)
        {
            T[] result = new T[this.numEntries];
            if (B.numEntries != this.numEntries)
            {
                throw new InvalidOperationException("Invalid dimensions for vector addition");
            }
            else
            {
                for(int i = 0; i < result.Length; i++)
                {
                    result[i] = add(this.values[i], B.values[i]);
                }
                return result;
            }
        }

        internal T[] vectorSubtract(Vector<T> B)
        {
            T[] result = new T[this.numEntries];
            if (B.numEntries != this.numEntries)
            {
                throw new InvalidOperationException("Invalid dimensions for vector subtraction");
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = subtract(this.values[i], B.values[i]);
                }
                return result;
            }
        }

        internal T vectorMultiply(Vector<T> B)
        {
            T result;
            if (B.numEntries != this.numEntries)
            {
                throw new InvalidOperationException("Invalid dimensions for vector multiplication");
            }
            else
            {
                result = GetZeroValue();
                for (int i = 0; i < B.numEntries; i++)
                {
                    result = add(multiply(this.values[i],B.values[i]), result);
                }
                return result;
            }
        }
        internal T[] vectorScalarMultiply(T k)
        {
            T[] result = new T[this.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = multiply(this.values[i], k);
            }
            return result;
        }

        internal T[] vectorElementWiseMultiply(Vector<T> B)
        {
            T[] result = new T[this.numEntries];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = multiply(this.values[i], B.values[i]);
            }
            return result;
        }
        #endregion
        /// <summary>
        /// Returns the Euclidean length of the vector
        /// </summary>
        public T Length
        {
            get
            {
                T norm = GetZeroValue();
                for (int i = 0; i < Size; i++)
                {
                    var value = Get(i);
                    norm = add(norm, multiply(value, value));
                }
                return sqrt(norm);
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
