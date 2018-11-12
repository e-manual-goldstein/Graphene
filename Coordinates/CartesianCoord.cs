using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Graphene.Lattice;

namespace Graphene
{
    public class CartesianCoord : Coord
    {
        public CartesianCoord(double x, double y)
        {
            X = x;
            Y = y;
        }
        
        public CartesianCoord(Point point, bool absolute = true)
        {
            X = absolute ? point.X : point.X - ((Constants.BaseGridWidth + Constants.DefaultMarginSize * 2) / 2); ;
            Y = absolute ? point.Y : ((Constants.BaseGridHeight + Constants.DefaultMarginSize * 2) / 2) - point.Y;
        }


        public CartesianCoord Add(Dimension dimension, int magnitude)
        {
            switch (dimension)
            {
                case Dimension.Alpha:
                    return AddAlpha(magnitude);
                case Dimension.Beta:
                    return AddBeta(magnitude);
                case Dimension.Gamma:
                    return AddGamma(magnitude);
                default:
                    throw new ArgumentException("Unknown Dimension");
            }
        }

        public bool IsConcentricWith(CartesianCoord coordinate)
        {
            var check = (Math.Abs(X - coordinate.X) / X < 0.01 && Math.Abs(Y - coordinate.Y) / Y < 0.01);
            //if (check)
            //    Console.WriteLine("Within Limits: DX = " + (X - coordinate.X) / X + ", DY = " + (Y - coordinate.Y) / Y);
            //else
            //    Console.WriteLine("Outside Limits: DX = " + (X - coordinate.X) / X + ", DY = " + (Y - coordinate.Y) / Y);
            return (Math.Abs(X - coordinate.X) / X < 0.01 && Math.Abs(Y - coordinate.Y) / Y < 0.01);
        }

        public CartesianCoord AddAlpha(int magnitude)
        {
            var newX = X - (magnitude * Hex.UnitWidth / 2);
            var newY = Y + (magnitude * Hex.UnitHeight);
            return new CartesianCoord(newX, newY);
        }

        public CartesianCoord AddBeta(int magnitude)
        {
            var newX = X + (magnitude * Hex.UnitWidth / 2);
            var newY = Y + (magnitude * Hex.UnitHeight);
            return new CartesianCoord(newX, newY);
        }

        public CartesianCoord AddGamma(int magnitude)
        {
            var newX = X + (magnitude * Hex.UnitWidth);
            var newY = Y;
            return new CartesianCoord(newX, newY);
        }

        public static CartesianCoord operator +(CartesianCoord coord, CartesianCoord vector)
        {
            return new CartesianCoord(coord.X + vector.X, coord.Y + vector.Y);
        }

        public static CartesianCoord operator -(CartesianCoord coord, CartesianCoord vector)
        {
            return new CartesianCoord(coord.X - vector.X, coord.Y - vector.Y);
        }

        public CartesianCoord ToAbsolute()
        {
            var absoluteX = ((Constants.BaseGridWidth + Constants.DefaultMarginSize * 2) / 2) + X;
            var absoluteY = ((Constants.BaseGridHeight + Constants.DefaultMarginSize * 2) / 2) - Y;
            return new CartesianCoord(absoluteX, absoluteY);
        }

        public CartesianCoord ToOriginal()
        {
            var originX = X - ((Constants.BaseGridWidth + Constants.DefaultMarginSize * 2) / 2);
            var originY = ((Constants.BaseGridHeight + Constants.DefaultMarginSize * 2) / 2) - Y;
            return new CartesianCoord(originX, originY);
        }

        public HexCoord ToHexCoord()
        {
            var x = (Math.Sqrt(3) / 3 * X - 1 / 3 * Y) / Constants.UnitLength;
            var z = (Y * 2 / 3) / Constants.UnitLength;
            var y = -x - z;
            return new HexCoord(x, y, z);
        }
    }
}
