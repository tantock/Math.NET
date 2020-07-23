using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathDotNET.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.LinearAlgebra.Tests
{
    [TestClass()]
    public class MatrixTests
    {
        [TestMethod()]
        public void FloatMatrixTestConstructor()
        {
            float[][] a_Data = new float[][] { new float[] { 1, 2, 3 }, new float[] { 4, 5, 6 } };
            var A = new Matrix<float>(a_Data);
            Assert.AreEqual(A.M, 2);
            Assert.AreEqual(A.N, 3);
            for(int i = 0; i < a_Data.Length; i++)
            {
               for(int j = 0; j < a_Data[0].Length; j++)
                {
                    Assert.AreEqual(a_Data[i][j], A.Get(i, j));
                }
            }
        }
        [TestMethod()]
        public void FloatMatrixTestCopyConstructor()
        {
            float[][] a_Data = new float[][] { new float[] { 1, 2, 3 }, new float[] { 4, 5, 6 } };
            var A = new Matrix<float>(a_Data);
            var B = new Matrix<float>(A);

            Assert.AreEqual(A, B);
            A.Set(0, 0, -A.Get(0,0)); //check that mutation doesn't leak into copied class
            Assert.AreNotEqual(A, B);
        }
        [TestMethod()]
        public void FloatMatrixTestMathOperators()
        {
            var A = new Matrix<float>(new float[][] { new float[] { 1, 2, 3 }, new float[] { 4, 5, 6 } });
            var minusA = new Matrix<float>(new float[][] { new float[] { -1, -2, -3 }, new float[] { -4, -5, -6 } });
            var ATimes2 = new Matrix<float>(new float[][] { new float[] { 2, 4, 6 }, new float[] { 8, 10, 12 } });
            var A0 = new Matrix<float>(new float[][] { new float[] { 0, 0, 0 }, new float[] { 0, 0, 0 } });
            var B = new Matrix<float>(new float[][] { new float[] { 1, 2, 3 }, new float[] { 4, 5, 6 } });
            var C = new Matrix<float>(new float[][] { new float[] { 10, 20, 30 }, new float[] { 40, 50, 60 }, new float[] { 70, 80, 90 } });
            var ATimesC = new Matrix<float>(new float[][] { new float[] { 300, 360, 420 }, new float[] { 660, 810, 960 } });
            var Atranspose = new Matrix<float>(new float[][] { new float[] { 1, 4 }, new float[] { 2, 5 }, new float[] { 3, 6 } });

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
            Assert.AreEqual((float)System.Math.Sqrt(91), A.Norm);
        }

        [TestMethod()]
        public void FloatMatrixTestTransposeMathOperators()
        {
            var minusA = new Matrix<float>(new float[][] { new float[] { -1, -2, -3 }, new float[] { -4, -5, -6 } });
            var ATimes2 = new Matrix<float>(new float[][] { new float[] { 2, 4, 6 }, new float[] { 8, 10, 12 } });
            var A0 = new Matrix<float>(new float[][] { new float[] { 0, 0, 0 }, new float[] { 0, 0, 0 } });
            var B = new Matrix<float>(new float[][] { new float[] { 1, 2, 3 }, new float[] { 4, 5, 6 } });
            var C = new Matrix<float>(new float[][] { new float[] { 10, 20, 30 }, new float[] { 40, 50, 60 }, new float[] { 70, 80, 90 } });
            var ATimesC = new Matrix<float>(new float[][] { new float[] { 300, 360, 420 }, new float[] { 660, 810, 960 } });
            var Atranspose = new Matrix<float>(new float[][] { new float[] { 1, 4 }, new float[] { 2, 5 }, new float[] { 3, 6 } });

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
            Assert.AreEqual((float)System.Math.Sqrt(91), Atranspose.T.Norm);
        }
        [TestMethod()]
        public void FloatMatrixTestReadOnlyCollectionGet()
        {
            var dataA = new float[][] { new float[] { 1, 2, 3 }, new float[] { 4, 5, 6 } };
            var A = new Matrix<float>(dataA);
            var ROCollection = A.GetReadOnlyValuesCollection();
            for(int i = 0; i < A.M; i++)
            {
                for(int j = 0; j < A.N; j++)
                {
                    Assert.AreEqual(ROCollection[i][j], dataA[i][j]);
                }
            }
        }
    }
}