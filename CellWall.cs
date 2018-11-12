using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Graphene
{
    public class CellWall
    {
        public CellWall(CartesianCoord origin, CartesianCoord end)
        {
            Origin = origin;
            End = end;
        }

        public CellWall(Vertex origin, Vertex end)
        {
            Origin = origin.Center;
            End = end.Center;
        }

        public int WallId { get; set; }

        public CartesianCoord Origin { get; set; }

        public CartesianCoord End { get; set; }

        public Line Path { get; set; }

        public override bool Equals(object obj)
        {
            var comparison = (CellWall)obj;
            var check = (Origin.IsConcentricWith(comparison.Origin) && End.IsConcentricWith(comparison.End))
                || (Origin.IsConcentricWith(comparison.End) && End.IsConcentricWith(comparison.Origin));
            return check;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
