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
    public class DisplayProgrammaticService : IService
    {
        public DisplayProgrammaticService()
        { }

        public void DrawLine(Canvas tableau, Line path, Color black)
        {
            
            tableau.Children.Add(path);
        }
    }
}
