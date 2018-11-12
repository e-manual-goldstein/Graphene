using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.UnitTest
{
    [TestClass]
    public class CoordTests
    {
        [TestMethod]
        public void constructorTest()
        {
            var height = Constants.BaseGridHeight = 100;
            var width = Constants.BaseGridWidth = 200;
            var margin = Constants.DefaultMarginSize = 30;
            var origin = new CartesianCoord(0, 0);
            var convertAbsolute = origin.ToAbsolute();
            var convertOriginal = convertAbsolute.ToOriginal();
            var topLeft = new CartesianCoord(0, 0).ToOriginal();
            //Assert.AreEqual(origin, altOrigin);
            //var x = Constants.BaseGridWidth + Constants.DefaultMarginSize * 2
        }
    }
}
