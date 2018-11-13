using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene
{
    public class MainController
    {
        //TODO: Finish or Drop this
        public List<IService> InitialiseServices(MainWindow mainWindow)
        {
            var members = mainWindow.GetType().GetProperties();
            foreach (var member in members) //.OrderBy(t => t.GetType()))
            {
                var interfaces = member.GetMethod.ReturnType.GetInterfaces();
                if (interfaces.Contains(typeof(IService)))
                {
                    var newType = Activator.CreateInstance(member.PropertyType);
                    member.SetValue(member, newType);
                    Console.WriteLine(member);
                }
            }
            return new List<IService>();
        }
    }
}
