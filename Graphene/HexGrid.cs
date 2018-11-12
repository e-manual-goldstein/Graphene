using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphene
{
    public class HexGrid
    {
        //public DisplayController DisplayController = new DisplayController();
        #region Constructors

        public HexGrid(int columnCount, int rowCount, int cellSize, CartesianCoord gridOrigin, Orientation orientation = Orientation.Horizontal)
        {
            VertexCounter = 0;
            GridLines = new List<Line>();
            ColumnCount = columnCount;
            RowCount = rowCount;
            //var y = (orientation == Orientation.Horizontal) ? rowCount : columnCount;
            //var x = (orientation == Orientation.Horizontal) ? columnCount : rowCount;
            //Generate all Vertices
            var vertices = new List<Vertex>();
            var channels = new List<Channel>();
            double xSpacing;
            double ySpacing;
            for (int i = 0; i < rowCount; i++)
            {
                var parity = i % 2;
                xSpacing = 2 * cellSize * Math.Cos(Math.PI / 180 * 30);
                ySpacing = xSpacing * Math.Cos(Math.PI / 180 * 30);

                for (int j = 0; j < columnCount; j++)
                {
                    for (int n = 0; n < 6; n++)
                    {
                        CartesianCoord coord;
                        if (orientation == Orientation.Vertical)
                        {
                            coord = new CartesianCoord(gridOrigin.X + (parity * xSpacing / 2) + xSpacing * j, gridOrigin.Y + i * ySpacing);
                        }
                        else
                        {
                            coord = new CartesianCoord(gridOrigin.Y + i * ySpacing, gridOrigin.X + (parity * xSpacing / 2) + xSpacing * j);
                        }
                        var vertex = new Vertex(coord,cellSize,n,orientation);
                        if (!vertices.Any(v => v.IsConcentricWith(vertex)))
                        {
                            vertices.Add(vertex);
                            VertexCounter++;
                            vertex.Id = VertexCounter;
                        }
                        var nextCorner = new Vertex(coord, cellSize, n + 1, orientation);
                        var channel = new Channel(vertex, nextCorner);
                        if (!channels.Any(c => c.IsCoherentWith(channel)))
                        {
                            channels.Add(channel);
                            ChannelCounter++;
                            channel.Id = ChannelCounter;
                        }
                    }
                }
            }
            AllVertices = vertices;
            AllChannels = channels;
            //Generate all Channels/Lines/Connectors (come up with a name)
        }

        #endregion

        #region Properties

        public List<Cell> Cells { get; set; }

        public List<CellWall> AllCellWalls
        {
            get
            {
                return Cells.SelectMany(c => c.CellWalls).ToList();
            }
        }

        public List<Vertex> AllVertices { get; set; }
        
        public List<Channel> AllChannels { get; set; }
        
        public void AddToCellWalls(CellWall wall)
        {
            if (!AllCellWalls.Contains(wall))
                AllCellWalls.Add(wall);
            else
            {
                var existingWall = AllCellWalls.FirstOrDefault(w => w.Equals(wall));
                AllCellWalls.Add(existingWall);
            }
        }

        public void AddToVertices(Vertex vertex)
        {
            if (!AllVertices.Contains(vertex))
                AllVertices.Add(vertex);
            else
            {
                var existingVertex = AllVertices.FirstOrDefault(v => v.Equals(vertex));
                AllVertices.Add(vertex);
            }
        }

        public int Step { get; set; }

        public List<Line> GridLines { get; set; }

        //Used to generate unique IDs for the beams
        public int BeamCounter { get; set; }

        //Used to generate unique IDs for the blocks
        public int BlockCounter { get; set; }

        //Used to generate unique IDs for the Cells
        public int CellCounter { get; set; }

        public int ChannelCounter { get; set; }

        public int VertexCounter { get; set; }

        //public List<Beam> AllBeams { get; set; }

        //public List<Tile> AllBlocks { get; set; }

        //public List<Row> Rows { get; set; }

        //public List<Column> Columns { get; set; }

        //public List<Edge> Interstitials { get; set; }

        public int ColumnCount { get; set; }

        public int RowCount { get; set; }

        public double UnitLength
        {
            get
            {
                return MainWindow.UnitSize;
            }
        }

        public double LeftBorder { get; set; }

        public double TopBorder { get; set; }

        public double GridWidth
        {
            get
            {
                return ColumnCount * UnitLength;
            }
        }

        public double GridHeight
        {
            get
            {
                return RowCount * UnitLength;
            }
        }
        #endregion

        #region Actions


        #endregion
    }
}
