using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManadger.Model
{
    [Serializable]
    public class Order
    {
        public string Seller { get; set; }
        public string Article { get; set; }
        public string Count { get; set; }
        public override string ToString()
        {
            return Seller + " -> " + Count + " -> " + Article;
        }
    }
}
