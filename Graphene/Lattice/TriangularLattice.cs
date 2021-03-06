﻿using Graphene.Geometry;
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
        public Ellipse[] Cursors { get; set; }
        double _numerator;
        double _lineCount;

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
            init(grid, orientation, latticeType);
            double xPos = -grid.OriginX + (Hex.UnitWidth / 2);
            double yPos = (_lineCount * Hex.UnitHeight / 2);
            int i = 0;
            double z = -Math.Floor(_lineCount / 2);
            while (yPos >= (-grid.OriginY))
            {
                double x = calculateBaseXCoordinate(i);
                double y = -x - z;
                while(xPos <= (grid.OriginX))
                {
                    var labelText = siteId + ": " + x + ", " + y + ", " + z;
                    var location = orientation == Orientation.Horizontal ? new CartesianCoord(xPos, yPos) : new CartesianCoord(yPos, xPos);
                    createNewSite(latticeType, x, y, z, labelText, location);
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

        public TriangularLattice(BaseGrid grid, Orientation orientation, LatticeTypeEnum latticeType, bool alt)
        {
            init(grid, orientation, latticeType);
            double z = Math.Floor(_lineCount / 2);
            //TODO: Swap out the "xPos and yPos variables + while loops" with "cell limit per row + for loop"
            double yPos = (_lineCount * Hex.UnitHeight / 2);
            double xPos = -grid.OriginX + ((Hex.UnitWidth / 2) * Math.Abs(z % 2));
            while (yPos >= (-grid.OriginY))
            {
                double x = calculateBaseXCoordinate(z);
                double y = -x - z;
                while (xPos <= (grid.OriginX))
                {
                    var labelText = siteId + ": " + x + ", " + y + ", " + z;
                    var hexLocation = new HexCoord(x, y, z);
                    var location = hexLocation.ToCartesian();
                    //var location2 = orientation == Orientation.Horizontal ? new CartesianCoord(xPos, yPos) : new CartesianCoord(yPos, xPos);
                    //if (siteId == 25 || siteId == 7)
                    //{

                    //}
                    //Console.WriteLine(labelText + " [" + xPos + "<" + grid.OriginX + "]");
                    createNewSite(latticeType, x, y, z, labelText, location);

                    xPos += Hex.UnitWidth;
                    //if (xPos > grid.OriginX)
                        //Console.WriteLine(xPos + ">" + grid.OriginX + "----");
                    x++;
                    y--;
                }
                xPos = (-grid.OriginX) + ((Hex.UnitWidth / 2) * Math.Abs((1 + z) % 2));
                yPos -= Hex.UnitHeight;
                z--;
            }
        }

        private void createNewSite(LatticeTypeEnum latticeType, double x, double y, double z, string labelText, CartesianCoord location)
        {
            var site = new Site(location, siteId++, x, y, z, latticeType);
            var label = new TextBlock() { FontSize = 10, Text = labelText };
            //Console.WriteLine(labelText);
            AddToXRanks(site);
            AddToYRanks(site);
            AddToZRanks(site);
            Grid.AddShape(site.Marker, location);
            Grid.Add(label, location);
            Sites.Add(site.Id, site);
        }

        private void init(BaseGrid grid, Orientation orientation, LatticeTypeEnum latticeType)
        {
            Grid = grid;
            grid.Canvas.MouseDown += canvasClick;
            Orientation = orientation;
            Sites = new Dictionary<int, Site>();
            Cursors = new Ellipse[3];
            GridLines = new Dictionary<int, HexLine>();
            _numerator = orientation == Orientation.Horizontal ? grid.Height : grid.Width;
            _lineCount = getMaximumLineCount(_numerator);
        }

        private void canvasClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var senderType = sender.GetType();
            var position = e.GetPosition(sender as Canvas);
            var nearestSites = GetNearestSitesToPoint(new CartesianCoord(position, false));
            drawHexLines(nearestSites);
            //var nearestLines = getNearestLinesToPoint(new Coord(position.X, position.Y));
        }

        private void drawHexLines(Site[] sites)
        {
            var lines = sites.SelectMany(s => s.HexLines);
            lines = lines.Where(l => sites.Contains(l.Source) && sites.Contains(l.Target)).Distinct();
            foreach (var line in lines)
            {
                line.Toggle();
            }
        }

        private List<HexLine> getNearestLinesToPoint(CartesianCoord point)
        {
            throw new NotImplementedException();
        }

        public Site[] GetNearestSitesToPoint(CartesianCoord point)
        {
            var hexCoord = point.ToHexCoord();
            var rx = Math.Round(hexCoord.X);
            var ry = Math.Round(hexCoord.Y);
            var rz = Math.Round(hexCoord.Z);
            
            var allSites = getSites(hexCoord.X, hexCoord.Y, hexCoord.Z);
            //HighlightSites(allSites);

            var xDiff = Math.Abs(hexCoord.X % 1);
            var yDiff = Math.Abs(hexCoord.Y % 1);
            var zDiff = Math.Abs(hexCoord.Z % 1);


            if (xDiff > yDiff && xDiff > zDiff)
                rx = -ry - rz;
            else if (yDiff > zDiff)
                ry = -rx - rz;
            else
                rz = -rx - ry;

            //var roundedHex = new HexCoord(rx, ry, rz);
            //var nearestSite = Sites.Where(s => s.Value.X == rx && s.Value.Y == ry && s.Value.Z == rz).SingleOrDefault();
            //MainWindow.Label.Text = roundedHex.ToString();
            //if (nearestSite.Value != null)
            //    HighlightSite(nearestSite.Value);
            return allSites;
        }

        private Site getXSite(double rx, double ry, double rz)
        {
            rx = -ry - rz;
            return getSite(rx, ry, rz);
        }

        private Site[] getSites(double x, double y, double z)
        {
            var siteList = new List<Site>();
            var xLower = Math.Floor(x);
            var xUpper = Math.Ceiling(x);
            var yLower = Math.Floor(y);
            var yUpper = Math.Ceiling(y);
            var zLower = Math.Floor(z);
            var zUpper = Math.Ceiling(z);
            siteList.Add(getSite(xLower, yLower, zLower));
            siteList.Add(getSite(xLower, yLower, zUpper));
            siteList.Add(getSite(xLower, yUpper, zLower));
            siteList.Add(getSite(xLower, yUpper, zUpper));
            siteList.Add(getSite(xUpper, yLower, zLower));
            siteList.Add(getSite(xUpper, yLower, zUpper));
            siteList.Add(getSite(xUpper, yUpper, zLower));
            siteList.Add(getSite(xUpper, yUpper, zUpper));
            return siteList.Where(s => s != null).Distinct().ToArray();
        }

        private Site getYSite(double rx, double ry, double rz)
        {
            ry = -rx - rz;
            return getSite(rx, ry, rz);
        }

        private Site getZSite(double rx, double ry, double rz)
        {
            rz = -rx - ry;
            return getSite(rx, ry, rz);
        }

        private Site getSite(double x, double y, double z)
        {
            return Sites.Where(s => s.Value.X == x && s.Value.Y == y && s.Value.Z == z).SingleOrDefault().Value;
        }

        private double calculateBaseXCoordinate(double rank)
        {
            double valueAtAxis = -Math.Round(Constants.BaseGridWidth / (2 * Hex.UnitWidth));
            var adjustment = (rank / 2) - (Math.Abs(rank % 2) / 2);
            var returnValue = valueAtAxis - adjustment;
            //Console.WriteLine(rank + ", " + (rank % 2) / 2);
            return valueAtAxis - adjustment;
        }

        private double getMaximumLineCount(double numerator)
        {
            var count = Math.Ceiling(numerator / Hex.UnitHeight);
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

        public HexLine AddGridLine(Site origin, Site target, int lineId, BaseGrid grid)
        {
            var line = new HexLine(origin, target, lineId, Grid);
            GridLines.Add(lineId, line);
            return line;
        }

        public void AddGridLines(Site site)
        {
            var dimensions = Enum.GetValues(typeof(Dimension));
            foreach (Dimension dimension in dimensions)
            {
                var neighbour = site.FindNeighbour(dimension, 1);
                if (neighbour != null)
                {
                    int thisLineId = _lineId++;
                    AddGridLine(site, neighbour, thisLineId, Grid);
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
            Grid.AddShape(Cursor, site.Location);
        }

        public void HighlightSites(params Site[] sites)
        {
            foreach (var cursor in Cursors)
            {
                if (Grid.Canvas.Children.Contains(cursor))
                    Grid.Canvas.Children.Remove(cursor);
            }
            for (int i = 0; i < sites.Length; i++)
            {
                //Console.WriteLine(sites[i]);
                var cursor = new Ellipse();
                cursor.Height = 20;
                cursor.Width = 20;
                cursor.Stroke = Brushes.Cornsilk;
                Cursors[i] = cursor;
                Grid.AddShape(cursor, sites[i].Location);
            }
            //Console.WriteLine("----");
        }
    }
}
