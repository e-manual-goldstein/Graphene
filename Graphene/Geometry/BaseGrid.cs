using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphene.Geometry
{
    public class BaseGrid
    {
        public BaseGrid(double windowHeight, double windowWidth, Canvas canvas)
        {
            OriginX = (windowWidth - Constants.DefaultMarginSize * 2) / 2;
            OriginY = (windowHeight + Constants.DefaultMarginSize * 2) / 2;
            Height = windowHeight - Constants.DefaultMarginSize * 2;
            Width = windowWidth - Constants.DefaultMarginSize * 2;
            Canvas = canvas;
            if (Constants.ShowGridLines)
                drawGridLines();
        }

        private void drawGridLines()
        {
            drawAxes();
        }

        private void drawAxes()
        {
            var label = new TextBlock();
            label.Text = "label";

            var xAxis = new Line();
            xAxis.X1 = 0;
            xAxis.Y1 = OriginY;
            xAxis.X2 = Width;
            xAxis.Y2 = OriginY;
            xAxis.StrokeThickness = 1;
            xAxis.Stroke = Brushes.Gray;

            var yAxis = new Line();

            yAxis.X1 = OriginX;
            yAxis.Y1 = 0;

            yAxis.X2 = OriginX;
            yAxis.Y2 = Height;


            yAxis.StrokeThickness = 1;
            yAxis.Stroke = Brushes.Gray;

            AddElement(xAxis);

            AddElement(yAxis);
        }

        public double OriginX { get; set; }

        public double OriginY { get; set; }

        public Canvas Canvas { get; set; }

        public void Add(UIElement element, double xGridReference, double yGridReference)
        {
            Canvas.SetLeft(element, OriginX + xGridReference + Constants.DefaultMarginSize);
            Canvas.SetBottom(element, OriginY + yGridReference - Constants.DefaultMarginSize);
            AddElement(element);
        }

        public void Add(UIElement element, CartesianCoord location)
        {
            Add(element, location.X, location.Y);
        }

        public void AddShape(FrameworkElement element, CartesianCoord center)
        {
            Add(element, center - new CartesianCoord(element.Width / 2, element.Height / 2));
        }

        public void AddElement(UIElement element)
        {
            Canvas.Children.Add(element);
        }

        public double Height
        {
            get { return Constants.BaseGridHeight; }
            set { Constants.BaseGridHeight = value; }
        }

        public double Width
        {
            get { return Constants.BaseGridWidth; }
            set { Constants.BaseGridWidth = value; }
        }

        
        //public Line  XAxis { get; set; }

        //public Line  YAxis { get; set; }
    }
}
