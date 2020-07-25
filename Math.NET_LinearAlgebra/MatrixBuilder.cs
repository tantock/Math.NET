using MathDotNET.MathOperators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.LinearAlgebra
{
    /// <summary>
    /// Common matrix builder
    /// </summary>
    /// <typeparam name="T">datatype</typeparam>
    public static class MatrixBuilder<T>
    {
        /// <summary>
        /// Returns an identity matrix of dimension N by N
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public static Matrix<T> Identity(int N)
        {
            Matrix<T> calc = new Matrix<T>(N,N);

            T one;

            Type datatype = typeof(T);

            if (datatype == typeof(double))
            {
                one = ((IMathOperators<T>)new DoubleOperators()).GetOneValue();
            }
            else if (datatype == typeof(float))
            {
                one = ((IMathOperators<T>)new FloatOperators()).GetOneValue();
            }
            else if (datatype == typeof(int))
            {
                one = ((IMathOperators<T>)new IntOperators()).GetOneValue();
            }
            else
            {
                throw new NotImplementedException("Datatype unknown for math operators");
            }

            for (int i = 0; i < calc.M; i++)
            {
                for(int j = 0; j < calc.N; j++)
                {
                    if(i == j)
                    {
                        calc.Set(i, j, one);
                    }
                }
            }
            return calc;
        }
        /// <summary>
        /// Creates an M by N matrix with all elements as value
        /// </summary>
        /// <param name="value">Set as this value</param>
        /// <param name="M">Number of rows</param>
        /// <param name="N">Number of columns</param>
        /// <returns></returns>
        public static Matrix<T> All(T value, int M, int N)
        {
            Matrix<T> calc = new Matrix<T>(M, N);
            for (int i = 0; i < calc.M; i++)
            {
                for (int j = 0; j < calc.N; j++)
                {
                    calc.Set(i, j, value);
                }
            }
            return calc;
        }
    }
}
