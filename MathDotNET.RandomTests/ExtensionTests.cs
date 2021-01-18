using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathDotNET.Random;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET.Random.Tests
{
    [TestClass()]
    public class ExtensionTests
    {
        [TestMethod()]
        public void BinomialTest()
        {
            System.Random rnd = new System.Random();
            List<int> list = new List<int>();
            for(int i = 0; i < 10; i++)
            {
                list.Add((int)rnd.Binomial(1, 0.9));
            }
            System.Diagnostics.Debug.WriteLine(list.ToString());
        }
    }
}