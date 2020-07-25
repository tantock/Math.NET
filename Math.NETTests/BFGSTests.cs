using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                for(int i = 0; i < x.Size; i++)
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

            var initialEstimate = new Vector<double>(new double[]{ 1, 4, -5 });

            minimize.Minimize(initialEstimate);

            var expected = new Vector<double>(new double[] { -5, -5, -5 });

            for(int i = 0; i < minimize.Solution.M; i++)
            {
                for(int j = 0; j < minimize.Solution.N; j++)
                {
                    Assert.AreEqual(expected.Get(i), minimize.Solution.Get(i,j), tollerance);
                }
            }
        }
    }
}
