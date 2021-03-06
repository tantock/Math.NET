﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MathDotNET.NumericalOptimizer;
using MathDotNET.LinearAlgebra;
using System.Globalization;

namespace MathDotNETTests.NumericalOptimizer
{
    [TestClass()]
    public class BFGSTests
    {
        [TestMethod()]
        public void BFGSTestsMinimize()
        {
            double tollerance = 1e-8;
            var minimize = new BFGSMinimizer((x) =>
            {
                //paraboloid function
                double z = 1;
                for (int i = 0; i < x.Size; i++)
                {
                    z += Math.Pow(x.Get(i) + 5, 2);
                }
                return z;
            },
            (x) =>
            {
                //paraboloid function gradient
                DoubleVector z = new DoubleVector(x.Size);
                for (int i = 0; i < x.Size; i++)
                {
                    z.Set(i, 2 * (x.Get(i) + 5));
                }
                return z;
            });

            var initialEstimate = new DoubleVector(new double[] { 10, 40, -50 });

            var t = minimize.Minimize(initialEstimate, new System.Threading.CancellationTokenSource());
            t.Wait();
            t.ContinueWith((a) =>
            {
                var expected = new Vector<double>(new double[] { -5, -5, -5 });

                for (int i = 0; i < minimize.Solution.Size; i++)
                {
                    Assert.AreEqual(expected.Get(i), minimize.Solution.Get(i), tollerance);
                    System.Diagnostics.Debug.WriteLine(String.Format("Expected:{0},Actual:{1}", expected.Get(i), minimize.Solution.Get(i)));
                }
            });




        }
    }
}
