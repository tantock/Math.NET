using MathDotNET.MathOperators;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
    /// <summary>
    /// M by N matrix of type U
    /// </summary>
    /// <typeparam name="U">datatype</typeparam>
    public class Matrix<U>
    {
        private U[][] values;
        
        readonly int numRows;
        readonly int numColumns;

        private static IMathOperators<U> operators;
        /// <summary>
        /// Number of rows
        /// </summary>
        public int M
        {
            get { return numRows; }
        }
        /// <summary>
        /// Number of columns
        /// </summary>
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
                else if(datatype == typeof(int))
                {
                    operators = (IMathOperators<U>)new IntOperators();
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
            setOperators();
            this.values = values;//new U[values.Length][];
            numRows = values.Length;
            numColumns = values[0].Length;
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
            setOperators();
            this.values = new U[toCopy.numRows][];
            numRows = toCopy.numRows;
            numColumns = toCopy.numColumns;
            for (int i = 0; i < numRows; i++)
            {
                this.values[i] = new U[numColumns];
                Array.Copy(toCopy.values[i], this.values[i], toCopy.values[i].Length);
            }
        }
        /// <summary>
        /// Creates a matrix object of size M by N
        /// </summary>
        /// <param name="M">Number of rows</param>
        /// <param name="N">Number of columns</param>
        public Matrix(int M, int N)
        {
            setOperators();
            numRows = M;
            numColumns = N;
            values = new U[numRows][];
            for (int i = 0; i < numRows; i++)
            {
                this.values[i] = new U[numColumns];
                for(int j = 0; j < numColumns; j++)
                {
                    this.values[i][j] = operators.GetZeroValue();
                }
            }
        }
        /// <summary>
        /// Converts a Vector to a Matrix object
        /// </summary>
        /// <param name="toCast"></param>
        /// <param name="isColumnVector">Determines if the vector object should be represented as a column or row vector</param>
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
        /// <summary>
        /// Gets the i-th and j-th value of the matrix
        /// </summary>
        /// <param name="i">Number of rows down</param>
        /// <param name="j">Number of columns across</param>
        /// <returns></returns>
        public U Get(int i, int j)
        {
            return values[i][j];
        }
        /// <summary>
        /// Sets the i-th and j-th value of the matrix
        /// </summary>
        /// <param name="i">Number of rows down</param>
        /// <param name="j">Number of columns across</param>
        /// <param name="value"></param>
        public void Set(int i, int j, U value)
        {
            values[i][j] = value;
        }
        /// <summary>
        /// Transposes a matrix
        /// </summary>
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
        /// <summary>
        /// Matrix addition
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(L+R)</returns>
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
        /// <summary>
        /// Matrix subtraction
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(L-R)</returns>
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
        /// <summary>
        /// Matrix negation
        /// </summary>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(-R)</returns>
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
        /// <summary>
        /// Matrix multiplication
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(L*R)</returns>
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
        /// <summary>
        /// Matrix element-wise multiplication
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(L&amp;R)</returns>
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
        /// <summary>
        /// Matrix scalar multiplication
        /// </summary>
        /// <param name="k">Scalar multiple</param>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(k*R)</returns>
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
        /// <summary>
        /// Matrix scalar multiplication
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="k">Scalar multiple</param>
        /// <returns>new Matrix(L*k)</returns>
        public static Matrix<U> operator *(Matrix<U> L, U k)
        {
            return k * L;
        }
        /// <summary>
        /// Matrix scalar division
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="k">Scalar divisor</param>
        /// <returns>new Matrix(L/k)</returns>
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
        /// <summary>
        /// Matrix values
        /// </summary>
        /// <returns>A A read-only collection</returns>
        public ReadOnlyCollection<ReadOnlyCollection<U>> GetReadOnlyValuesCollection()
        {
            ReadOnlyCollection<U>[] ROArray = new ReadOnlyCollection<U>[M];

            for(int i = 0; i < M; i++)
            {
                ROArray[i] = Array.AsReadOnly(values[i]);
            }
            return Array.AsReadOnly(ROArray);
        }
        /// <summary>
        /// String representation of the matrix object
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false</returns>
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
