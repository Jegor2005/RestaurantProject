using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.DataModel
{
    public class Restaurant
    {
        public string Color { get; set; }
        public string Address { get; set; }
        public Guid Id { get; set; }
        public decimal Rent { get; set; }
        public Menu Menu { get; set; }
    }
}
