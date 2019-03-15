using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManadger.Model
{
    [Serializable]
    public class Entry
    {
        private List<Order> OrderList;
        private DateTime Datetime;
        public Status status { get; set; }
        public string Comment { get; set; }
        public string Orders { get => string.Join(Environment.NewLine, OrderList); }
        public string Date { get => Datetime.ToShortDateString(); }
    }
}
