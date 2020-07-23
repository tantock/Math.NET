using MathDotNET.MathOperators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
    /// <summary>
    /// M by N matrix of type U
    /// </summary>
    /// <typeparam name="U"></typeparam>
    public class Matrix<U>
    {
        private U[][] values;

        
        readonly int numRows;
        readonly int numColumns;

        private static IMathOperators<U> operators;

        public int M
        {
            get { return numRows; }
        }

        public int N
        {
            get { return numColumns; }
        }

        private void setOperators()
        {
            if(operators == null)
            {
                Type datatype = typeof(U);
                if (datatype == typeof(double))
                {
                    operators = (IMathOperators<U>)new DoubleOperators();
                }
                else if(datatype == typeof(float))
                {
                    operators = (IMathOperators<U>)new FloatOperators();
                }
                else
                {
                    throw new NotImplementedException("Datatype unknown for math operators");
                }
            }
        }

        /// <summary>
        /// Copies the reference of a 2d array
        /// </summary>
        /// <param name="values">Pass by reference</param>
        public Matrix(U[][] values)
        {
            this.values = values;//new U[values.Length][];
            numRows = values.Length;
            numColumns = values[0].Length;
            setOperators();
            //for(int i = 0; i < values.Length; i++)
            //{
            //    this.values[i] = new U[numColumns];
            //    Array.Copy(values[i], this.values[i], values[i].Length);
            //}
        }
        /// <summary>
        /// Deep copies a matrix object
        /// </summary>
        /// <param name="toCopy"></param>
        public Matrix(Matrix<U> toCopy)
        {
            this.values = new U[toCopy.numRows][];
            numRows = toCopy.numRows;
            numColumns = toCopy.numColumns;
            for (int i = 0; i < numRows; i++)
            {
                this.values[i] = new U[numColumns];
                Array.Copy(toCopy.values[i], this.values[i], toCopy.values[i].Length);
            }
            setOperators();
        }

        public Matrix(int numRows, int numColumns)
        {
            this.numRows = numRows;
            this.numColumns = numColumns;
            this.values = new U[numRows][];
            for (int i = 0; i < numRows; i++)
            {
                this.values[i] = new U[numColumns];
                for(int j = 0; j < numColumns; j++)
                {
                    this.values[i][j] = operators.GetZeroValue();
                }
            }
            setOperators();
        }
        public Matrix(Vector<U> toCast, bool isColumnVector) : this(isColumnVector ? toCast.Size : 1, isColumnVector? 1 : toCast.Size)
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
            setOperators();
        }

        public U Get(int i, int j)
        {
            return values[i][j];
        }

        public void Set(int i, int j, U value)
        {
            values[i][j] = value;
        }

        public Matrix<U> T
        {
            get
            {
                int m = this.N;
                int n = this.M;
                U[][] result = new U[m][];
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new U[n];
                }
                //tranpose
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = this.values[j][i];
                    }
                }

                return new Matrix<U>(result);
            }
        }

        #region Matrix operators
        public static Matrix<U> operator +(Matrix<U> L, Matrix<U> R)
        {
            int m = L.numRows;
            int n = L.numColumns;
            U[][] result = new U[m][];

            if (R.numRows != L.numRows || R.numColumns != L.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for matrix addition");
            }
            else
            {
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new U[n];
                }
                //add
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = Matrix<U>.operators.add(L.values[i][j], R.values[i][j]);
                    }
                }

                return new Matrix<U>(result);
            }
        }
        public static Matrix<U> operator -(Matrix<U> L, Matrix<U> R)
        {
            int m = L.numRows;
            int n = L.numColumns;
            U[][] result = new U[m][];

            if (R.numRows != L.numRows || R.numColumns != L.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for matrix subtraction");
            }
            else
            {
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new U[n];
                }
                //add
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = Matrix<U>.operators.subtract(L.values[i][j], R.values[i][j]);
                    }
                }

                return new Matrix<U>(result);
            }
        }
        public static Matrix<U> operator -(Matrix<U> R)
        {
            int m = R.numRows;
            int n = R.numColumns;
            U[][] result = new U[m][];
            //create new 2d array
            for (int i = 0; i < m; i++)
            {
                result[i] = new U[n];
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = operators.subtract(operators.GetZeroValue(), R.values[i][j]);
                }
            }
            return new Matrix<U>(result);
        }
        public static Matrix<U> operator *(Matrix<U> L, Matrix<U> R)
        {
            int m = L.M;
            int n = L.N;
            int p = R.N;
            U[][] result = new U[m][];

            if (R.numRows != L.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for matrix multiplication");
            }
            else
            {
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new U[p];
                }
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < p; j++)
                    {
                        result[i][j] = operators.GetZeroValue();
                        for (int k = 0; k < n; k++)
                        {
                            result[i][j] = operators.add(operators.multiply(L.values[i][k], R.values[k][j]), result[i][j]);
                        }
                    }
                }

                return new Matrix<U>(result);
            }
        }
        public static Matrix<U> operator &(Matrix<U> L, Matrix<U> R)
        {
            int m = L.numRows;
            int n = L.numColumns;
            U[][] result = new U[m][];

            if (R.numRows != L.numRows || R.numColumns != L.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for hadamard multiplication");
            }
            else
            {
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new U[n];
                }
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = operators.multiply(L.values[i][j], R.values[i][j]);
                    }
                }

                return new Matrix<U>(result);
            }
        }
        public static Matrix<U> operator *(U k, Matrix<U> R)
        {
            int m = R.numRows;
            int n = R.numColumns;
            U[][] result = new U[m][];
            //create new 2d array
            for (int i = 0; i < m; i++)
            {
                result[i] = new U[n];
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = operators.multiply(k, R.values[i][j]);
                }
            }
            return new Matrix<U>(result);
        }
        public static Matrix<U> operator *(Matrix<U> L, U k)
        {
            return k * L;
        }
        public static Matrix<U> operator /(Matrix<U> L, U k)
        {
            int m = L.numRows;
            int n = L.numColumns;
            U[][] result = new U[m][];
            //create new 2d array
            for (int i = 0; i < m; i++)
            {
                result[i] = new U[n];
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = operators.divide(L.values[i][j],k);
                }
            }
            return new Matrix<U>(result);
        }
        #endregion

        /// <summary>
        /// Returns the Frobenius norm of the matrix
        /// </summary>
        public U Norm
        {
            get
            {
                U norm = operators.GetZeroValue();
                for(int i = 0; i < M; i++)
                {
                    for(int j = 0; j < N; j++)
                    {
                        var value = Get(i, j);
                        norm = operators.add(norm, operators.multiply(value, value));
                    }
                }
                return operators.sqrt(norm);
            }
        }

        public ReadOnlyCollection<ReadOnlyCollection<U>> GetReadOnlyValuesCollection()
        {
            ReadOnlyCollection<U>[] ROArray = new ReadOnlyCollection<U>[M];

            for(int i = 0; i < M; i++)
            {
                ROArray[i] = Array.AsReadOnly(values[i]);
            }
            return Array.AsReadOnly(ROArray);
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
            var item = obj as Matrix<U>;
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
