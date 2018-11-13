using Graphene.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphene.Lattice
{
    public class TriangularLattice
    {
        private int siteId = 0;
        private int _lineId = 0;
        public Ellipse Cursor { get; set; }

        [Obsolete]
        public TriangularLattice(BaseGrid grid, int size, Orientation orientation, LatticeTypeEnum latticeType)
        {
            Grid = grid;
            Size = size;
            Orientation = orientation;
            for (int i = 0; i <= Size; i++)
            {
                addShell(i, latticeType);
            }
        }

        public TriangularLattice(BaseGrid grid, Orientation orientation, LatticeTypeEnum latticeType)
        {
            Grid = grid;
            grid.Canvas.MouseDown += canvasClick;
            Orientation = orientation;
            Sites = new Dictionary<int, Site>();
            GridLines = new Dictionary<int, HexLine>();
            var numerator = orientation == Orientation.Horizontal ? grid.Height : grid.Width;
            var lineCount = getMaximumLineCount(numerator);
            double xPos = -grid.OriginX + (Hex.UnitWidth / 2);
            double yPos = (lineCount * Hex.UnitHeight / 2);
            int i = 1;
            double z = -Math.Floor(lineCount / 2);
            while (yPos >= (-grid.OriginY))
            {
                double x = calculateBaseXCoordinate(i);
                double y = -x - z;
                while(xPos <= (grid.OriginX))
                {
                    var labelText = siteId + ": " + x + ", " + y + ", " + z;
                    var location = orientation == Orientation.Horizontal ? new CartesianCoord(xPos, yPos) : new CartesianCoord(yPos, xPos);
                    var site = new Site(location, siteId++, x, y, z, latticeType);
                    var label = new TextBlock() { FontSize = 10, Text = labelText };
                    //Console.WriteLine(labelText);
                    AddToXRanks(site);
                    AddToYRanks(site);
                    AddToZRanks(site);
                    Grid.AddShape(site.Marker, location);
                    //Grid.Add(label, location);
                    Sites.Add(site.Id, site);

                    xPos += Hex.UnitWidth;
                    x++;
                    y--;
                }
                i++;
                xPos = (-grid.OriginX) + ((Hex.UnitWidth / 2) * (i % 2));
                yPos -= Hex.UnitHeight;
                z++;
            }
        }

        private void canvasClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var senderType = sender.GetType();
            var position = e.GetPosition(sender as Canvas);
            var nearestPoints = GetNearestSitesToPoint(new CartesianCoord(position, false));
            //var nearestLines = getNearestLinesToPoint(new Coord(position.X, position.Y));
        }

        private List<HexLine> getNearestLinesToPoint(CartesianCoord point)
        {
            throw new NotImplementedException();
        }

        public List<Site> GetNearestSitesToPoint(CartesianCoord point)
        {
            var hexCoord = point.ToHexCoord();
            var rx = Math.Round(hexCoord.X);
            var ry = Math.Round(hexCoord.Y);
            var rz = Math.Round(hexCoord.Z);

            var xDiff = hexCoord.X % 1;
            var yDiff = hexCoord.Y % 1;
            var zDiff = hexCoord.Z % 1;

            if (xDiff > yDiff && xDiff > zDiff)
                rx = -ry - rz;
            else if (yDiff > zDiff)
                ry = -rx - rz;
            else
                rz = -rx - ry;

            var roundedHex = new HexCoord(rx, ry, rz);
            var nearestSite = Sites.Where(s => s.Value.X == rx && s.Value.Y == ry && s.Value.Z == rz).Single();
            MainWindow.Label.Text = roundedHex.ToString();
            HighlightSite(nearestSite.Value);
            return null;
        }

        private double calculateBaseXCoordinate(double rank)
        {
            double valueAtAxis = -Math.Floor(Constants.BaseGridWidth / (2 * Hex.UnitWidth));
            double distanceToAxis = Math.Floor(Constants.BaseGridHeight / (2 * Hex.UnitHeight)) - rank;
            //double returnValue = ((result - (result % 2)) / 2) - (result % 2);
            return (((valueAtAxis + distanceToAxis) + rank % 2) / 2) - 1;
        }

        private double getMaximumLineCount(double numerator)
        {
            var count = Math.Floor(numerator / Hex.UnitHeight);
            return (count % 2 == 0) ? count-- : count;
        }

        [Obsolete]
        private double stepDown(double y)
        {
            return y - (Math.Sqrt(3) / 2);
        }

        [Obsolete]
        private void addShell(int shellNumber, LatticeTypeEnum latticeType)
        {
            var siteCount = edgeSiteCount(shellNumber);
            for (int i = 0; i < siteCount; i++)
            {
                addSite(shellNumber, siteCount, i, siteId++, latticeType);
            }
        }

        [Obsolete]
        private void addSite(int shellNumber, int shellSiteCount, int shellSiteId, int siteId, LatticeTypeEnum latticeType)
        {
            var location = GetCoordForSite(shellNumber, shellSiteCount, shellSiteId, latticeType);
            //var site = new Site(location, siteId, latticeType);
            //var label = new TextBlock() { FontSize = 20, Text = siteId.ToString() };
            ////Grid.Add(label, location.X, location.Y);
            //Grid.Add(site.Marker, location.X, location.Y);
        }

        public HexLine AddGridLine(Dimension dimension, Site origin, int lineId, BaseGrid grid)
        {
            var line = new HexLine(dimension, origin, lineId, Grid);
            GridLines.Add(lineId, line);
            return line;
        }

        public void AddGridLines(Site site)
        {
            var dimensions = Enum.GetValues(typeof(Dimension));
            foreach (Dimension dimension in dimensions)
            {
                if (site.FindNeighbour(dimension, 1) != null)
                {
                    int thisLineId = _lineId++;
                    AddGridLine(dimension, site, thisLineId, Grid);
                }
            }
        }

        private CartesianCoord GetCoordForSite(int shellNumber, int shellSiteCount, int shellSiteId, LatticeTypeEnum latticeType)
        {
            var radians = ((2 * Math.PI) / shellSiteCount) * shellSiteId;
            var internalRadians = radians % (Math.PI / 3);
            var radius = shellNumber * Constants.UnitLength;
            //cos(a-b) = cos(a)cos(b) + sin(a)sin(b)
            //a = Math.PI / 6
            //b = internalRadians
            var cos30 = Math.Sqrt(3) / 2;
            var sin30 = 0.5;
            var hexRadius = (radius * cos30) / (
                (cos30 * Math.Cos(internalRadians)) + (sin30 * Math.Sin(internalRadians))
                );
            //var adjustment = 2 * (int)latticeType * (Constants.UnitLength / 3);
            var xCoord = hexRadius * Math.Cos(radians);
            var yCoord = hexRadius * Math.Sin(radians); //+ adjustment;
            
            Console.WriteLine((siteId - 1) + "\t[" + shellNumber + "," + shellSiteId + "]\t" + xCoord + "\t" + yCoord);
            return Orientation == Orientation.Horizontal ? new CartesianCoord(xCoord, yCoord) : new CartesianCoord(yCoord, xCoord);
        }

        public IDictionary<int, Site> Sites { get; set; }

        public IDictionary<int, HexLine> GridLines { get; set; }

        #region Ranks

        public IDictionary<int, List<Site>> XRanks { get; set; }

        public void AddToXRanks(Site site)
        {
            var xValue = site.X;
            if (XRanks == null)
            {
                XRanks = new Dictionary<int, List<Site>>();
            }
            if (!XRanks.ContainsKey(xValue))
            {
                XRanks[xValue] = new List<Site>();
            }
            site.XRank = XRanks[xValue];
            XRanks[xValue].Add(site);
        }

        public IDictionary<int, List<Site>> YRanks { get; set; }

        public void AddToYRanks(Site site)
        {
            var yValue = site.Y;
            if (YRanks == null)
            {
                YRanks = new Dictionary<int, List<Site>>();
            }
            if (!YRanks.ContainsKey(yValue))
            {
                YRanks[yValue] = new List<Site>();
            }
            site.YRank = YRanks[yValue];
            YRanks[yValue].Add(site);
        }

        public IDictionary<int, List<Site>> ZRanks { get; set; }

        public void AddToZRanks(Site site)
        {
            var zValue = site.Z;
            if (ZRanks == null)
            {
                ZRanks = new Dictionary<int, List<Site>>();
            }
            if (!ZRanks.ContainsKey(zValue))
            {
                ZRanks[zValue] = new List<Site>();
            }
            site.ZRank = ZRanks[zValue];
            ZRanks[zValue].Add(site);
        }

        #endregion

        public Orientation Orientation { get; set; }

        public BaseGrid Grid { get; set; }

        public int Size { get; set; }

        //public Coord GetCoordForSite(int siteId)
        //{
        //    int shellNumber = 
        //}

        [Obsolete]
        private int totalSiteCount(int shellSize)
        {
            int n = shellSize + 1;
            if (n < 1)
                return 0;
            return 1 + (3 * (n * (n - 1)));
        }

        [Obsolete]
        private int edgeSiteCount(int shellSize)
        {
            int totalSites = totalSiteCount(shellSize);
            int innerSites = totalSiteCount(shellSize - 1);
            return totalSites - innerSites;
        }

        public void HighlightSite(Site site)
        {
            if (Grid.Canvas.Children.Contains(Cursor))
                Grid.Canvas.Children.Remove(Cursor);
            Cursor = new Ellipse();
            Cursor.Height = 20;
            Cursor.Width = 20;
            Cursor.Stroke = Brushes.Cornsilk;
            Grid.AddShape(Cursor, site.Location.ToAbsolute());
        }
    }
}
