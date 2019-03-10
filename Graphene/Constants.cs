using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene
{
    public static class Constants
    {
        public static double SiteMarkerSize = 6;

        public static double UnitLength = 60;

        public static double DefaultMarginSize = 50;

        public static bool ShowGridLines = false;

        public static double BaseGridWidth { get; set; }
        public static double BaseGridHeight { get;  set; }
    }

    public static class Hex
    {
        public static double UnitWidth
        {
            get { return 2 * Constants.UnitLength; }
        }

        public static double UnitHeight
        {
            get { return Math.Sqrt(3) * Constants.UnitLength; }
        }

        public static bool ShowGridLines = true;
    }
}
