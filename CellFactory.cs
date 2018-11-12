using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene
{
    public static class CellFactory
    {
        public static void AddToVertices(HexGrid hexGrid, Cell cell, Vertex vertex)
        {
            //if (!hexGrid.AllVertices.Contains(vertex))
            //    cell.Vertices.Add(vertex);
            //else
            //{
            //    var existingVertex = AllVertices.FirstOrDefault(v => v.Equals(vertex));
            //    AllVertices.Add(vertex);
            //}
        }

        public static CartesianCoord GetHexCorner(CartesianCoord center, double size, int corner)
        {
            var degrees = 60 * corner + 30;
            var radians = Math.PI / 180 * degrees;
            return new CartesianCoord(center.X + size * Math.Cos(radians), center.Y + size * Math.Sin(radians));
        }
    }
}
