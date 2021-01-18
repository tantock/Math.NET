using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MathDotNET.LinearAlgebra
{
    [Serializable]
    public class DoubleMatrix
    {
        private double[][] values;

        readonly int numRows;
        readonly int numColumns;

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

        /// <summary>
        /// Copies the reference of a 2d array
        /// </summary>
        /// <param name="values">Pass by reference</param>
        public DoubleMatrix(double[][] values)
        {
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
        public DoubleMatrix(DoubleMatrix toCopy)
        {
            this.values = new double[toCopy.numRows][];
            numRows = toCopy.numRows;
            numColumns = toCopy.numColumns;
            for (int i = 0; i < numRows; i++)
            {
                this.values[i] = new double[numColumns];
                Array.Copy(toCopy.values[i], this.values[i], toCopy.values[i].Length);
            }
        }
        /// <summary>
        /// Creates a matrix object of size M by N
        /// </summary>
        /// <param name="M">Number of rows</param>
        /// <param name="N">Number of columns</param>
        public DoubleMatrix(int M, int N)
        {
            numRows = M;
            numColumns = N;
            values = new double[numRows][];
            for (int i = 0; i < numRows; i++)
            {
                this.values[i] = new double[numColumns];
                for (int j = 0; j < numColumns; j++)
                {
                    this.values[i][j] = 0;
                }
            }
        }
        /// <summary>
        /// Converts a Vector to a Matrix object
        /// </summary>
        /// <param name="toCast"></param>
        /// <param name="isColumnVector">Determines if the vector object should be represented as a column or row vector</param>
        public DoubleMatrix(DoubleVector toCast, bool isColumnVector) : this(isColumnVector ? toCast.Size : 1, isColumnVector ? 1 : toCast.Size)
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
        /// <summary>
        /// Gets the i-th and j-th value of the matrix
        /// </summary>
        /// <param name="i">Number of rows down</param>
        /// <param name="j">Number of columns across</param>
        /// <returns></returns>
        public double Get(int i, int j)
        {
            return values[i][j];
        }
        /// <summary>
        /// Sets the i-th and j-th value of the matrix
        /// </summary>
        /// <param name="i">Number of rows down</param>
        /// <param name="j">Number of columns across</param>
        /// <param name="value"></param>
        public void Set(int i, int j, double value)
        {
            values[i][j] = value;
        }
        /// <summary>
        /// Transposes a matrix
        /// </summary>
        public DoubleMatrix T
        {
            get
            {
                int m = this.N;
                int n = this.M;
                double[][] result = new double[m][];
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new double[n];
                }
                //tranpose
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = this.values[j][i];
                    }
                }

                return new DoubleMatrix(result);
            }
        }

        #region Matrix operators
        /// <summary>
        /// Matrix addition
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(L+R)</returns>
        public static DoubleMatrix operator +(DoubleMatrix L, DoubleMatrix R)
        {
            int m = L.numRows;
            int n = L.numColumns;
            double[][] result = new double[m][];

            if (R.numRows != L.numRows || R.numColumns != L.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for matrix addition");
            }
            else
            {
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new double[n];
                }
                //add
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = L.values[i][j] + R.values[i][j];
                    }
                }

                return new DoubleMatrix(result);
            }
        }
        /// <summary>
        /// Element-wise addition
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="k">scalar</param>
        /// <returns>new Matrix(L+R)</returns>
        public static DoubleMatrix operator +(DoubleMatrix L, double k)
        {
            int m = L.numRows;
            int n = L.numColumns;
            double[][] result = new double[m][];
            //create new 2d array
            for (int i = 0; i < m; i++)
            {
                result[i] = new double[n];
            }
            //add
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = L.values[i][j] + k;
                }
            }

            return new DoubleMatrix(result);
        }
        /// <summary>
        /// Matrix subtraction
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(L-R)</returns>
        public static DoubleMatrix operator -(DoubleMatrix L, DoubleMatrix R)
        {
            int m = L.numRows;
            int n = L.numColumns;
            double[][] result = new double[m][];

            if (R.numRows != L.numRows || R.numColumns != L.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for matrix subtraction");
            }
            else
            {
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new double[n];
                }
                //add
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = L.values[i][j] - R.values[i][j];
                    }
                }

                return new DoubleMatrix(result);
            }
        }
        /// <summary>
        /// Element-wise subtraction
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="k">scalar</param>
        /// <returns>new Matrix(L+R)</returns>
        public static DoubleMatrix operator -(DoubleMatrix L, double k)
        {
            int m = L.numRows;
            int n = L.numColumns;
            double[][] result = new double[m][];
            //create new 2d array
            for (int i = 0; i < m; i++)
            {
                result[i] = new double[n];
            }
            //add
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = L.values[i][j] - k;
                }
            }

            return new DoubleMatrix(result);
        }
        /// <summary>
        /// Matrix negation
        /// </summary>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(-R)</returns>
        public static DoubleMatrix operator -(DoubleMatrix R)
        {
            int m = R.numRows;
            int n = R.numColumns;
            double[][] result = new double[m][];
            //create new 2d array
            for (int i = 0; i < m; i++)
            {
                result[i] = new double[n];
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = -R.values[i][j];
                }
            }
            return new DoubleMatrix(result);
        }
        /// <summary>
        /// Matrix multiplication
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(L*R)</returns>
        public static DoubleMatrix operator *(DoubleMatrix L, DoubleMatrix R)
        {
            int m = L.M;
            int n = L.N;
            int p = R.N;
            double[][] result = new double[m][];

            if (R.numRows != L.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for matrix multiplication");
            }
            else
            {
                var degreeOfParallelism = Environment.ProcessorCount;

                var tasks = new Task[degreeOfParallelism];

                for (int taskNumber = 0; taskNumber < degreeOfParallelism; taskNumber++)
                {
                    // capturing taskNumber in lambda wouldn't work correctly
                    int taskNumberCopy = taskNumber;

                    tasks[taskNumber] = Task.Factory.StartNew(
                        () =>
                        {
                            var max = m * (taskNumberCopy + 1) / degreeOfParallelism;
                            for (int i = m * taskNumberCopy / degreeOfParallelism;
                                i < max;
                                i++)
                            {
                                result[i] = new double[p];
                                for (int j = 0; j < p; j++)
                                {
                                    for (int k = 0; k < n; k++)
                                    {
                                        result[i][j] += L.values[i][k] * R.values[k][j];
                                    }
                                }
                            }
                        });
                }

                Task.WaitAll(tasks);

                return new DoubleMatrix(result);
            }
        }
        /// <summary>
        /// Matrix type U cast to U
        /// </summary>
        /// <param name="R">Right matrix</param>
        public static implicit operator double(DoubleMatrix R)
        {
            if (R.M != 1 || R.N != 1)
            {
                throw new InvalidOperationException("Invalid dimensions for casting to type " + typeof(double).ToString());
            }
            else
            {
                return R.Get(0, 0);
            }
        }
        /// <summary>
        /// Matrix element-wise multiplication
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(L&amp;R)</returns>
        public static DoubleMatrix operator &(DoubleMatrix L, DoubleMatrix R)
        {
            int m = L.numRows;
            int n = L.numColumns;
            double[][] result = new double[m][];

            if (R.numRows != L.numRows || R.numColumns != L.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for hadamard multiplication");
            }
            else
            {
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new double[n];
                }
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = L.values[i][j] * R.values[i][j];
                    }
                }

                return new DoubleMatrix(result);
            }
        }
        /// <summary>
        /// Matrix element-wise division
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(L&amp;R)</returns>
        public static DoubleMatrix operator %(DoubleMatrix L, DoubleMatrix R)
        {
            int m = L.numRows;
            int n = L.numColumns;
            double[][] result = new double[m][];

            if (R.numRows != L.numRows || R.numColumns != L.numColumns)
            {
                throw new InvalidOperationException("Invalid dimensions for hadamard multiplication");
            }
            else
            {
                //create new 2d array
                for (int i = 0; i < m; i++)
                {
                    result[i] = new double[n];
                }
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = L.values[i][j] / R.values[i][j];
                    }
                }

                return new DoubleMatrix(result);
            }
        }
        /// <summary>
        /// Matrix scalar multiplication
        /// </summary>
        /// <param name="k">Scalar multiple</param>
        /// <param name="R">Right matrix</param>
        /// <returns>new Matrix(k*R)</returns>
        public static DoubleMatrix operator *(double k, DoubleMatrix R)
        {
            int m = R.numRows;
            int n = R.numColumns;
            double[][] result = new double[m][];
            //create new 2d array
            for (int i = 0; i < m; i++)
            {
                result[i] = new double[n];
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = k* R.values[i][j];
                }
            }
            return new DoubleMatrix(result);
        }
        /// <summary>
        /// Matrix scalar multiplication
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="k">Scalar multiple</param>
        /// <returns>new Matrix(L*k)</returns>
        public static DoubleMatrix operator *(DoubleMatrix L, double k)
        {
            return k * L;
        }
        /// <summary>
        /// Matrix scalar division
        /// </summary>
        /// <param name="L">Left matrix</param>
        /// <param name="k">Scalar divisor</param>
        /// <returns>new Matrix(L/k)</returns>
        public static DoubleMatrix operator /(DoubleMatrix L, double k)
        {
            int m = L.numRows;
            int n = L.numColumns;
            double[][] result = new double[m][];
            //create new 2d array
            for (int i = 0; i < m; i++)
            {
                result[i] = new double[n];
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = L.values[i][j]/ k;
                }
            }
            return new DoubleMatrix(result);
        }
        #endregion
        /// <summary>
        /// Returns the element-wise power of this matrix
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public DoubleMatrix Pow(double power)
        {
            double[][] result = new double[M][];
            //create new 2d array
            for (int i = 0; i < M; i++)
            {
                result[i] = new double[N];
            }
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    result[i][j] = Math.Pow(values[i][j], power);
                }
            }

            return new DoubleMatrix(result);
        }
        /// <summary>
        /// Returns the Frobenius norm of the matrix
        /// </summary>
        public double Norm
        {
            get
            {
                double norm = 0;
                for (int i = 0; i < M; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        var value = Get(i, j);
                        norm += (value * value);
                    }
                }
                return Math.Sqrt(norm);
            }
        }
        /// <summary>
        /// Matrix values
        /// </summary>
        /// <returns>A read-only collection</returns>
        public ReadOnlyCollection<ReadOnlyCollection<double>> GetReadOnlyValuesCollection()
        {
            ReadOnlyCollection<double>[] ROArray = new ReadOnlyCollection<double>[M];

            for (int i = 0; i < M; i++)
            {
                ROArray[i] = Array.AsReadOnly(values[i]);
            }
            return Array.AsReadOnly(ROArray);
        }
        public double[] To1D()
        {
            double[] flatten = new double[numRows * numColumns];
            int counter = 0;
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    flatten[counter] = values[i][j];
                    counter++;
                }
            }
            return flatten;
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
            for (int i = 1; i < M; i++)
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
            var item = obj as DoubleMatrix;
            if (item == null || item.M != this.M || item.N != this.N)
            {
                return false;
            }
            else
            {
                bool isEqual = true;
                for (int i = 0; i < this.M && isEqual; i++)
                {
                    for (int j = 0; j < this.N && isEqual; j++)
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

        public static double[][] Identity(int size)
        {
            double[][] result = new double[size][];
            for(int i = 0; i < size; i++)
            {
                result[i] = new double[size];
                result[i][i] = 1;
            }
            return result;
        }
    }
}
