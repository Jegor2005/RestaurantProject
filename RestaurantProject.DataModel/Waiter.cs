using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.DataModel
{
    public class Waiter : Person
    {
        public IList<Order> Orders { get;set; }
    }
}
