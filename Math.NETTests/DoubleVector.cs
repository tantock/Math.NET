using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathDotNET.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.LinearAlgebra.Tests
{
    [TestClass()]
    public class DoubleVectorTests
    {
        [TestMethod()]
        public void DoubleVectorTestConstructor()
        {
            double[] a_data = new double[] { 1, 2, 3 };
            var A = new DoubleVector(a_data);
            Assert.AreEqual(A.Size, 3);
            for (int i = 0; i < a_data.Length; i++)
            {
                Assert.AreEqual(a_data[i], A.Get(i));
            }
        }
        [TestMethod()]
        public void DoubleVectorTestCopyConstructor()
        {
            double[] a_data = new double[] { 1, 2, 3 };
            var A = new DoubleVector(a_data);
            var B = new DoubleVector(A);
            Assert.AreEqual(A, B);
            A.Set(0, -A.Get(0)); //check that mutation doesn't leak into copied class
            Assert.AreNotEqual(A, B);
        }
        [TestMethod()]
        public void DoubleVectorTestMathOperators()
        {
            var A = new DoubleVector(new double[] { 1, 2, 3 });
            var minusA = new DoubleVector(new double[] { -1, -2, -3 });
            var ATimes2 = new DoubleVector(new double[] { 2, 4, 6 });
            var A0 = new DoubleVector(new double[] { 0, 0, 0 });
            var B = new DoubleVector(new double[] { 1, 2, 3 });
            var C = new DoubleVector(new double[] { 7, 8, 9 });
            var D = new DoubleVector(new double[] { 4, 5 });
            var AouterD = new DoubleMatrix(new double[][] { new double[] { 4, 5 }, new double[] { 8, 10 }, new double[] { 12, 15 } });
            var ATimesC = 50f;

            //addition
            Assert.AreEqual(ATimes2, A + B);
            //subtraction
            Assert.AreEqual(A0, A - B);
            //scalar multiplication
            Assert.AreEqual(ATimes2, 2 * A);
            //scalar multiplication
            Assert.AreEqual(ATimes2, A * 2);
            //negate
            Assert.AreEqual(minusA, -A);
            //Scalar division
            Assert.AreEqual(A, ATimes2 / 2);
            //Element-wise multiplication
            Assert.AreEqual(A0, A & A0);
            //vector multiplication
            Assert.AreEqual(ATimesC, A * C);
            //vector outer product
            Assert.AreEqual(AouterD, A ^ D);
            //vector norm
            Assert.AreEqual((double)System.Math.Sqrt(14), A.EuclidLength);
            //Matrix-Vector multiplication
            DoubleMatrix M = new DoubleMatrix(new double[][] { new double[] { 2, 0, 0 }, new double[] { 0, 2, 0 }, new double[] { 0, 0, 2 } });
            Assert.AreEqual(ATimes2, M * A);
            Assert.AreEqual(ATimes2, A * M);
        }
        [TestMethod()]
        public void DoubleVectorTestReadOnlyCollectionGet()
        {
            var dataA = new double[] { 1, 2, 3 };
            var A = new DoubleVector(dataA);
            var ROCollection = A.GetReadOnlyValuesCollection();
            for (int i = 0; i < A.Size; i++)
            {
                Assert.AreEqual(ROCollection[i], dataA[i]);
            }
        }
    }
}