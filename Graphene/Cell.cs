using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Graphene
{
    public class Cell
    {
        //TODO: Creating a cell should:
        //1. Create the walls and add the unique ones to the list of walls
        //2. Create the vertices and add the unique ones to the list of vertices
        //3. Create an enum for labelling the Cell Vertices (Ortho, Para, Meta, Normal, Anti-Ortho, Anti-Meta)
        //4. Create an enum for labelling the Cell Walls, among other things (use Quark types for directions)

        public Cell(CartesianCoord center, int cellSize, Orientation orientation, HexGrid hexGrid)
        {
            Center = center;
            for (int i = 0; i < 6; i++)
            {
                var origin = new Vertex(center, cellSize, i);
                //CellFactory.AddToVertices(hexGrid, this, origin.Center);
                var end = new Vertex(center, cellSize, i + 1);
                hexGrid.AddToVertices(end);
                var wall = new CellWall(origin, end);
                hexGrid.AddToCellWalls(wall);
            }
        }

        public CartesianCoord GetHexCorner(CartesianCoord center, double size, int corner)
        {
            var angle_deg = 60 * corner + 30;
            var angle_rad = Math.PI / 180 * angle_deg;
            return new CartesianCoord(center.X + size * Math.Cos(angle_rad), center.Y + size * Math.Sin(angle_rad));
        }

        public List<CellWall> CellWalls { get; set; }

        public List<Vertex> Vertices { get; set; }

        public CartesianCoord Center { get; set; }

        public Vertex NormalVertex
        {
            get
            {
                return Vertices.FirstOrDefault(v => v.Center.X == Center.X && v.Center.Y < Center.Y);
            }
        }
    }
}
