using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene
{
    public class HexCoord : Coord
    {
        public double Z { get; set; }

        public HexCoord(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }

        public CartesianCoord ToCartesian()
        {
            var x = Constants.UnitLength * (Math.Sqrt(3) * X + Math.Sqrt(3) / 2 * Z);
            var y = Constants.UnitLength * (Z * 3 / 2);
            return new CartesianCoord(x, y);
        }
    }
}
