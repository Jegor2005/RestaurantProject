using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.DataModel
{
    public class Dish
    {
        public string Name { get; set; }
        public decimal Price {  get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}
