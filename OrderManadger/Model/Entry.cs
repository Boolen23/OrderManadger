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
        public Entry() { }
        public Entry(DateTime date, List<Order> _orderList, string comment, Status _status)
        {
            Datetime = date;
            OrderList = _orderList;
            Comment = comment;
            status = _status;
        }
        public int id;
        public List<Order> OrderList;
        public DateTime Datetime { get; set; }
        public Status status { get; set; }
        public string Comment { get; set; }
        public string Orders { get => string.Join(Environment.NewLine, OrderList); }
        public string Date { get => Datetime.ToShortDateString(); }
    }
}
