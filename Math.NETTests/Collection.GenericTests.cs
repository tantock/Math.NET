using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathDotNET.Collections.Statistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.Collections.Statistics.Tests
{
    [TestClass()]
    public class ExtensionTests
    {
        [TestMethod()]
        public void StandardDeviationPTest()
        {
            List<double> list = new List<double>(new double[] { 2,4,4,4,5,5,7,9});
            Assert.AreEqual(2, list.StandardDeviationP());
        }
        [TestMethod()]
        public void VariancePTest()
        {
            List<double> list = new List<double>(new double[] { 2, 4, 4, 4, 5, 5, 7, 9 });
            Assert.AreEqual(4, list.VarianceP());
        }

    }
}