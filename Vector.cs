using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene
{
    public struct Vector
    {
        private readonly double _x;
        private readonly double _y;
        public double X { get { return _x; } }
        public double Y { get { return _y; } }

        public Vector(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public double Magnitude
        {
            get
            {
                return Math.Sqrt((_x * _x) + (_y * _y));
            }
        }
    }
}
