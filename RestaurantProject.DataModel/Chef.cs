using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.DataModel
{
    public class Chef : Person
    {
        public Dish ChefsDish { get; set; }
    }
}
