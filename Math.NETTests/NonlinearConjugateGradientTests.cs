using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathDotNET.NumericalOptimizer;
using System;
using System.Collections.Generic;
using System.Text;
using MathDotNET.LinearAlgebra;

namespace MathDotNET.NumericalOptimizer.Tests
{
    [TestClass()]
    public class NonlinearConjugateGradientTests
    {
        [TestMethod()]
        public void NonlinearConjugateGradientTestsMinimize()
        {
            double tollerance = 1e-8;
            var minimize = new NonlinearConjugateGradient((x) =>
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
                Vector<double> z = new Vector<double>(x.Size);
                for (int i = 0; i < x.Size; i++)
                {
                    z.Set(i, 2 * (x.Get(i) + 5));
                }
                return z;
            });

            var initialEstimate = new Vector<double>(new double[] { 1, 4, -5 });

            var t = minimize.Minimize(initialEstimate, new System.Threading.CancellationTokenSource());

            t.ContinueWith((antecedent) =>
            {
                var expected = new Vector<double>(new double[] { -5, -5, -5 });

                for (int i = 0; i < minimize.Solution.Size; i++)
                {
                    Assert.AreEqual(expected.Get(i), minimize.Solution.Get(i), tollerance);
                }
            });

        }
    }
}