using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Graphene
{
    public class Channel
    {
        public Channel(CartesianCoord origin, CartesianCoord end)
        {
            Origin = origin;
            End = end;
            Path = new Line()
                { X1 = origin.X, Y1 = origin.Y, X2 = end.X, Y2 = end.Y};
        }

        public Channel(Vertex origin, Vertex end)
        {
            Origin = origin.Center;
            End = end.Center;
            Path = new Line()
                { X1 = origin.X, Y1 = origin.Y, X2 = end.X, Y2 = end.Y};
        }

        public int Id { get; set; }

        public CartesianCoord Origin { get; set; }

        public CartesianCoord End { get; set; }

        public Line Path { get; set; }

        public bool IsCoherentWith(Channel channel)
        {
            var check = (Origin.IsConcentricWith(channel.Origin) && End.IsConcentricWith(channel.End))
             || (Origin.IsConcentricWith(channel.End) && End.IsConcentricWith(channel.Origin));
            return check;
        }
    }
}
