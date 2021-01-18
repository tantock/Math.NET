using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathDotNET.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace MathDotNET.LinearAlgebra.Tests
{
    [TestClass()]
    public class DoubleMatrixTests
    {
        [TestMethod()]
        public void DoubleMatrixTestConstructor()
        {
            double[][] a_Data = new double[][] { new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 } };
            var A = new DoubleMatrix(a_Data);
            Assert.AreEqual(A.M, 2);
            Assert.AreEqual(A.N, 3);
            for (int i = 0; i < a_Data.Length; i++)
            {
                for (int j = 0; j < a_Data[0].Length; j++)
                {
                    Assert.AreEqual(a_Data[i][j], A.Get(i, j));
                }
            }
        }
        [TestMethod()]
        public void DoubleMatrixTestCopyConstructor()
        {
            double[][] a_Data = new double[][] { new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 } };
            var A = new DoubleMatrix(a_Data);
            var B = new DoubleMatrix(A);

            Assert.AreEqual(A, B);
            A.Set(0, 0, -A.Get(0, 0)); //check that mutation doesn't leak into copied class
            Assert.AreNotEqual(A, B);
        }
        [TestMethod()]
        public void DoubleMatrixTestMathOperators()
        {
            var A = new DoubleMatrix(new double[][] { new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 } });
            var minusA = new DoubleMatrix(new double[][] { new double[] { -1, -2, -3 }, new double[] { -4, -5, -6 } });
            var ATimes2 = new DoubleMatrix(new double[][] { new double[] { 2, 4, 6 }, new double[] { 8, 10, 12 } });
            var A0 = new DoubleMatrix(new double[][] { new double[] { 0, 0, 0 }, new double[] { 0, 0, 0 } });
            var B = new DoubleMatrix(new double[][] { new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 } });
            var C = new DoubleMatrix(new double[][] { new double[] { 10, 20, 30 }, new double[] { 40, 50, 60 }, new double[] { 70, 80, 90 } });
            var ATimesC = new DoubleMatrix(new double[][] { new double[] { 300, 360, 420 }, new double[] { 660, 810, 960 } });
            var Atranspose = new DoubleMatrix(new double[][] { new double[] { 1, 4 }, new double[] { 2, 5 }, new double[] { 3, 6 } });
            var rowVec = new DoubleMatrix(new double[][] { new double[] { 1, 2, 3, 4, 5, 6 } });

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
            //Matrix multiplication
            Assert.AreEqual(ATimesC, A * C);
            //Matrix transpose
            Assert.AreEqual(Atranspose, A.T);
            //Matrix norm
            Assert.AreEqual((double)System.Math.Sqrt(91), A.Norm);
            //Matrix downcast
            Assert.AreEqual(91.0, rowVec * rowVec.T);
        }

        [TestMethod()]
        public void DoubleMatrixTestTransposeMathOperators()
        {
            var minusA = new DoubleMatrix(new double[][] { new double[] { -1, -2, -3 }, new double[] { -4, -5, -6 } });
            var ATimes2 = new DoubleMatrix(new double[][] { new double[] { 2, 4, 6 }, new double[] { 8, 10, 12 } });
            var A0 = new DoubleMatrix(new double[][] { new double[] { 0, 0, 0 }, new double[] { 0, 0, 0 } });
            var B = new DoubleMatrix(new double[][] { new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 } });
            var C = new DoubleMatrix(new double[][] { new double[] { 10, 20, 30 }, new double[] { 40, 50, 60 }, new double[] { 70, 80, 90 } });
            var ATimesC = new DoubleMatrix(new double[][] { new double[] { 300, 360, 420 }, new double[] { 660, 810, 960 } });
            var Atranspose = new DoubleMatrix(new double[][] { new double[] { 1, 4 }, new double[] { 2, 5 }, new double[] { 3, 6 } });

            //addition
            Assert.AreEqual(ATimes2, Atranspose.T + B);
            //subtraction
            Assert.AreEqual(A0, Atranspose.T - B);
            //scalar multiplication
            Assert.AreEqual(ATimes2, 2 * Atranspose.T);
            //scalar multiplication
            Assert.AreEqual(ATimes2, Atranspose.T * 2);
            //negate
            Assert.AreEqual(minusA, -Atranspose.T);
            //Scalar division
            Assert.AreEqual(Atranspose.T, ATimes2 / 2);
            //Element-wise multiplication
            Assert.AreEqual(A0, Atranspose.T & A0);
            //Matrix multiplication
            Assert.AreEqual(ATimesC, Atranspose.T * C);
            //Matrix norm
            Assert.AreEqual((double)System.Math.Sqrt(91), Atranspose.T.Norm);
        }
        [TestMethod()]
        public void DoubleMatrixTestReadOnlyCollectionGet()
        {
            var dataA = new double[][] { new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 } };
            var A = new DoubleMatrix(dataA);
            var ROCollection = A.GetReadOnlyValuesCollection();
            for (int i = 0; i < A.M; i++)
            {
                for (int j = 0; j < A.N; j++)
                {
                    Assert.AreEqual(ROCollection[i][j], dataA[i][j]);
                }
            }
        }
    }
}