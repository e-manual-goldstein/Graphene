using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene
{
    public class Vertex
    {
        public Vertex(CartesianCoord cellCenter, double cellSize, int corner, Orientation orientation = Orientation.Horizontal)
        {
            var angle_deg = 60 * corner + (30 * (int)orientation);
            var angle_rad = Math.PI / 180 * angle_deg;
            Center = new CartesianCoord(cellCenter.X + cellSize * Math.Cos(angle_rad), cellCenter.Y + cellSize * Math.Sin(angle_rad));
        }

        public int Id { get; set; }

        public CartesianCoord Center { get; set; }

        public double X
        {
            get
            {
                return Center.X;
            }
        }

        public double Y
        {
            get
            {
                return Center.Y;
            }
        }

        //public HexCoord Position
        //{
        //    get
        //    {
        //        return HexCoord.FromCoord(Center);
        //    }
        //}

        //public override string ToString()
        //{
        //    return Id.ToString() + ": " + Position;
        //}
        
        public bool IsConcentricWith(Vertex vertex)
        {
            return Center.IsConcentricWith(vertex.Center);
        }
        //TODO: List of adjoining walls
        //TODO: List of adjoining cells
    }
}
