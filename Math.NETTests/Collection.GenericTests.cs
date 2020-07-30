using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathDotNET.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.Collections.Generic.Tests
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
    }
}