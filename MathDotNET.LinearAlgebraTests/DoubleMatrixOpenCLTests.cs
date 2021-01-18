using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathDotNET.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.LinearAlgebra.Tests
{
    [TestClass()]
    public class DoubleMatrixOpenCLTests
    {
        [TestMethod()]
        public void MultiplyTest()
        {
            var A = new Matrix<double>(new double[][] { new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 } });
            var C = new Matrix<double>(new double[][] { new double[] { 10, 20, 30 }, new double[] { 40, 50, 60 }, new double[] { 70, 80, 90 } });
            var ATimesC = new Matrix<double>(new double[][] { new double[] { 300, 360, 420 }, new double[] { 660, 810, 960 } });
            using (var clMultiply = new DoubleMatrixOpenCL(0))
            {
                Assert.AreEqual(ATimesC, clMultiply.Multiply(A, C));
            }
            
        }
    }
}