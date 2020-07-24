using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET
{
    /// <summary>
    /// An interface for math operator functions of type T
    /// </summary>
    /// <typeparam name="T">datatype</typeparam>
    public interface IMathOperators<T>
    {
        /// <summary>
        /// Addition operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>a+b</returns>
        T add(T a, T b);
        /// <summary>
        /// Subtraction operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>a-b</returns>
        T subtract(T a, T b);
        /// <summary>
        /// Multiplication operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>a*b</returns>
        T multiply(T a, T b);
        /// <summary>
        /// Division operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>a/b</returns>
        T divide(T a, T b);
        /// <summary>
        /// Square root operator
        /// </summary>
        /// <param name="a"></param>
        /// <returns>Sqrt(a)</returns>
        T sqrt(T a);
        /// <summary>
        /// Default zero operator
        /// </summary>
        /// <returns>The zero of the datatype T</returns>
        T GetZeroValue();

    }
}
