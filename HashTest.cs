using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene
{
    public class HashTest
    {
        public int Id { get; set; }

        public HashTest(int id)
        {
            Id = id;
            var hashCode = GetHashCode();
            Console.WriteLine(Id + " = " + hashCode);
        }

        public override int GetHashCode()
        {
            return (Id * 23) ^ 1;
        }
    }
}
