using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.DataModel
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Restaurant Restaurant { get; set; }
        public override string ToString()
        {
            return $"{Name} {Surname} {Restaurant}";
        }
    }
}
