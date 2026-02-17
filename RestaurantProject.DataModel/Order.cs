using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.DataModel
{
    public class Order
    {
        public int Id { get; set; }
        public Waiter Waiter {  get; set; }
        public DayOfWeek Day { get; set; } 
        public decimal Amount { get; set; }
        public int Guests { get; set; }
        public IList<Dish> Dishes { get; set; }
    }
}
