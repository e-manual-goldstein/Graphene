using System;
using Graphene.Lattice;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graphene.UnitTest
{
    [TestClass]
    public class TriangularLatticeTests
    {
        [TestMethod]
        public void TriangularLattice_Test1()
        {
            var lattice = new TriangularLattice(null, Orientation.Horizontal, LatticeTypeEnum.Red);
        }
    }
}
