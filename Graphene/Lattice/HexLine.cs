using Graphene.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphene.Lattice
{
    public class HexLine
    {
        public int Id { get; private set; }
        public Line Line { get; set; }
        public bool Flash { get; set; }
        public Dimension Dimension { get; set; }

        public HexLine(Dimension dimension, Site source, int id, BaseGrid grid)
        {
            Id = id;
            var origin = source.Location;
            var target = origin.Add(dimension, 1);
            Dimension = dimension;
            Line = new Line();
            Line.X1 = origin.ToAbsolute().X;
            Line.Y1 = origin.ToAbsolute().Y;
            Line.X2 = target.ToAbsolute().X;
            Line.Y2 = target.ToAbsolute().Y;
            Line.Stroke = Brushes.Gray;
            Line.StrokeThickness = 5;
            grid.AddElement(Line);
            Line.MouseDown += mouseClick;
        }

        public HexLine(Site source, Site target, int id, BaseGrid grid)
        {
            Id = id;
            Line = new Line();
            Line.X1 = source.Location.ToAbsolute().X;
            Line.Y1 = source.Location.ToAbsolute().Y;
            Line.X2 = target.Location.ToAbsolute().X;
            Line.Y2 = target.Location.ToAbsolute().Y;
            Line.Stroke = Brushes.Gray;
            Line.StrokeThickness = 5;
            grid.AddElement(Line);
            Line.MouseDown += mouseClick;
        }

        private void mouseClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var line = (Line)sender;
            Flash = !Flash;
        }

        public void SwitchOn()
        {
            Canvas.SetZIndex(Line, 1);
            Line.Stroke = Brushes.Red;
            Line.StrokeThickness = 10;   
        }

        public void SwitchOff()
        { 
            Canvas.SetZIndex(Line, 0);
            Line.Stroke = Brushes.Gray;
            Line.StrokeThickness = 10;
        }

    }
}
