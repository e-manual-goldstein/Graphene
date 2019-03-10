using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphene.Lattice
{
    public class Site
    {
        public int Id { get; set; }

        public Site(CartesianCoord location, int siteId, double x, double y, double z, LatticeTypeEnum? latticeType = null)
        {
            Id = siteId;
            Location = location;
            Marker = new Ellipse();
            Marker.Height = Constants.SiteMarkerSize;
            Marker.Width = Constants.SiteMarkerSize;
            X = (int) x;
            Y = (int) y;
            Z = (int) z;
            var color = getColorBrushFromLatticeType(latticeType);
            Color = color ?? Brushes.Black;
        }

        private SolidColorBrush getColorBrushFromLatticeType(LatticeTypeEnum? latticeType)
        {
            switch (latticeType)
            {
                case LatticeTypeEnum.Red:
                    return Brushes.Red;
                case LatticeTypeEnum.Green:
                    return Brushes.Green;
                case LatticeTypeEnum.Blue:
                    return Brushes.Blue;
                default:
                    return null;
            }
        }

        public CartesianCoord Location { get; set; }
        
        public Ellipse Marker { get; set; }

        public List<Site> XRank { get; set; }

        public List<Site> YRank { get; set; }

        public List<Site> ZRank { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }

        public Site FindNeighbour(Dimension dimension, int magnitude)
        {
            var rank = GetRank(dimension);
            var filter = FindDimensionFilter(dimension, magnitude);
            return rank.SingleOrDefault(filter);
        }

        public List<Site> GetRank(Dimension dimension)
        {
            switch (dimension)
            {
                case Dimension.Alpha:
                    return XRank;
                case Dimension.Beta:
                    return YRank;
                case Dimension.Gamma:
                    return ZRank;
                default:
                    throw new ArgumentException("Unknown Dimension");
            };
        }

        public Func<Site,bool> FindDimensionFilter(Dimension dimension, int magnitude)
        {
            switch (dimension)
            {
                case Dimension.Alpha:
                    return site => site.Y == Y + magnitude;
                case Dimension.Beta:
                    return site => site.Z == Z - magnitude;
                case Dimension.Gamma:
                    return site => site.X == X + magnitude;
                default:
                    throw new ArgumentException("Unknown Dimension");
            }
        }

        public SolidColorBrush Color
        {
            get => Marker?.Fill as SolidColorBrush;
            set => Marker.Fill = value;
        }
    }
}
