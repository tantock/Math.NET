using System;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
    /// <summary>
    /// M by N matrix of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Matrix<T>
    {
        private T[][] values;

        
        readonly int numRows;
        readonly int numColumns;

        protected abstract T add(T a, T b);
        protected abstract T subtract(T a, T b);
        protected abstract T multiply(T a, T b);

        protected abstract T sqrt(T a);
        protected abstract T GetZeroValue();
        public int M
        {
            get { return numRows; }
        }

        public int N
        {
            get { return numColumns; }
        }
        public Matrix(T[][] values)
        {
            this.values = new T[values.Length][];
            numRows = values.Length;
            numColumns = values[0].Length;
            for(int i = 0; i < values.Length; i++)
            {
                this.values[i] = new T[numColumns];
                Array.Copy(values[i], this.values[i], values[i].Length);
            }
        }

        public Matrix(Matrix<T> toCopy)
        {
            this.values = new T[toCopy.values.Length][];
            numRows = toCopy.numRows;
            numColumns = toCopy.numColumns;
            for (int i = 0; i < values.Length; i++)
            {
                this.values[i] = new T[numColumns];
                Array.Copy(toCopy.values[i], this.values[i], toCopy.values[i].Length);
            }
        }

        public Matrix(int numRows, int numColumns)
        {
            this.numRows = numRows;
            this.numColumns = numColumns;
            this.values = new T[numRows][];
            for (int i = 0; i < numRows; i++)
            {
                this.values[i] = new T[numColumns];
            }
        }
        public Matrix(Vector<T> toCast, bool isColumnVector) : this(isColumnVector ? toCast.Size : 1, isColumnVector? 1 : toCast.Size)
        {
            if (isColumnVector)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    this.values[i][0] = toCast.Get(i);
                }
            }
            else
            {
                for (int i = 0; i < numColumns; i++)
                {
                    this.values[0][i] = toCast.Get(i);
                }
            }
        }

        public T Get(int i, int j)
        {
            return values[i][j];
        }

        public void Set(int i, int j, T value)
        {
            values[i][j] = value;
        }
        internal T[][] matrixMultiply(Matrix<T> B)
        {
            int m = this.M;
            int n = this.N;
            int p = B.N;
            T[][] result = new T[m][];

            if(B.numRows != this.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for matrix multiplication");
            }
            else
            {
                //create new 2d array
                for(int i = 0; i < m; i++)
                {
                    result[i] = new T[p];
                }
                //multiply
                for (int i = 0; i < m; i++)
                {
                    for(int j = 0; j < p; j++)
                    {
                        result[i][j] = GetZeroValue();
                        for(int k = 0; k < n; k++)
                        {
                            result[i][j] = add(multiply(this.values[i][k],B.values[k][j]), result[i][j]);
                        }
                    }
                }

                return result;
            }
        }

        internal T[][] matrixElementwiseMultiply(Matrix<T> B)
        {
            int m = this.numRows;
            int n = this.numColumns;
            T[][] result = new T[m][];

            if (B.numRows != this.numRows || B.numColumns != this.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for hadamard multiplication");
            }
            else
            {
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new T[n];
                }
                //add
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = multiply(this.values[i][j], B.values[i][j]);
                    }
                }

                return result;
            }
        }

        internal T[][] matrixAdd(Matrix<T> B)
        {
            int m = this.numRows;
            int n = this.numColumns;
            T[][] result = new T[m][];

            if (B.numRows != this.numRows || B.numColumns != this.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for matrix addition");
            }
            else
            {
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new T[n];
                }
                //add
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = add(this.values[i][j], B.values[i][j]);
                    }
                }

                return result;
            }
        }

        internal T[][] matrixSubtract(Matrix<T> B)
        {
            int m = this.numRows;
            int n = this.numColumns;
            T[][] result = new T[m][];

            if (B.numRows != this.numRows || B.numColumns != this.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for matrix subtraction");
            }
            else
            {
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new T[n];
                }
                //add
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = subtract(this.values[i][j], B.values[i][j]);
                    }
                }

                return result;
            }
        }

        internal T[][] matrixScalarMultiply(T k)
        {
            int m = this.numRows;
            int n = this.numColumns;
            T[][] result = new T[m][];
            //create new 2d array
            for (int i = 0; i < m; i++)
            {
                result[i] = new T[n];
            }
            //add
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = multiply(this.values[i][j], k);
                }
            }

            return result;
        }

        internal T[][] transpose()
        {
            int m = this.N;
            int n = this.M;
            T[][] result = new T[m][];
            //create new 2d array
            for (int i = 0; i < m; i++)
            {
                result[i] = new T[n];
            }
            //tranpose
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = this.values[j][i];
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the Frobenius norm of the matrix
        /// </summary>
        public T Norm
        {
            get
            {
                T norm = GetZeroValue();
                for(int i = 0; i < M; i++)
                {
                    for(int j = 0; j < N; j++)
                    {
                        var value = Get(i, j);
                        norm = add(norm, multiply(value, value));
                    }
                }
                return sqrt(norm);
            }
        }
        public override string ToString()
        {
            //string val = string.Join(",", values[0]);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(",", values[0]));
            for(int i = 1; i < M; i++)
            {
                sb.AppendLine(string.Join(",", values[i]));
            }
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            var item = obj as Matrix<T>;
            if(item == null || item.M != this.M || item.N != this.N)
            {
                return false;
            }
            else
            {
                bool isEqual = true;
                for(int i = 0; i < this.M && isEqual; i++)
                {
                    for(int j = 0; j < this.N && isEqual; j++)
                    {
                        isEqual = this.Get(i, j).Equals(item.Get(i, j));
                    }
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
