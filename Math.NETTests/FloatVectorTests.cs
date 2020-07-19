﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathDotNET.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.LinearAlgebra.Tests
{
    [TestClass()]
    public class FloatVectorTests
    {
        [TestMethod()]
        public void FloatVectorTestConstructor()
        {
            float[] a_data = new float[] { 1, 2, 3 };
            var A = new FloatVector(a_data);
            Assert.AreEqual(A.Size, 3);
            for (int i = 0; i < a_data.Length; i++)
            {
                Assert.AreEqual(a_data[i], A.Get(i));
            }
        }
        [TestMethod()]
        public void FloatVectorTestCopyConstructor()
        {
            float[] a_data = new float[] { 1, 2, 3 };
            var A = new FloatVector(a_data);
            var B = new FloatVector(A);
            Assert.AreEqual(A, B);
            A.Set(0, -A.Get(0)); //check that mutation doesn't leak into copied class
            Assert.AreNotEqual(A, B);
        }

        [TestMethod()]
        public void FloatVectorTestMathOperators()
        {
            var A = new FloatVector(new float[] { 1, 2, 3 });
            var minusA = new FloatVector(new float[] { -1, -2, -3 });
            var ATimes2 = new FloatVector(new float[] { 2, 4, 6 });
            var A0 = new FloatVector(new float[] { 0, 0, 0 });
            var B = new FloatVector(new float[] { 1, 2, 3 });
            var C = new FloatVector(new float[] { 7, 8, 9 });
            
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
            //vector norm
            Assert.AreEqual((float)System.Math.Sqrt(14), A.Length);
            //Matrix-Vector multiplication
            FloatMatrix M = new FloatMatrix(new float[][] { new float[] { 2, 0, 0 }, new float[] { 0, 2, 0 }, new float[] { 0, 0, 2 } });
            Assert.AreEqual(ATimes2, M * A);
            Assert.AreEqual(ATimes2, A * M);
        }
    }
}